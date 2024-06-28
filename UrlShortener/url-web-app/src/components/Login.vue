<template>
    <div class="container mt-5">
      <h2>Log in</h2>
      <form @submit.prevent="loginUser">
        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input type="email" class="form-control" id="email" v-model="loginRequest.Email" required>
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Password</label>
          <input type="password" class="form-control" id="password" v-model="loginRequest.Password" required>
        </div>
        <button type="submit" class="btn btn-primary">Log in</button>
        <p class="mt-3">Don't have account? <router-link to="/register">Register here</router-link></p>
      </form>
    </div>
  </template>
  
<script>
import { API_URL } from '../config/consts';

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
        const response = await fetch(`${API_URL}/login`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(this.loginRequest)
        });

        const token = await response.json();

        if (response.ok) {
          localStorage.setItem('authToken', token);
          this.$router.push('/');
        } else {
          alert(data.message);
        }
      } catch (error) {
        console.error(error);
      }
    }
  }
}
</script>

<style scoped>
.container {
  width: 30%;
  margin: 0 auto;
  padding: 2rem;
  border: 1px solid #ccc;
  border-radius: 5px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}
</style>