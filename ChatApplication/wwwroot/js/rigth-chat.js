/*
 * Заказчик своеобразный, требуйте ТЗ, и четко следуйте ему, посылайте НА при первой попытке: а вот тут не так.
 * 
 *
 */
var tokenKey = "accessToken";
var openPanelKey = 'openPanelKey';
var sessionToken = sessionStorage.getItem(tokenKey);
var id = findIdFromUrl();
var gcOut = 0;
var gPerf = { s: null, e: null };
var gStep = { s: null, e: null };
checkMessagesForUser();
setInterval(function () { RefreshToken(sessionToken); }, 55000);


// Обработчики  событий закрытия панели правого чата
$(document).ready(function () {

    $('.mask-content').click(function () {

        localStorage.setItem(openPanelKey, RigthChatApp.showRC);
        showRigthChat(RigthChatApp.showRC);
    });
    $('#right-chat-toggle').change(function () {
        RigthChatApp.showRC = document.getElementById('right-chat-toggle').checked;
        localStorage.setItem(openPanelKey, RigthChatApp.showRC);
    });

});
/**
 * Плавающая кнопка 
 */
var FloatMessageButton;
/**
 * Инициализируем приложение в момент загрузки DOM
 */
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

/**
 * Приложение чата.
 */
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
        query: "",
        showRC: true
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

            var file_data = $('#send_panel input[type="file"]').prop('files')[0];
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
            // animation: hideout 1.2s ease reverse;
            var container = $('.ms_container:visible ')[0];
            //animation: hideout .6s ease-in;
            var header = $('.nav h2:visible')[0];
            //animation: hideout_sp .6s ease-out;
            var send_panel = $('.send_panel')[0];
            var anonse_bar = $('.anonse_bar')[0];
            var anonse_bar_r = $('.anonse_bar .row')[0];

            var ulbar = $('.ul_btn_bar')[0];
            var ulbar1 = $('.ul_btn_bar')[1];


            container.style.webkitAnimation = "initial";
            header.style.webkitAnimation = 'initial';
            send_panel.style.webkitAnimation = 'initial';
            anonse_bar.style.webkitAnimation = 'initial';
            anonse_bar_r.style.webkitAnimation = 'initial';

            ulbar.style.webkitAnimation = 'initial';
            ulbar1.style.webkitAnimation = 'initial';

            setTimeout(function () {
                container.style.webkitAnimation = "hideout 1.2s ease reverse";
                anonse_bar_r.style.webkitAnimation = 'hideout_r 1.2s ease reverse';

            }, 10);
            setTimeout(function () {
                header.style.webkitAnimation = 'hideout .6s ease reverse';
                send_panel.style.webkitAnimation = 'hideout_sp .6s ease reverse';
                anonse_bar.style.webkitAnimation = 'hideout_ab .6s ease-in-out reverse';

                ulbar.style.webkitAnimation = 'hideout .6s ease-in reverse';
                ulbar1.style.webkitAnimation = 'hideout .6s ease-in reverse';
            }, 600);

            setTimeout(function () {
                container.style.webkitAnimation = "";
                header.style.webkitAnimation = '';
                send_panel.style.webkitAnimation = '';
                anonse_bar.style.webkitAnimation = '';
                anonse_bar_r.style.webkitAnimation = '';

                ulbar.style.webkitAnimation = '';
                ulbar1.style.webkitAnimation = '';

                RigthChatApp.showMessagePanel = false;
                RigthChatApp.actualtopic = null;
                RigthChatApp.getUserData();
            }, 1100);
        },
        /**
         * Добавляем символ перевода новой строки к сообщению.
         */
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
            var bottom = getComputedStyle(document.getElementById('rigth-chat-app')).bottom;
            console.log('gcOut: ' + gcOut + ' bottom: ' + bottom + ' RigthChatApp.showRC: ' + RigthChatApp.showRC);
            if (bottom == '0px') {
                RigthChatApp.showRC = true;
                console.log('2px');
            } else {
                console.log('bottom: ' + bottom);
            }

            console.log('closeRigthPanel: ' + RigthChatApp.showRC);
            RigthChatApp.showRC = !RigthChatApp.showRC;
            localStorage.setItem(openPanelKey, RigthChatApp.showRC);
            showRigthChat(RigthChatApp.showRC);
        },
        startUnread: function () {
            var req = $.ajax({
                type: 'GET',
                url: '/api/v1/user/startunread/' + loggedinUserId,
                success: function (data) {
                    var count = 0;
                    if (data)
                        count = parseInt(data);
                    if (FloatMessageButton)
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
        /**
         * Переключение размера чата. 
         */
        toggle: function () {
            console.group("min/max");
            var chat = document.getElementById('rigth-chat-app');
            var container = getVisibleScroolElement();

            var expand = false;
            var height = chat.style.height;
            if (height === "") {
                height = "60%";
            }
            if (height === "60%") {
                expand = true;
                RigthChatApp.application.state = RigthChatApp.application.stateMax;
            } else {
                RigthChatApp.application.state = RigthChatApp.application.stateMin;
            }

            window.console.log('Expand: ' + expand);
            /** Текущая высота чата в пикселях*/
            var chat100H = document.body.offsetHeight;
            /** 60% высоты чата в пикселях*/
            var chat60H = document.body.offsetHeight * .6;
            gPerf.s = performance.now();
            if (expand) {
                scrollStartExpand(container, chat, chat100H, chat100H - chat60H);
            } else {
                scrollChat(container, chat, chat60H, chat100H - chat60H);
            }
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


/**
 * Форматируем строку длительности перехода
 * @param {any} duration - длительность в микросекундах.
 */
function getDurationString(duration) {
    duration = duration / 1000;
    return "height " + duration + "s ease-in-out";
}

/**
 * Уменьшение чата вниз.
 * @param {any} container - Контейнер который изменяется
 * @param {any} chat - Элемент чата
 * @param {any} end - Конечная высота
 * @param {any} length - Разница в длине между высотами
 */
function scrollChat(container, chat, end, length) {
    console.log('scrollChat');
    var duration = 600;
    var step = 4;

    var initialHeight = chat.clientHeight;
    console.log("initial:" + initialHeight);

    gStep.s = performance.now();
    chat.style.transition = "";
    setTimeout(function () {
        if (RigthChatApp.showMessagePanel) {
            fixPosition(container, calculateMaxHeight(chat, container));
        }
        chat.style.transition = getDurationString(duration);
        chat.style.height = "60%";
    }, step);
    setTimeout(function () {
        chat.style.transition = "";
        resizeSpacer();
    }, duration + step);
}

/**
 * Фиксируем позицию чата, задавая позицию блока. 
 * @param {any} container - контейнер прокрутки
 * @param {any} maxHeight - максимальная высота контейнера
 */
function fixPosition(container, maxHeight) {

    console.log('maxHeight: ' + maxHeight);
    var sizer = container.firstElementChild;
    sizer.style.position = "fixed";
    sizer.style.bottom = "0px";    
    sizer.style.height = maxHeight + "px"; // тут должна быть полная высота контейнера.
    // container.screenTop = container.scrollHeight - container.clientHeight;
}


/**
 * Расширение чата. 600 ms, step 15
 * @param {any} container - контейнер ресайза
 * @param {any} chat - чат
 * @param { Number} end - Конечная высота в пикселях
 * @param { Number } length - Длина изменения
 */
function scrollStartExpand(container, chat, end, length) {
    console.log('scrollStartExpand');
    // from 60% -> 100%    
    var duration = 600;
    var step = 4;
    chat.style.transition = "";
    setTimeout(function () {
        if (RigthChatApp.showMessagePanel) {
            fixPosition(container, calculateMaxHeight(chat, container));
        }
        chat.style.transition = getDurationString(duration);
        chat.style.height = "100%";
    }, step);

    setTimeout(function () {
        chat.style.transition = "";
        resizeSpacer();
    }, duration + step);

    return;
}
/**
 * Вычисляет полную ывсоту контейнера
 * @param {any} chat
 * @param {any} container
 */
function calculateMaxHeight(chat, container) {
    var delta = chat.offsetHeight - container.offsetHeight;
    var docHeight = document.body.offsetHeight;
    return docHeight - delta;
}


/**
 * Получаем видимый контейнер.
 */
function getVisibleScroolElement() {
    return $('.ms_container:visible')[0];
}

/**
 * Прокрутка чата до нижней позиции.
 * @param {any} container
 */
function scroolToBottom(container) {

    console.log('container.scrollTop: ' + container.scrollTop);
    console.log('container.scrollHeight: ' + container.scrollHeight);
    console.log('container.clientHeight: ' + container.clientHeight);

    var sh = container.scrollHeight - container.clientHeight;
    console.log("sh %s", sh);
    if (!RigthChatApp.showMessagePanel) {
        container.scrollTop = 0;
    } else {
        container.scrollTop = sh;
    }
    return sh;
}
function clearTypingInterval() {
    RigthChatApp.typingData.id = null;
    RigthChatApp.typingData.topic = null;
}

RigthChatApp.startUnread();
RigthChatApp.getUserData();

var formatTimeCH = {};
Vue.filter('formatTime', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { hour: '2-digit', minute: '2-digit' }
        if (formatTimeCH[d] != undefined)
            return formatTimeCH[d];
        var str = d.toLocaleTimeString('ru-RU', opt);
        formatTimeCH[d] = str;
        return str;
    }
});
Vue.filter('formatMonthDay', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { day: 'numeric', month: 'short' }
        return d.toLocaleDateString('ru-RU', opt);
    }
});

var formatDateTimeCH = {};
Vue.filter('formatDateTime', function (value) {
    if (value) {
        var d = new Date(Date.parse(value));
        var opt = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' }
        if (formatDateTimeCH[d] != undefined)
            return formatDateTimeCH[d];
        var str = d.toLocaleDateString('ru-RU', opt);
        formatDateTimeCH[d] = str;
        return str;
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
    console.group("getMessagesForTopicRc");

    var sessionToken = sessionStorage.getItem(tokenKey);
    var container = getVisibleScroolElement();
    var top = $('.nav-header h2:visible')[0];
    var search = $('.rigth_chat_search_input')[0];

    var ulbar = $('.ul_btn_bar')[0];

    var ulbar1 = $('.ul_btn_bar')[1];



    container.style.webkitAnimation = "initial";
    top.style.webkitAnimation = "initial";
    search.style.webkitAnimation = "initial";
    ulbar.style.webkitAnimation = "initial";
    ulbar1.style.webkitAnimation = "initial";

    setTimeout(function () {
        container.style.webkitAnimation = "hideout 1.2s ease reverse";
    }, 10);
    setTimeout(function () {
        top.style.webkitAnimation = "hideout .6s ease-in reverse";
        search.style.webkitAnimation = "movetop .6s ease-out reverse";

        ulbar.style.webkitAnimation = 'hideout .6s ease-in reverse';
        ulbar1.style.webkitAnimation = 'hideout .6s ease-in reverse';
    }, 600);
    setTimeout(function () {
        top.style.opacity = 0;
        search.style.opacity = 0;
        ulbar.style.opacity = 0;
        ulbar1.style.opacity = 0;
    }, 600);

    $.ajax({
        type: 'GET',
        url: '/api/v1/user/messages/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            RigthChatApp.posts = data;
            setTimeout(function () {
                var template =
                    '<div class="viewbox-container width_sub_375"><div class="viewbox-body"><div class="viewbox-header"></div><div class="viewbox-content"></div><div class="viewbox-footer"></div></div></div>';
                $(".litebox").viewbox({ template: template, navButtons: false, nextOnContentClick: false });
            }, 1200);

            setTimeout(function () {
                container.style.webkitAnimation = "";
                top.style.webkitAnimation = "";
                search.style.webkitAnimation = "";

                ulbar.style.webkitAnimation = "";
                ulbar1.style.webkitAnimation = "";

                top.style.opacity = 1;
                search.style.opacity = 1;

                ulbar.style.opacity = 1;
                ulbar1.style.opacity = 1;

                RigthChatApp.showMessagePanel = true;
                //setTimeout(function () { resizeSpacer(); }, 20);
                setTimeout(function () {
                     var sc = getVisibleScroolElement();
                    scroolToBottom(sc);
                    resizeSpacer();
                }, 300);
            }, 600);
        }
    });
}

/**
 * Подгоняем высоту распорки, для прижатия сообщений к нижнему краю
 */
/***
 * Расширяем контейнер для подгонки высоты отдельного элемента.
 */
/**
 * Изменение контейнера заполнителя, для подгонки высоты под контейнер.
 */
function resizeSpacer() {
    console.log("resizeSpacer");
    var container = document.querySelector(".message_container");
    var spacer = document.querySelector(".spike-nail");
    var child = container.firstElementChild.children;
    console.log('container.scrollHeight: %i', container.scrollHeight);
    if (container.scrollHeight > container.clientHeight) {
        spacer.style.height = 0;
    }

    else {
        var space = 0;
        for (var i = 0; i < child.length; i++) {
            if (child[i] !== spacer)
                space += child[i].offsetHeight;
        }
        spacer.style.height = "calc(100% - " + space + "px)";
    }
    console.groupEnd();
}

// Отправка сообщения в топик
function sendMessageToTopicRc(body, topicId) {

    if (topicId == null) return;
    var sessionToken = sessionStorage.getItem(tokenKey);
    console.log("sendMessageToTopicRc");
    console.log(body);
    var container = getVisibleScroolElement();
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
                scroolToBottom(container);
            }, 300);
            if (isFileSelectedRc()) {
                console.log('Topicid ' + topicId);
                uploadFileRc(topicId, data.id);
            }
            var arr = RigthChatApp.topics;
            RigthChatApp.topics = sortTopics(arr, topicId);
            console.log(data);
            RigthChatApp.posts.push(data);
            setTimeout(resizeSpacer, 4);
        }
    });
}
// есть ли файл в форме отправки
function isFileSelectedRc() {
    try {

        return $('#send_panel input[type="file"]').prop('files').length > 0;
    }
    catch (e) {
        console.log(e)
        return false;
    }
}
// загрузка файла
function uploadFileRc(topicid, messageid) {

    var file_data = $('#send_panel input[type="file"]').prop('files')[0];
    var form_data = new FormData();
    form_data.append('uploads', file_data);
    RigthChatApp.showLoader = true;
    var container = getVisibleScroolElement();
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
            $('#send_panel input[type="file"]').val('')
            RigthChatApp.showLoader = false;
            assignAttacmentToMessageRc(messageid, data);
            setTimeout(function () {
                scroolToBottom(container);
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

function showRigthChat(ifShow, id) {
    console.log('switch chat ' + ifShow);
    RigthChatApp.showRC = ifShow;
    RigthChatApp.application.state = RigthChatApp.application.stateMin;
    //$('#rigth-chat-app').height('60%');

    var st = localStorage.getItem(openPanelKey);
    if (st != null) {
        ifShow = JSON.parse(st);
    }
    if (id) {
        ifShow = true;
        createTopic(id);
        RigthChatApp.showThread(id);
    }
    if (ifShow) {
        document.getElementById('right-chat-toggle').checked = 'checked';
        //RigthChatApp.maximize();
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
    var container = getVisibleScroolElement();
    var req = $.ajax({
        type: 'GET',
        url: '/api/v1/user/messages/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {
            ChatApp.posts = data;

            setTimeout(function () {
                scroolToBottom(container);
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

//Идентификатор чата на основе переданной строки
function findIdFromUrl() {
    console.log('Topic id from query string: ' + window.location.search);

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


/*--------------Library code----------------*/
/**
 * BezierEasing - use bezier curve for transition easing function
 * by Gaëtan Renaudeau 2014 – MIT License
 *
 * Credits: is based on Firefox's nsSMILKeySpline.cpp
 * Usage:
 * var spline = BezierEasing(0.25, 0.1, 0.25, 1.0)
 * spline(x) => returns the easing value | x must be in [0, 1] range
 *
 */
(function (definition) {
    if (typeof exports === "object") {
        module.exports = definition();
    } else if (typeof define === 'function' && define.amd) {
        define([], definition);
    } else {
        window.BezierEasing = definition();
    }
}(function () {
    var global = this;

    // These values are established by empiricism with tests (tradeoff: performance VS precision)
    var NEWTON_ITERATIONS = 4;
    var NEWTON_MIN_SLOPE = 0.001;
    var SUBDIVISION_PRECISION = 0.0000001;
    var SUBDIVISION_MAX_ITERATIONS = 10;

    var kSplineTableSize = 11;
    var kSampleStepSize = 1.0 / (kSplineTableSize - 1.0);

    var float32ArraySupported = 'Float32Array' in global;

    function A(aA1, aA2) { return 1.0 - 3.0 * aA2 + 3.0 * aA1; }
    function B(aA1, aA2) { return 3.0 * aA2 - 6.0 * aA1; }
    function C(aA1) { return 3.0 * aA1; }

    // Returns x(t) given t, x1, and x2, or y(t) given t, y1, and y2.
    function calcBezier(aT, aA1, aA2) {
        return ((A(aA1, aA2) * aT + B(aA1, aA2)) * aT + C(aA1)) * aT;
    }

    // Returns dx/dt given t, x1, and x2, or dy/dt given t, y1, and y2.
    function getSlope(aT, aA1, aA2) {
        return 3.0 * A(aA1, aA2) * aT * aT + 2.0 * B(aA1, aA2) * aT + C(aA1);
    }

    function binarySubdivide(aX, aA, aB) {
        var currentX, currentT, i = 0;
        do {
            currentT = aA + (aB - aA) / 2.0;
            currentX = calcBezier(currentT, mX1, mX2) - aX;
            if (currentX > 0.0) {
                aB = currentT;
            } else {
                aA = currentT;
            }
        } while (Math.abs(currentX) > SUBDIVISION_PRECISION && ++i < SUBDIVISION_MAX_ITERATIONS);
        return currentT;
    }

    function BezierEasing(mX1, mY1, mX2, mY2) {
        // Validate arguments
        if (arguments.length !== 4) {
            throw new Error("BezierEasing requires 4 arguments.");
        }
        for (var i = 0; i < 4; ++i) {
            if (typeof arguments[i] !== "number" || isNaN(arguments[i]) || !isFinite(arguments[i])) {
                throw new Error("BezierEasing arguments should be integers.");
            }
        }
        if (mX1 < 0 || mX1 > 1 || mX2 < 0 || mX2 > 1) {
            throw new Error("BezierEasing x values must be in [0, 1] range.");
        }

        var mSampleValues = float32ArraySupported ? new Float32Array(kSplineTableSize) : new Array(kSplineTableSize);

        function newtonRaphsonIterate(aX, aGuessT) {
            for (var i = 0; i < NEWTON_ITERATIONS; ++i) {
                var currentSlope = getSlope(aGuessT, mX1, mX2);
                if (currentSlope === 0.0) return aGuessT;
                var currentX = calcBezier(aGuessT, mX1, mX2) - aX;
                aGuessT -= currentX / currentSlope;
            }
            return aGuessT;
        }

        function calcSampleValues() {
            for (var i = 0; i < kSplineTableSize; ++i) {
                mSampleValues[i] = calcBezier(i * kSampleStepSize, mX1, mX2);
            }
        }

        function getTForX(aX) {
            var intervalStart = 0.0;
            var currentSample = 1;
            var lastSample = kSplineTableSize - 1;

            for (; currentSample != lastSample && mSampleValues[currentSample] <= aX; ++currentSample) {
                intervalStart += kSampleStepSize;
            }
            --currentSample;

            // Interpolate to provide an initial guess for t
            var dist = (aX - mSampleValues[currentSample]) / (mSampleValues[currentSample + 1] - mSampleValues[currentSample]);
            var guessForT = intervalStart + dist * kSampleStepSize;

            var initialSlope = getSlope(guessForT, mX1, mX2);
            if (initialSlope >= NEWTON_MIN_SLOPE) {
                return newtonRaphsonIterate(aX, guessForT);
            } else if (initialSlope === 0.0) {
                return guessForT;
            } else {
                return binarySubdivide(aX, intervalStart, intervalStart + kSampleStepSize);
            }
        }

        var _precomputed = false;
        function precompute() {
            _precomputed = true;
            if (mX1 != mY1 || mX2 != mY2)
                calcSampleValues();
        }

        var f = function (aX) {
            if (!_precomputed) precompute();
            if (mX1 === mY1 && mX2 === mY2) return aX; // linear
            // Because JavaScript number are imprecise, we should guarantee the extremes are right.
            if (aX === 0) return 0;
            if (aX === 1) return 1;
            return calcBezier(getTForX(aX), mY1, mY2);
        };

        f.getControlPoints = function () { return [{ x: mX1, y: mY1 }, { x: mX2, y: mY2 }]; };

        var args = [mX1, mY1, mX2, mY2];
        var str = "BezierEasing(" + args + ")";
        f.toString = function () { return str; };

        var css = "cubic-bezier(" + args + ")";
        f.toCSS = function () { return css; };

        return f;
    }

    // CSS mapping
    BezierEasing.css = {
        "ease": BezierEasing(0.25, 0.1, 0.25, 1.0),
        "linear": BezierEasing(0.00, 0.0, 1.00, 1.0),
        "ease-in": BezierEasing(0.42, 0.0, 1.00, 1.0),
        "ease-out": BezierEasing(0.00, 0.0, 0.58, 1.0),
        "ease-in-out": BezierEasing(0.42, 0.0, 0.58, 1.0)
    };

    return BezierEasing;

}));

function fixH() {
    var child = document.querySelector('.message_container').querySelectorAll('*');
    var len = child.length;
    for (var i = 0; i < len; i++) {

        child[i].style.height = child[i].offsetHeight + 'px';
    }
}