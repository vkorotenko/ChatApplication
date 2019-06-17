var tokenKey = "accessToken";
var openPanelKey = 'openPanelKey';
var sessionToken = sessionStorage.getItem(tokenKey);
var id = findIdFromUrl();


checkMessagesForUser();
setInterval(function () { RefreshToken(sessionToken); }, 55000);


// Обработчики  событий закрытия панели правого чата
$(document).ready(function () {
    showRigthChat(showRigthChatPanel);

    $('.mask-content').click(function () {

        showRigthChatPanel = !showRigthChatPanel;
        localStorage.setItem(openPanelKey, showRigthChatPanel);
        showRigthChat(showRigthChatPanel);
    });
    $('#right-chat-toggle').change(function () {
        showRigthChatPanel = document.getElementById('right-chat-toggle').checked;
        localStorage.setItem(openPanelKey, showRigthChatPanel);
    });
});

var FloatMessageButton;
$(document).ready(function () {
    /**
     * Плавающая кнопка и индикатор.
     */
    FloatMessageButton = new Vue({
        el: '#floatMessageButton',
        data: {
            count: 0,
            showCounter: false
        },
        methods: {
            setCount: function (unread) {
                FloatMessageButton.count = unread;
                if (unread > 0)
                    FloatMessageButton.showCounter = true;
                else
                    FloatMessageButton.showCounter = false;
            }
        }
    });
    /*end*/
});



var RigthChatApp = new Vue({
    el: '#rigth-chat-app',
    data: {
        application: {
            state: 'maximized',
            stateMax: 'maximized',
            stateMin: 'minimized',
            typing: false,
            tipinginterval: null,
            localtypingdate: null
        },
        /**
         * Устанавливаем при получении данных о печати.
         */
        typingData: {
            id: null,
            name: null,
            topic: null,
            unread: null
        },
        unreadMessages: 0,
        actualtopic: null,
        username: "",
        user: {
            username: "",
            password: ""
        },
        aid: null,
        topics: [],
        posts: [],
        topicAuthor: 0,
        topicId: null,
        messageArea: "",
        userId: null,
        selectFileName: null,
        fileAdded: false,
        showLoader: false,
        showDeveloperConsole: false,
        mailRefresh: true,
        showSearchLoader: false,
        showMessagePanel: false,
        query: ""
    },
    methods: {
        showThread: function (id, el, event) {
            RigthChatApp.dayExist = [];
            RigthChatApp.actualtopic = findTopic(RigthChatApp.topics, id);
            RigthChatApp.topicId = id;
            selectItemRc(id);
            getMessagesForTopicRc(id, RigthChatApp.topicAuthor);
            clearNewMessagesRc(id);
        },
        sendMessage: function () {
            if (RigthChatApp.messageArea != "" || isFileSelectedRc())
                sendMessageToTopicRc(RigthChatApp.messageArea, RigthChatApp.topicId);
        },        
        getUserData: function () {
            GetUserDataRc();
        },
        fileSelected: function () {

            var file_data = $('#file-rch').prop('files')[0];
            console.log(file_data);
            RigthChatApp.fileAdded = true;
            RigthChatApp.selectFileName = file_data.name;
            // auto post after file added
            sendMessageToTopicRc(RigthChatApp.messageArea, RigthChatApp.topicId);
        },
        changeRefresh: function () {
            processRefresh = !processRefresh;
            RigthChatApp.mailRefresh = processRefresh;
        },
        search: function (event) {
            RigthChatApp.showSearchLoader = true;
            var query = event.target.value;
            searchTopicsRc(query);
        },
        backToTopics: function () {
            console.log('Back from topic');
            RigthChatApp.showMessagePanel = false;
            RigthChatApp.actualtopic = null;
            RigthChatApp.getUserData();
        },
        appendNewLine: function () {
            RigthChatApp.messageArea = RigthChatApp.messageArea + '\n';
        },
        setTyping: function (data) {
            console.log('setTyping');
            console.log(data);

            RigthChatApp.typingData.id = data.id;
            RigthChatApp.typingData.name = data.name;
            RigthChatApp.typingData.topic = data.topic;
            RigthChatApp.typingData.unread = data.unread;

            setTimeout(clearTypingInterval, 5000);
        },
        closePanel: function () {
            closeRigthPanel();
        },
        startUnread: function () {
            var req = $.ajax({
                type: 'GET',
                url: '/api/v1/user/startunread/' + loggedinUserId,
                success: function (data) {
                    var count = 0;
                    if (data)
                        count = parseInt(data);

                    FloatMessageButton.setCount(count);

                    if (count > 0) {
                        if (RigthChatApp && RigthChatApp.getUserData) RigthChatApp.getUserData();

                    } 
                }
            });

            req.fail(function (data, status) {
                console.log(status);
                if (data.status == 502) {
                    console.log('error 502');
                }
                console.log(data.status);
            });
        },
        resizeSendArea: function () {

            var ta = document.getElementById('ta_send_panel');
            var sp = document.getElementById('send_panel');

            var fs = parseInt($('#ta_send_panel').css('font-size'));
            var h = ta.value.length * fs * 0.7;
            var str = h / ta.clientWidth;
            sp.style.height = str * fs + 'px';
            RigthChatApp.typing();
        },
        collapse: function () {
            RigthChatApp.application.state = RigthChatApp.application.stateMax;
            $('#rigth-chat-app').height('100%');
        },
        maximize: function () {
            RigthChatApp.application.state = RigthChatApp.application.stateMin;
            $('#rigth-chat-app').height('60%');
        },
        typing: function () {
            var tm = RigthChatApp.localtypingdate;
            var ct = new Date().getTime();

            if (tm == null || tm < ct) {
                var url = '/api/v1/user/typing/' + RigthChatApp.actualtopic.id;
                console.log('typing: ' + url);
                RigthChatApp.localtypingdate = ct + 5000;
                $.ajax({
                    type: 'POST',
                    url: url,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
                    },
                    success: function () {
                    }
                });
            }

        }
    }
});

