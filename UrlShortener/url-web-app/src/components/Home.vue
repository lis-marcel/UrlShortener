<template>
  <div class="container">
    <div class="input">
      <form @submit.prevent="submitLink">
        <h2>Paste the URL to be shortened</h2>
        <div class="form-group mx-sm-3 mb-2">
          <input v-model="Url" type="text" class="form-control" placeholder="Enter link here">
          <button type="submit" class="btn btn-primary mb-2">Shorten URL</button>
        </div>
      </form>
    </div>
  </div>

  <div v-if="shortLink" class="output">
    <div class="output-content">
      <h2>Your shortened link is:</h2>
      <p>{{ shortLink }}</p>
    </div>
    <button @click="copyToClipboard" class="btn btn-primary">Copy to clipboard</button>
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
  .container {
    margin: 5vh;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 30vh;
    width: 50%;
    border: 1px solid black;
    border-radius: 20px;
  }

  .form-group {
    display: flex;
    align-items: center;
  }

  .form-control {
    margin-right: 10px;
  }

  .btn-primary {
    margin-left: 10px;
  }

  .output {
    text-align: center;
    margin-top: 20px;
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  .output-content {
    margin-bottom: 10px;
  }
</style>