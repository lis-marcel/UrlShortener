<template>
  <h1>Welcome to FoxNet URL shortener</h1>

  <form @submit.prevent="submitForm">
    <label for="url">Paste your link below</label><br><br>
    <input type="text" v-model="Url" placeholder="URL" id="url">
    <button type="submit">Send</button>
  </form>

  <br>

  <div class="code-container">
    <pre><code>{{ ShortenedUrl }}</code></pre>
  </div>
  <button @click="copyCode">Kopiuj kod</button>
</template>
  
<script>
  import axios from 'axios';

  export default {
    data() {
      return {
        Url: '',
        ShortenedUrl: 'Here will appear your shortened URL',
      };
    },
    methods: {
      submitForm() {
        axios.post('https://localhost:7271/add', {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
          },
          body: this.Url,
        })
        .then(response => {
          this.ShortenedUrl = response.data
        })
        .catch(error => {
          console.error(error)
        });
      },
      copyCode() {
        navigator.clipboard.writeText(this.ShortenedUrl)
        .then(() => {
          alert('Kod skopiowany!');
        })
        .catch(err => {
          console.error('Błąd podczas kopiowania kodu:', err);
        });
      }
    }
  };
</script>

<style>
.code-container {
  margin-left: 39%;
  width: 20%;
  position: relative;
  background-color: #f9f9f9;
  border: 1px solid #ddd;
  padding: 10px;
}
pre {
  white-space: pre-wrap;
  word-wrap: break-word;
}
</style>