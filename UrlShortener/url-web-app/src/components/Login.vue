<template>
    <div class="container mt-5">
      <h2>Logowanie</h2>
      <form @submit.prevent="loginUser">
        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input type="email" class="form-control" id="email" v-model="userCredentials.email" required>
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Hasło</label>
          <input type="password" class="form-control" id="password" v-model="userCredentials.password" required>
        </div>
        <button type="submit" class="btn btn-primary">Zaloguj się</button>
        <p class="mt-3">Nie masz konta? <router-link to="/register">Zarejestruj się</router-link></p>
      </form>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        userCredentials: {
          email: '',
          password: ''
        }
      };
    },
    methods: {
      async loginUser() {
        // Tutaj dodaj logikę do logowania użytkownika, np. wysyłając dane do API
        console.log('Logowanie użytkownika:', this.userCredentials);
      }
    }
  };
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