function clearTypingInterval() {
    RigthChatApp.typingData.id = null;
    RigthChatApp.typingData.topic = null;
}

RigthChatApp.startUnread();
RigthChatApp.getUserData();


Vue.filter('formatTime', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { hour: '2-digit', minute: '2-digit' }
        return d.toLocaleTimeString('ru-RU', opt);
    }
});
Vue.filter('formatMonthDay', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { day: 'numeric', month: 'short' }
        return d.toLocaleDateString('ru-RU', opt);
    }
});
Vue.filter('formatDateTime', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' }
        return d.toLocaleDateString('ru-RU', opt);
    }
});

Vue.filter('formatDate', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { year: 'numeric', month: 'short', day: 'numeric' }
        return d.toLocaleDateString('ru-RU', opt);
    }
});

Vue.filter('formatMonthDayEx', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var cur = new Date();
        var opt = { day: 'numeric', month: 'short' };

        // Сегодня
        if (cur.getFullYear() == d.getFullYear() &&
            cur.getMonth() == d.getMonth() &&
            cur.getDate() == d.getDate()) {

            opt = { hour: '2-digit', minute: '2-digit' };
            return 'сегодня ' + d.toLocaleTimeString('ru-RU', opt);
        }
        if (cur.getFullYear() == d.getFullYear() &&
            cur.getMonth() == d.getMonth() &&
            cur.getDate() == (d.getDate() + 1)) {

            opt = { hour: '2-digit', minute: '2-digit' };
            return 'вчера ' + d.toLocaleTimeString('ru-RU', opt);
        }
        return d.toLocaleDateString('ru-RU', opt);
    }
});

Vue.filter('striphtml', function (value) {
    var div = document.createElement("div");
    div.innerHTML = value;
    var text = div.textContent || div.innerText || "";
    return text;
});

