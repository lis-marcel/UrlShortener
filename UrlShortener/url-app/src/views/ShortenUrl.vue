<template>
  <div class="d-flex align-items-center justify-content-center vh-100">
    <div class="container text-center">
      <form @submit.prevent="submitForm">
        <div class="mb-3">
          <label for="url" class="form-label">URL do skrócenia</label>
          <input type="text" class="form-control" id="url" v-model="this.Url" required>
        </div>
        <button type="submit" class="btn btn-primary">Skróć URL</button>
      </form>
      <div class="mt-3">
        <h5>Twój skrócony URL:</h5>
        <p v-if="this.ShortUrl">{{ this.ShortUrl }}</p>
        <p v-else>Brak skróconego URL</p>
        <button :class="buttonClass" :disabled="!this.ShortUrl" @click="copyCode">Kopiuj URL</button>
      </div>
    </div>
  </div>
</template>
  
<script>
  import axios from 'axios';

  export default {
    data() {
      return {
        Url: '',
        ShortUrl: '',
        CopyButtonText: 'Copy',
        copied: false
      };
    },
    computed: {
      buttonClass() {
        return this.copied ? 'btn-secondary' : 'btn-primary';
      }
    },
    methods: {
      submitForm() {
        axios.post('https://localhost:7271/add', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          Url: this.Url,
        })
        .then(response => {
          this.ShortUrl = response.data
        })
        .catch(error => {
          console.error(error)
        });
      },
      copyCode() {
        navigator.clipboard.writeText(this.ShortUrl)
        .then(() => {
          this.CopyButtonText = 'Copied!'
          this.copied = true
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