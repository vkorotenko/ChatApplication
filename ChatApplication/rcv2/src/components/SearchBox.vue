<template>
    <input type="text" placeholder="Поиск..." class="rigth_chat_search_input" v-model="query">    
</template>

<script>
    export default {
        name: 'SearchBox',
        props: {
            query: String
        }
    }
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>   
</style>