Vue.filter('fileSize', function (value) {

    var int = parseInt(value);
    var kb = Math.floor(int / 1024);
    var mb = Math.floor(int / (1024 * 1024));

    var result;
    if (int < 1024) {
        result = '1 КБ';
    }
    else if (kb > 0 && kb < 1024) {
        result = kb + 1 + ' КБ';
    }
    else if (mb >= 1) {
        result = mb + 1 + ' МБ';
    }
    return result;
});

Vue.filter('getIcon', function (value) {
    var chk = value.split('.');
    var ext = chk[chk.length - 1];
    return '/img/rc/ico/' + ext + '.png';
});


function findTopic(array, id) {
    for (i = 0; i < array.length; i++) {
        if (array[i].id == id) return array[i];
    }
    return null;
}
function GetUserDataRc() {
    var sessionToken = sessionStorage.getItem(tokenKey);
    if (id > -1) createTopic(id);
    $.ajax({
        type: 'GET',
        url: '/api/v1/user',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            RigthChatApp.topics = applyTopicsSelectedRc(data.topics);
            RigthChatApp.unreadMessages = data.messages;
            RigthChatApp.user.username = data.username;
            RigthChatApp.username = data.username;
            RigthChatApp.aid = data.id;
            if (RigthChatApp.actualtopic) {
                var id = RigthChatApp.actualtopic.id;
                console.log('RC getMessagesForTopicRc: ' + id);
                getMessagesForTopicRc(id, RigthChatApp.topicAuthor);
                clearNewMessagesRc(id);
            }
        }
    });
}
function applyTopicsSelectedRc(topics) {
    for (i = 0; i < topics.length; i++) {
        topics[i].selected = false;
    }
    return topics;
}
function searchTopicsRc(query) {
    var sessionToken = sessionStorage.getItem(tokenKey);

    $.ajax({
        type: 'GET',
        url: '/api/v1/user/search/' + query,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            RigthChatApp.topics = applyTopicsSelectedRc(data.topics);
            RigthChatApp.unreadMessages = data.messages;
            RigthChatApp.user.username = data.username;
            RigthChatApp.username = data.username;
        }
    });
}
function selectItemRc(id) {
    for (i = 0; i < RigthChatApp.topics.length; i++) {
        RigthChatApp.topics[i].selected = false;
        if (RigthChatApp.topics[i].id == id) {
            RigthChatApp.topicAuthor = RigthChatApp.topics[i].authorId;
            RigthChatApp.topics[i].selected = true;
        }
    }
}
function getMessagesForTopicRc(id, authorId) {
    var sessionToken = sessionStorage.getItem(tokenKey);
    $.ajax({
        type: 'GET',
        url: '/api/v1/user/messages/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {



            RigthChatApp.posts = data;
            setTimeout(function () {
                var sh = $('.ms_container:visible')[0].scrollHeight + 9999999;
                $('.ms_container:visible').scrollTop(sh);
                var template =
                    '<div class="viewbox-container width_sub_375"><div class="viewbox-body"><div class="viewbox-header"></div><div class="viewbox-content"></div><div class="viewbox-footer"></div></div></div>';
                $(".litebox").viewbox({ template: template, navButtons: false, nextOnContentClick: false });
            }, 300);
            RigthChatApp.showMessagePanel = true;
        }
    });
}
// Отправка сообщения в топик
function sendMessageToTopicRc(body, topicId) {

    if (topicId == null) return;
    var sessionToken = sessionStorage.getItem(tokenKey);
    console.log('sendMessageToTopicRc');
    console.log(body);
    var bd = {
        body: body
    };
    $.ajax({
        type: 'POST',
        url: '/api/v1/user/addtotopic/' + topicId,
        data: JSON.stringify(bd),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            RigthChatApp.messageArea = "";

            setTimeout(function () {
                var sh = $('.ms_container:visible')[0].scrollHeight + 9999999;
                console.log(sh);
                $('.ms_container:visible').scrollTop(sh);
            }, 300);
            if (isFileSelectedRc()) {
                console.log('Topicid ' + topicId);
                uploadFileRc(topicId, data.id);
            }
            var arr = RigthChatApp.topics;
            RigthChatApp.topics = sortTopics(arr, topicId);
            console.log(data);
            RigthChatApp.posts.push(data);
        }
    });
}
// есть ли файл в форме отправки
function isFileSelectedRc() {
    try {
        return $('#file-rch').prop('files').length > 0;
    }
    catch (e) {
        console.log(e)
        return false;
    }
}
// загрузка файла
function uploadFileRc(topicid, messageid) {
    var file_data = $('#file-rch').prop('files')[0];
    var form_data = new FormData();
    form_data.append('uploads', file_data);
    RigthChatApp.showLoader = true;
    var req = $.ajax({
        type: 'POST',
        // addfiles/{topicid}/{ messageid }
        url: "/api/v1/user/addfiles/" + topicid + "/" + messageid,
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            RigthChatApp.fileAdded = false;
            RigthChatApp.selectFileName = null;
            RigthChatApp.showLoader = false;
            assignAttacmentToMessageRc(messageid, data);
            setTimeout(function () {
                var sh = $('.ms_container:visible')[0].scrollHeight + 9999999;
                $('.ms_container:visible').scrollTop(sh);
            }, 300);
        },
        error: function (data) {
            RigthChatApp.showLoader = false;
        }
    });
    req.fail(function (data, status) {
        console.log(status);
        RigthChatApp.showLoader = false;
        if (data.status == 415) {
            alert(data.responseText);
            RigthChatApp.posts.pop();
        }
        if (data.status == 401) {
            var te = data.getResponseHeader('Token-Expired');
            if (te) {
                RefreshToken(sessionToken);
                sendMessageToTopicRc(body, userId, topicId);
            }
        } else
            console.log('err');
    });
}
// Добавляем пришедший файл к сообщению.
function assignAttacmentToMessageRc(messageid, data) {
    for (var i = 0; i < RigthChatApp.posts.length; i++) {
        var item = RigthChatApp.posts[i];
        if (item && item.id == messageid) {
            item.attachment = data;
        }
    }
}
function clearNewMessagesRc(id) {
    var token = sessionStorage.getItem(tokenKey);
    var req = $.ajax({
        type: 'POST',
        url: '/api/v1/user/clearmessages/' + id,
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            var topics = RigthChatApp.topics;
            for (i = 0; i < topics.length; i++) {
                if (topics[i].id == id) {
                    topics[i].hasMessages = false;
                    topics[i].unread = null;
                }
            }
        }
    });
}
function sortTopics(array, id) {
    for (i = 0; i < array.length; i++) {
        if (array[i].id == id) {
            array[i].updated = Date().toLocaleString();
            console.log(Date().toLocaleString());
            break;
        }
    }
    var na = array.sort(function (a, b) {
        var x = new Date(a.updated) > new Date(b.updated) ? -1 : 1;
        return x;
    });
    return na;
}

