﻿var tokenKey = "accessToken";
var openPanelKey = 'openPanelKey';
var sessionToken = sessionStorage.getItem(tokenKey);
var id = findIdFromUrl();

setInterval(checkMessagesForUser, 1000);
setInterval(function () { RefreshToken(sessionToken); }, 55000);

var RigthChatApp = new Vue({
    el: '#rigth-chat-app',
    data: {
        unreadMessages: 0,
        actualtopic: null,
        username: "",
        user: {
            username: "",
            password: ""
        },
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
        showMessagePanel: false
    },
    methods: {
        showThread: function (id, el, event) {
            RigthChatApp.actualtopic = findTopic(RigthChatApp.topics, id);
            RigthChatApp.showMessagePanel = true;
            RigthChatApp.topicId = id;
            selectItemRc(id);
            getMessagesForTopicRc(id, RigthChatApp.topicAuthor);
            clearNewMessagesRc(id);
            RigthChatApp.getUserData();
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
            RigthChatApp.showMessagePanel = false;
        },
        appendNewLine: function() {
            RigthChatApp.messageArea = RigthChatApp.messageArea + '\n';
        }
    }
});
RigthChatApp.getUserData();
var NewMessagesInformer = new Vue({
    el: '#new-messages-informer',
    data: {
        items: [],
        unread: 0
    },
    methods: {
        getData: function () {
            var sessionToken = sessionStorage.getItem(tokenKey);
            $.ajax({
                type: 'GET',
                url: '/api/v1/user/getlatestmessages',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
                },
                success: function (data) {
                    NewMessagesInformer.items = data;
                }
            });
        }
    }
});
Vue.filter('formatTime', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { hour: '2-digit', minute: '2-digit' }
        return d.toLocaleTimeString('ru-RU', opt);
    }
});
Vue.filter('formatDateTime', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { year: 'numeric', mounth: '2-digit', day: 'numeric', hour: '2-digit', minute: '2-digit' }
        return d.toLocaleDateString('ru-RU', opt);
    }
});

