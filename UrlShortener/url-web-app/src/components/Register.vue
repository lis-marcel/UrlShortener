<template>
    <div class="container mt-5">
        <h2>User registration</h2>
        <form @submit.prevent="registerUser">
            <div class="mb-3">
                <label for="username" class="form-label">Name</label>
                <input type="text" class="form-control" id="username" v-model="registerRequest.Name" required>
            </div>
            <div class="mb-3">
                <label for="email" class="form-label">Email</label>
                <input type="email" class="form-control" id="email" v-model="registerRequest.Email" required>
            </div>
            <div class="mb-3">
                <label for="password" class="form-label">Password</label>
                <input type="password" class="form-control" id="password" v-model="registerRequest.Password" required>
            </div>
            <button type="submit" class="btn btn-primary">Register</button>
            <p class="mt-3">Already have an account? <router-link to="/login">Log in</router-link></p>
        </form>
    </div>
</template>

<script>
    import { ref } from 'vue';
    import axios from 'axios';
    import { API_USER_REGISTER } from '../config/consts';

    export default {
        setup() {
            const registerRequest = ref({
                Name: '',
                Email: '',
                Password: ''
            });

            const registerUser = async () => {
                try {
                    await axios.post(API_USER_REGISTER, registerRequest.value);
                } catch (error) {
                    console.error(error);
                }
            };

            return {
                registerRequest,
                registerUser
            };
        }
    };
</script>

<style scoped>
    .container {
        width: 30%;
        padding: 2rem;
        border: 1px solid #ccc;
        border-radius: 5px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }
</style>
