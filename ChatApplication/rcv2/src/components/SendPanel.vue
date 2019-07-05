<template>
    <!-- send message bar-->
    <div class="send_panel" id="send_panel">
        <div class="loader" v-if="showLoader"></div>
        <div class="row" style="height: 100%">
            <div class="messager__file file file_small col-md-2 mp-0 flx_bt">
                <form>
                    <label for="file-rch">
                        <img src="/img/rc/attachment.png" title="selectFileName" class="send_panel__attachment" />
                    </label>
                    <input type="file" id="file-rch" name="uploads" @change="fileSelected">
                </form>
            </div>
            <div class="col-md-8 mp-0">
                <textarea class="send_ta" id="ta_send_panel"
                          placeholder="Напишите сообщение..."
                          v-model="messageArea"
                          @keyup="resizeSendArea"
                          @keyup.enter.exact="sendMessage"
                          @keyup.enter.ctrl.exact="appendNewLine"></textarea>
            </div>
            <div class="col-md-2 mp-0 flx_bt">
                <img src="/img/rc/send.png" class="send_panel__btn" @click="sendMessage" title="Отправить" />
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'SendPanel',
        data: function () {
            return {
                messageArea: '',
                showLoader: false
            }            
        },
        props: {
            topicid: Number
        },
        methods: {
            resizeSendArea: function () {

            }
        }
    }
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .send_panel__btn {
        width: 23px;
        margin-bottom: 17px;
        margin-left: 7px;
    }
    .send_panel__attachment {
        margin-left: 20px;
        margin-bottom: 11px;
    }
</style>