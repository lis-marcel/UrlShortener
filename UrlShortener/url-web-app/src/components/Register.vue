<template>
    <div class="container mt-5">
      <h2>Rejestracja nowego użytkownika</h2>
      <form @submit.prevent="registerUser">
        <div class="mb-3">
          <label for="username" class="form-label">Nazwa użytkownika</label>
          <input type="text" class="form-control" id="username" v-model="newUser.username" required>
        </div>
        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input type="email" class="form-control" id="email" v-model="newUser.email" required>
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Hasło</label>
          <input type="password" class="form-control" id="password" v-model="newUser.password" required>
        </div>
        <button type="submit" class="btn btn-primary">Zarejestruj</button>
        <p class="mt-3">Masz już konto? <router-link to="/login">Zaloguj się</router-link></p>
      </form>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        newUser: {
          username: '',
          email: '',
          password: ''
        }
      };
    },
    methods: {
      async registerUser() {
        // Tutaj dodaj logikę do rejestracji użytkownika, np. wysyłając dane do API
        console.log('Rejestrowanie użytkownika:', this.newUser);
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