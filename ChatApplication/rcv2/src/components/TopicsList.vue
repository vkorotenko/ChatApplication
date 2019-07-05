<template>
    <div class="ms_container first_hider">
        <a v-for="topic in topics"
           :href="'#message-' + topic.id"
           v-bind:class="{active : topic.selected, active: topic.hasMessages ,selected: topic.selected}"
           class="msg_container mp-0 h100"
           @click="showThread(topic.id, this, $event)">

            <div class="row h68">
                <div class="col-md-2 mp-0 ms_first">
                    <img :src="'/api/v1/user/avatar/' + topic.authorId" class="w48 " />
                </div>
                <div class="col-md-8 mp-0 ms_body">
                    <span v-bind:class="{'message-name-active':  topic.hasMessages}"
                          class="message-name">
                        {{ topic.lmName }}
                    </span>
                    <span v-bind:class="{ readed : topic.lmIsReaded }"
                          class=" message-time-label">
                        {{topic.lmCreated | formatMonthDayEx }}
                    </span>
                    <br />

                    <span class="message-text" v-if="topic.lmIsCurrent">
                        <!--Вывод последнего сообщения или маркера печати-->
                        <span v-if="typingData && typingData.topic == topic.id" class="body_typing_i">
                            печатает
                        </span>
                        <template v-else>
                            Вы: {{topic.lastMessage |striphtml}}
                        </template>

                    </span>
                    <span class="message-text" v-else>
                        <!--Вывод последнего сообщения или маркера печати-->
                        <span v-if="typingData && typingData.topic == topic.id" class="body_typing_i">
                            печатает
                        </span>
                        <template v-else>
                            {{topic.lastMessage |striphtml}}
                        </template>
                    </span>
                </div>
                <div class="col-md-2 mp-0 end_col">
                    <img class="w48r3" :src="'/api/v1/article/' + topic.announcementId + '/img'">
                </div>
            </div>
            <div class="row low_row_border">
                <div class="col-md-12 mp-0 message-meta">
                    {{topic.title}} Цена: {{ topic.price }} <img src="/img/rc/run14it.png" style="height: 11px" /> <br />
                    Артикул: {{topic.vendorCode}} Производитель: {{topic.vendor}}
                </div>
            </div>
        </a>
        <a v-if="topics.length == 0">
            <div class="no_result">
                Нет результатов
            </div>
        </a>
    </div>
</template>

<script>
    import { mapState, mapActions } from 'vuex'
    export default {
        name: 'TopicsList',
        computed: mapState({
            topics: state => state.topics
        }),
        methods: mapActions('cart', [
            'addProductToCart'
        ]),
        created() {
            this.$store.dispatch('topics/getAllTopics')
        }

    }
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>