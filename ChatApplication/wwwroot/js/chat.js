var tokenKey = "accessToken";
var sessionToken = sessionStorage.getItem(tokenKey);

var ChatApp = new Vue({
    el: '#chatApp',
    data: {
        unreadMessages: 0,
        topics: [],
        posts: [],
        topicAuthor: 0,
        topicId: null,
        messageArea: "",
        userId: null
    },
    methods: {
        showThread: function (id, el, event) {
            ChatApp.topicId = id;
            selectItem(id);
            getMessagesForTopic(id, ChatApp.topicAuthor);
            clearNewMessages(id);
        },
        sendMessage: function () {
            if (ChatApp.messageArea != "")
                sendMessageToTopic(ChatApp.messageArea, ChatApp.topicId);
        },
        getUserData: function() {
            GetUserData();
        }
    }
});

Vue.filter('formatTime', function (value) {
    if (value) {
        var date = new Date(value);
        return date.getHours() + ":" + date.getMinutes();
    }
});

GetUserData();
function GetUserData() {
    var sessionToken = sessionStorage.getItem(tokenKey);
    var req = $.ajax({
        type: 'GET',
        url: '/api/v1/user',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + sessionToken);
        },
        success: function (data) {            
            ChatApp.topics = applyTopicsSelected(data.topics);
            ChatApp.unreadMessages = data.messages;
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
    var req = $.ajax({
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
            if ($('#file-1').prop('files').length > 0) {
                uploadFile(ChatApp.topicId, data.id);
            }
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

// загрузка файла
function uploadFile(topicid, messageid) {
    var file_data = $('#file-1').prop('files')[0];
    var form_data = new FormData();
    form_data.append('uploads', file_data);

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
        }
    });
    req.fail(function (data, status) {
        console.log(status);
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
            GetUserData();
        }
    });
}