GetUserDataRc();
function findTopic(array, id) {
    for (i = 0; i < array.length; i++) {
        if (array[i].id == id) return array[i];
    }
    return null;
}
function GetUserDataRc() {
    var sessionToken = sessionStorage.getItem(tokenKey);
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
    var req = $.ajax({
        type: 'GET',
        url: '/api/v1/user/messages/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {

            for (i = 0; i < data.length; i++) {
                if (data[i].authorId == authorId) data[i].isAuthor = true;
                else data[i].isAuthor = false;
            }

            RigthChatApp.posts = data;
            setTimeout(function () {
                $('.msg_h').scrollTop(99999);
            }, 300);
        }
    });

    req.fail(function (data, status) {
        if (data.status == 401) {
            var te = data.getResponseHeader('Token-Expired');
            if (te) {
                RefreshToken(sessionToken);
                getMessagesForTopicRc(id);
            }
        }
        else
            console.log('err');
    });
}
// Отправка сообщения в топик
function sendMessageToTopicRc(body, topicId) {

    if (topicId == null) return;
    var sessionToken = sessionStorage.getItem(tokenKey);
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
            data.isAuthor = true;
            setTimeout(function () {
                $('.msg_h').scrollTop(99999);
            }, 300);
            if (isFileSelectedRc()) {
                console.log('Topicid ' + topicId);
                uploadFileRc(topicId, data.id);
            }
            var arr = RigthChatApp.topics;
            RigthChatApp.topics = sortTopics(arr, topicId);
            RigthChatApp.posts.push(data);
        }
    });
}
// есть ли файл в форме отправки
function isFileSelectedRc() {
    return $('#file-rch').prop('files').length > 0;
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
                $('.msg_h').scrollTop(99999);
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

// Обработчики  событий закрытия панели правого чата
$(document).ready(function () {
    showRigthChat(showRigthChatPanel);

    $('#close-span').click(function () {

        showRigthChatPanel = !showRigthChatPanel;        
        localStorage.setItem(openPanelKey, showRigthChatPanel);
        showRigthChat(showRigthChatPanel);
    });
    $('#right-chat-toggle').change(function () {
        showRigthChatPanel = document.getElementById('right-chat-toggle').checked;
        localStorage.setItem(openPanelKey, showRigthChatPanel);
    });
});



function showRigthChat(ifShow) {

    showRigthChatPanel = ifShow;
    var st = localStorage.getItem(openPanelKey);
    if (st != null) {
        ifShow = JSON.parse(st);
    }
    if (ifShow) {
        document.getElementById('right-chat-toggle').checked = 'checked';
    } else {
        document.getElementById('right-chat-toggle').checked = false;
    }
};
function checkMessagesForUser() {
    if (!processRefresh) return;
    $.ajax({
        type: 'GET',
        url: '/api/v1/user/totalunread/' + loggedinUserId,
        success: function (data) {
            var count = parseInt(data);

            if (count > 0) {
                totalMessagesSpan.text(count);
                totalMessagesSpan.show();
                totalMessagesSpanRc.text(count);
                totalMessagesSpanRc.show();
                if (ChatApp && ChatApp.getUserData) ChatApp.getUserData();
                if (RigthChatApp && RigthChatApp.getUserData) RigthChatApp.getUserData();
                if (NewMessagesInformer && NewMessagesInformer.getData) NewMessagesInformer.getData();
            } else {
                totalMessagesSpan.hide();
                totalMessagesSpanRc.hide();
            }
        }
    });
}
function RefreshToken(token) {
    var bd = { token: token };
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


var ChatApp = new Vue({
    el: '#chatApp',
    data: {
        unreadMessages: 0,
        username: "",
        user: {
            username: "",
            password: ""
        },
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
        showSearchLoader: false
    },
    methods: {
        showThread: function (id, el, event) {
            ChatApp.actualtopic = findTopic(ChatApp.topics, id);
            ChatApp.topicId = id;
            selectItem(id);
            getMessagesForTopic(id, ChatApp.topicAuthor);
            clearNewMessages(id);
        },
        sendMessage: function () {
            if (ChatApp.messageArea != "" || isFileSelected())
                sendMessageToTopic(ChatApp.messageArea, ChatApp.topicId);
        },
        getUserData: function () {
            GetUserData();
        },
        fileSelected: function () {

            var file_data = $('#file-1').prop('files')[0];
            console.log(file_data);
            ChatApp.fileAdded = true;
            ChatApp.selectFileName = file_data.name;
        },
        changeRefresh: function () {
            processRefresh = !processRefresh;
            ChatApp.mailRefresh = processRefresh;
        },
        newLogin: function () {
            var user = ChatApp.user;
            specialLogin(user.username, user.password);
        },
        search: function (event) {
            ChatApp.showSearchLoader = true;
            var query = event.target.value;
            searchTopics(query);
        }
    }
});

if (id > -1) {
    GetUserData(id);
} else GetUserData();

// консоль отладки
function keyUpHandlerCtrlShiftL(e) {
    if (e.code == "KeyL" && e.ctrlKey && e.shiftKey) {
        ChatApp.showDeveloperConsole = !ChatApp.showDeveloperConsole;
        if (ChatApp.showDeveloperConsole) {
            processRefresh = ChatApp.mailRefresh = false;
        }
    }
}
function GetUserData(id) {
    var sessionToken = sessionStorage.getItem(tokenKey);
    if (id) createTopic(id);
    var req = $.ajax({
        type: 'GET',
        url: '/api/v1/user',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            ChatApp.topics = applyTopicsSelected(data.topics);
            ChatApp.unreadMessages = data.messages;
            ChatApp.user.username = data.username;
            ChatApp.username = data.username;

            if (ChatApp.actualtopic) {
                var id = ChatApp.actualtopic.id;
                console.log('Chat getMessagesForTopic: ' + id);
                getMessagesForTopic(id, ChatApp.topicAuthor);
            }
            if (id) {
                ChatApp.showThread(id);
            }
            window.addEventListener('keyup', keyUpHandlerCtrlShiftL);
        }
    });

    req.fail(function (data, status) {
        if (data.status == 401) {
            var te = data.getResponseHeader('Token-Expired');
            if (te) {
                RefreshToken(sessionToken);
                GetUserData();
            }
        }
        else
            console.log('err');
    });
}
function RefreshToken(token) {
    var bd = { token: token };
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

            for (i = 0; i < data.length; i++) {
                if (data[i].authorId == authorId) data[i].isAuthor = true;
                else data[i].isAuthor = false;
            }

            ChatApp.posts = data;

            setTimeout(function () {
                $('.messager__body').scrollTop(99999);
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
            data.isAuthor = true;
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
            //ChatApp.showThread(id);
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
            console.log(data);
            ChatApp.getUserData();
            if (loggedinUserId) loggedinUserId = data.id;
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