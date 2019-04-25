var tokenKey = "accessToken";
var sessionToken = sessionStorage.getItem(tokenKey);

var app = new Vue({
    el: '#chatApp',
    data: {
        unreadMessages: 0,
        topics: [],
        posts: [],
        topicAuthor: 0
    },
    methods: {
        showThread: function (id, el, event) {
            selectItem(id);
            getMessagesForTopic(id, app.topicAuthor);
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