function closeRigthPanel() {
    showRigthChatPanel = !showRigthChatPanel;
    localStorage.setItem(openPanelKey, showRigthChatPanel);
    showRigthChat(showRigthChatPanel);
}

function showRigthChat(ifShow, id) {
    console.log('switch chat ' + ifShow);
    showRigthChatPanel = ifShow;

    RigthChatApp.application.state = RigthChatApp.application.stateMin;
    $('#rigth-chat-app').height('60%');

    var st = localStorage.getItem(openPanelKey);
    if (st != null) {
        ifShow = JSON.parse(st);
    }
    if (id) {
        ifShow = true;
        RigthChatApp.showThread(id);
    }
    if (ifShow) {
        document.getElementById('right-chat-toggle').checked = 'checked';
        RigthChatApp.maximize();
    } else {
        document.getElementById('right-chat-toggle').checked = false;
    }
};
function checkMessagesForUser() {
    if (!processRefresh)
        setTimeout(checkMessagesForUser, 500);
    var req = $.ajax({
        type: 'GET',
        url: '/api/v1/user/totalunread/' + loggedinUserId,
        success: function (data) {
            var count = 0;
            if (data.unread)
                count = data.unread;
            if (data.name && data.name != "") {
                RigthChatApp.setTyping(data);
            }
            var button = $('.nav-toggle');

            FloatMessageButton.setCount(count);
            if (count > 0) {
                console.log('Unread: ' + count);
                if (RigthChatApp && RigthChatApp.getUserData) RigthChatApp.getUserData();
            }
            setTimeout(checkMessagesForUser, 500);
        }
    });

    req.fail(function (data, status) {
        console.log(status);
        if (data.status == 502) {
            console.log('error 502');
        }
        setTimeout(checkMessagesForUser, 500);
    });

}
function RefreshToken(token) {
    var sessionToken = sessionStorage.getItem(tokenKey);
    var bd = { token: sessionToken };
    $.ajax({
        type: 'POST',
        url: '/api/v1/account/refresh',
        data: JSON.stringify(bd),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            sessionStorage.setItem(tokenKey, data.access_token);
            sessionToken = data.access_token;
        }
    });
}



