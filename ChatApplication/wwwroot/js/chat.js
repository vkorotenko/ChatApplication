var tokenKey = "accessToken";
var sessionToken = sessionStorage.getItem(tokenKey);

var app = new Vue({
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
            app.topicId = id;
            selectItem(id);
            getMessagesForTopic(id, app.topicAuthor);
        },
        sendMessage: function () {
            if (app.messageArea != "")
                sendMessageToTopic(app.messageArea, app.topicId);
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
            var countBox = $('#totalUnreadMessages');
            var count = data.messages;
            countBox.text(count);
            if (count > 0)
                countBox.show();

            app.topics = applyTopicsSelected(data.topics);
            app.unreadMessages = data.messages;
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
    for (i = 0; i < app.topics.length; i++) {
        app.topics[i].selected = false;
        if (app.topics[i].id == id) {
            app.topicAuthor = app.topics[i].authorId;
            app.topics[i].selected = true;
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

            app.posts = data;
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
            app.messageArea = "";
            data.isAuthor = true;
            if ($('#file-1').prop('files').length > 0) {
                uploadFile(app.topicId, data.id);
            }
            app.posts.push(data);
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