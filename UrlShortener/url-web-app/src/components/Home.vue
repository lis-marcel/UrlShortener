<template>
  <div class="link-shortener">
    <form @submit.prevent="submitLink">
      <input v-model="Url" type="text" placeholder="Wprowadź link do skrócenia" />
      <button type="submit">Skróć link</button>
    </form>
    <div v-if="shortLink">
      <p>Twój skrócony link: {{ shortLink }}</p>
      <button @click="copyToClipboard">Kopiuj do schowka</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      Url: '',
      shortLink: ''
    };
  },
  methods: {
    async submitLink() {
      try {
        await axios.post('https://localhost:7271/add', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          Url: this.Url,
        })
        .then(response => {
          this.shortLink = response.data;
        })
        
      } catch (error) {
        console.error(error);
      }
    },
    copyToClipboard() {
      navigator.clipboard.writeText(this.shortLink);
    }
  }
};
</script>

<style scoped>
.link-shortener {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100vh;
  gap: 20px;
}

input {
  padding: 10px;
  font-size: 16px;
}

button {
  padding: 10px 20px;
  font-size: 16px;
  cursor: pointer;
}
</style>
