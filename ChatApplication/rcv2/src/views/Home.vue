<template>
    <!-- Base container-->
    <div v-if="inthread" class="fh_100">
        <AnonseBar />
        <MessageList />
        <SendPanel />
    </div>
    <div v-else class="fh_100">
        <SearchBox />
        <TopicsList />        
    </div>
</template>
<style scoped>
    .fh_100{
        height: calc(100% - 50px);
    }
</style>
<script>
    import SearchBox from '@/components/SearchBox.vue'
    import TopicsList from '@/components/TopicsList.vue'
    import AnonseBar from '@/components/AnonseBar.vue'
    import MessageList from '@/components/MessageList.vue'
    import SendPanel from '@/components/SendPanel.vue'

    export default {
        name: 'home',
        data: function () {
            return {
                inthread: false,
                threadid: null
            }
        },
        components: {
            SearchBox,
            TopicsList,
            AnonseBar,
            MessageList,
            SendPanel
        },
        methods: {
        }
    }
</script>
