<template>
    <nav id="rigth-chat-app" class="nav">        
        <NavHeader/>        
        <router-view />
    </nav>
</template>
<style>

</style>
<script>
    import NavHeader from '@/components/NavHeader.vue'
    export default {
        name: 'App',
        components: {
            NavHeader
        }
    }
</script>
