<template>
    <div class="nav-header">
        <div class="row btn_bar">
            <div class="col-md-2 mp-0 w36">
                <ul class="ul_btn_bar">
                    <li v-if="showBackButton" @click="backToTopics">
                        <img src="/img/rc/back_ico.png" class="back_span back_btn">
                    </li>
                </ul>
            </div>
            <div class="col-md-8 mp-0" style="padding-top: 9px;">
                <h2>
                    {{ titleText }}
                </h2>
                <span class="rigth_chat_count_bg" v-if="unreadMessages > 0 && !messageMode">
                    {{unreadMessages}}
                </span>
            </div>
            <div class="col-md-2 mp-0" style="width: 74px;margin-left: auto;">
                <ul class="ul_btn_bar">
                    <li @click="closePanel">
                        <span class="close_span minimize_btn"></span>
                    </li>
                    <li v-if="appstatemax" @click="collapse">
                        <span class="close_span restore_btn" v-if="appstatemax"></span>
                    </li>
                    <li v-if="!appstatemax" @click="maximize">
                        <span class="close_span maximize_btn" v-if="!appstatemax"></span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script>
    var openPanelKey = 'openPanelKey';
    import { mapMutations } from 'vuex';
    export default {
        name: 'NavHeader',
        props: {
            title: {
                type: String,
                default: 'ЧАТ ПОДДЕРЖКИ'
            }
        },
        data: function () {
            return {
                messageMode: false,
                titleText: this.title,
                showBackButton: false,
                unreadMessages: 0
            }
        },
        computed: {
            appstatemax() {
                //console.log(this.$store.state.appstatemax)
                return this.$store.state.appstatemax;
            }
        },
        methods: {
            ...mapMutations({
                minimizeChat: 'minimize',
                maximizeChat: 'maximize'
            }),
            switchTitle: function () {

                this.messageMode = !this.messageMode;
                if (this.messageMode) {
                    this.titleText = 'ВЫЙТИ ИЗ ДИАЛОГА'
                    this.showBackButton = true;
                } else {
                    this.titleText = 'ЧАТ ПОДДЕРЖКИ'
                    this.showBackButton = false;
                }
            },
            backToTopics: function () {
                window.console.log('backToTopics');
                // todo: backToTopics
            },
            closePanel: function () {                
                var bottom = getComputedStyle(document.getElementById('rigth-chat-app')).bottom;                
                var state = false;
                if (bottom == '0px') {                    
                    window.console.log('bottom: 0');
                } else {
                    window.console.log('bottom: ' + bottom);
                }
                window.console.log('closeRigthPanel: ' + state);                
                localStorage.setItem(openPanelKey, state);
                showRigthChat(state);
            },
            collapse: function () {
                window.console.log('minimize');
                var chat = document.getElementById('rigth-chat-app');
                chat.style.transition = '';                  
                setTimeout(function () {
                    chat.style.transition = 'opacity .4s ease-in, height 0.8s ease-in-out ';  
                    chat.style.height = '60%';
                }, 4);                
                
                setTimeout(function () {
                    chat.style.transition = '';
                }, 800);

                this.minimizeChat();
            },
            maximize: function () {
                window.console.log('maximize');
                // todo: maximize                
                var chat = document.getElementById('rigth-chat-app');
                chat.style.transition = '';
                
                setTimeout(function () {
                    chat.style.transition = 'opacity .4s ease-in, height 0.8s ease-in-out ';
                    chat.style.height = '100%';
                }, 4);                                
                setTimeout(function () {
                    chat.style.transition = '';
                }, 800);                
                this.maximizeChat();
            },
        }
    }
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>