<template>
    <router-view />
</template>

<script>
    export default {
        name: 'Project workflow',
        components: {}
    }
</script>

<style>
    @import '~@mdi/font/css/materialdesignicons.css';

    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #00ffff;
        margin-top: 60px;
    }

    body {
        background: #373737;
    }

    v-table {
        background: #373737;
    }
</style>
