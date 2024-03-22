<template>
  <ShortenUrl />
</template>

<script>
import ShortenUrl from './components/ShortenUrl.vue'

export default {
  name: 'App',
  components: {
    ShortenUrl
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
