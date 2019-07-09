import Vue from 'vue'
import App from './App.vue'
import FloatButton from './FloatButton.vue'
import router from './router'
import store from './store'

Vue.config.productionTip = false;
var openPanelKey = 'openPanelKey';

var rc = new Vue({
    router,
    methods: {        
        setLogin: function (data) {            
            this.$store.commit('setLoginUser', data);
        },
        refreshToken: function(token) {

        }
    },
    store,
    render: h => h(App)
});
rc.$mount('#rigth-chat-app');
window.RigthChat = rc;


window.FloatMessageButton = new Vue({    
    store,
    render: h => h(FloatButton)
});

window.FloatMessageButton.$mount('#floatMessageButton');

window.showRigthChat = function (ifShow, id) {
    var st = localStorage.getItem(openPanelKey);
    if (st != null) {
        ifShow = JSON.parse(st);
    }
    if (id) {
        ifShow = true;
        // todo: show chat thread by id
        window.RigthChatApp.showThread(id);
    }
    document.getElementById('right-chat-toggle').checked = ifShow;    
};

document.getElementById('right-chat-toggle').onchange  = function(e) {    
    var state = document.getElementById('right-chat-toggle').checked;
    localStorage.setItem(openPanelKey, state);
};