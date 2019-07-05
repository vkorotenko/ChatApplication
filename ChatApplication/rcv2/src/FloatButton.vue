<template>
    <label for="right-chat-toggle"
           v-bind:class="{float_button_no_bg: count == 0, float_button_yes_bg: count > 0, 'nav-toggle': true }" onclick>
        <span v-if="count > 0">{{count}}</span>
        <span id="totalUnreadMessagesRc" style="display: none;"></span>
    </label>
</template>
<style>
</style>
<script>
    export default {
        name: 'FloatButton',
        props: {
            unread: {
                type: Number,
                default: 0
            }
        },
        data: function () {
            return {
                count: this.unread
            }
        },
        methods: {
            setCount: function (unread) {
                window.console.log(unread);
                this.count = unread;
            }
        }
    }
</script>