function selectItem(id) {
    for (i = 0; i < ChatApp.topics.length; i++) {
        ChatApp.topics[i].selected = false;
        if (ChatApp.topics[i].id == id) {
            ChatApp.topicAuthor = ChatApp.topics[i].authorId;
            ChatApp.topics[i].selected = true;
        }
    }
}
function applyTopicsSelected(topics) {
    for (i = 0; i < topics.length; i++) {
        topics[i].selected = false;
    }
    return topics;
}
function getMessagesForTopic(id, authorId) {
    var sessionToken = sessionStorage.getItem(tokenKey);
    var req = $.ajax({
        type: 'GET',
        url: '/api/v1/user/messages/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            ChatApp.posts = data;

            setTimeout(function () {
                var sh = $('.ms_container:visible')[0].scrollHeight + 9999999;
                $('.ms_container:visible').scrollTop(sh);
            }, 300);
        }
    });

    req.fail(function (data, status) {
        if (data.status == 401) {
            var te = data.getResponseHeader('Token-Expired');
            if (te) {
                RefreshToken(sessionToken);
                getMessagesForTopic(id);
            }
        }
        else
            console.log('err');
    });
}

// Отправка сообщения в топик
function sendMessageToTopic(body, topicId) {

    if (topicId == null) return;
    var sessionToken = sessionStorage.getItem(tokenKey);
    var bd = {
        body: body
    };
    var req = $.ajax({
        type: 'POST',
        url: '/api/v1/user/addtotopic/' + topicId,
        data: JSON.stringify(bd),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            ChatApp.messageArea = "";

            if (isFileSelected()) {
                uploadFile(ChatApp.topicId, data.id);
            }
            var arr = ChatApp.topics;
            ChatApp.topics = sortTopics(arr, topicId);
            ChatApp.posts.push(data);
        }
    });
    req.fail(function (data, status) {
        if (data.status == 401) {
            var te = data.getResponseHeader('Token-Expired');
            if (te) {
                RefreshToken(sessionToken);
                sendMessageToTopic(body, userId, topicId);
            }
        } else
            console.log('err');
    });
}


