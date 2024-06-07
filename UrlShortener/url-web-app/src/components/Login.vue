<template>
    <div class="container mt-5">
      <h2>Logowanie</h2>
      <form @submit.prevent="loginUser">
        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input type="email" class="form-control" id="email" v-model="loginRequest.Email" required>
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Hasło</label>
          <input type="password" class="form-control" id="password" v-model="loginRequest.Password" required>
        </div>
        <button type="submit" class="btn btn-primary">Zaloguj się</button>
        <p class="mt-3">Nie masz konta? <router-link to="/register">Zarejestruj się</router-link></p>
      </form>
    </div>
  </template>
  
<script>
import axios from 'axios';

export default {
  data() {
    return {
      loginRequest: {
        Email: '',
        Password: ''
      }
    };
  },
  methods: {
    async loginUser() {
      try {
        const response = await axios.post('https://localhost:7271/login', this.loginRequest, {
          headers: {
            'Content-Type': 'application/json',
          },
        });
        console.log(response.data);
      } catch (error) {
        console.error(error);
      }
    }
  }
}
</script>

<style scoped>
.container {
  width: 50%;
  margin: 0 auto; /* Add this line to center the container */
  padding: 2rem;
  border: 1px solid #ccc;
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}
</style>