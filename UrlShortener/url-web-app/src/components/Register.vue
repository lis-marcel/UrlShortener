<template>
    <div class="container mt-5">
      <h2>Rejestracja nowego użytkownika</h2>
      <form @submit.prevent="registerUser">
        <div class="mb-3">
          <label for="username" class="form-label">Nazwa użytkownika</label>
          <input type="text" class="form-control" id="username" v-model="registerRequest.Name" required>
        </div>
        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input type="email" class="form-control" id="email" v-model="registerRequest.Email" required>
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Hasło</label>
          <input type="password" class="form-control" id="password" v-model="registerRequest.Password" required>
        </div>
        <button type="submit" class="btn btn-primary">Zarejestruj</button>
        <p class="mt-3">Masz już konto? <router-link to="/login">Zaloguj się</router-link></p>
      </form>
    </div>
  </template>
  
<script>
import axios from 'axios';

export default {
  data() {
    return {
      registerRequest: {
        Name: '',
        Email: '',
        Password: ''
      }
    };
  },
  methods: {
    async registerUser() {
      try {
        await axios.post('https://localhost:7271/register', this.registerRequest);
      } catch (error) {
        console.error(error);
      }
    }
  }
};
</script>
  
<style scoped>
.container {
  width: 50%;
  padding: 2rem;
  border: 1px solid #ccc;
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}
</style>