// есть ли файл в форме отправки
function isFileSelected() {
    return $('#file-1').prop('files').length > 0;
}
// загрузка файла
function uploadFile(topicid, messageid) {
    var file_data = $('#file-1').prop('files')[0];
    var form_data = new FormData();
    form_data.append('uploads', file_data);
    ChatApp.showLoader = true;
    var req = $.ajax({
        type: 'POST',
        // addfiles/{topicid}/{ messageid }
        url: "/api/v1/user/addfiles/" + topicid + "/" + messageid,
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            ChatApp.fileAdded = false;
            ChatApp.selectFileName = null;
            ChatApp.showLoader = false;
            assignAttacmentToMessage(messageid, data);
        },
        error: function (data) {
            ChatApp.showLoader = false;
        }
    });
    req.fail(function (data, status) {
        console.log(status);
        ChatApp.showLoader = false;
        if (data.status == 415) {

            alert(data.responseText);
            ChatApp.posts.pop();
        }
        if (data.status == 401) {
            var te = data.getResponseHeader('Token-Expired');
            if (te) {
                RefreshToken(sessionToken);
                sendMessageToTopic(body, userId, topicId);
            }
        } else
            console.log('err');
    });
}

// Добавляем пришедший файл к сообщению.
function assignAttacmentToMessage(messageid, data) {
    for (var i = 0; i < ChatApp.posts.length; i++) {
        var item = ChatApp.posts[i];
        if (item && item.id == messageid) {
            item.attachment = data;
        }
    }
}
// Очистка флага новых сообщений в топике

function clearNewMessages(id) {
    var token = sessionStorage.getItem(tokenKey);
    var req = $.ajax({
        type: 'POST',
        url: '/api/v1/user/clearmessages/' + id,
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: function (data) {
            //GetUserData();            
        }
    });
}


function findIdFromUrl() {
    console.log(window.location.search);

    var search = window.location.search;
    var id = -1;
    if (search != "") {
        search = search.replace('?', '');
        var arr = search.split('&');
        for (i = 0; i < arr.length; i++) {
            var kv = arr[i].split('=');
            if (kv.length == 2) {
                if (kv[0].toLowerCase() == 'id') {
                    id = parseInt(kv[1]);
                    console.log('id: ' + id);
                }
            }
        }
    }
    return id;
}

// создание нового топика
function createTopic(id) {
    var sessionToken = sessionStorage.getItem(tokenKey);
    $.ajax({
        type: 'GET',
        url: '/api/v1/user/createtopic/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function () {
            console.log('Created topic from article id: ' + id);
        }
    });
}

// 
function searchTopics(query) {
    var sessionToken = sessionStorage.getItem(tokenKey);

    var req = $.ajax({
        type: 'GET',
        url: '/api/v1/user/search/' + query,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            ChatApp.topics = applyTopicsSelected(data.topics);
            ChatApp.unreadMessages = data.messages;
            ChatApp.user.username = data.username;
            ChatApp.username = data.username;
            ChatApp.showSearchLoader = false;
        }
    });
    req.fail(function (data, status) {
        console.log('Failed search query');
        console.log(data);
        ChatApp.showSearchLoader = false;
    });
}
// производится вход, получение тикета и обновление сессии, так же обновляется идентификатор
function specialLogin(username, password) {
    var tokenKey = "accessToken";
    var loginData = { userName: username, password: password };
    $.ajax({
        type: 'POST',
        contentType: "application/json",
        url: '/api/v1/Account/token',
        data: JSON.stringify(loginData),
        success: function (data) {
            sessionStorage.setItem(tokenKey, data.access_token);
            ChatApp.username = data.username;
            ChatApp.id = data.id;
            if (loggedinUserId) loggedinUserId = data.id;
            RigthChatApp.id = data.id;

            RigthChatApp.getUserData();
            ChatApp.getUserData();
            console.log(data);
        }
    });
}

function sortTopics(array, id) {
    for (i = 0; i < array.length; i++) {
        if (array[i].id == id) {
            array[i].updated = Date().toLocaleString();
            console.log(Date().toLocaleString());
            break;
        }
    }
    var na = array.sort(function (a, b) {
        var x = new Date(a.updated) > new Date(b.updated) ? -1 : 1;
        return x;
    });
    return na;
}




