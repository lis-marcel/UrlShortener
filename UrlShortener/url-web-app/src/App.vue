<script setup>
import Navbar from './components/Navbar.vue'
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <Navbar />
  <RouterView />
</template>

<style>
  template {
    background-color:  #f0f0f0;
  };
</style>