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
    import { ref } from 'vue';
    import axios from 'axios';
    import { useRouter } from 'vue-router';
    import { API_SESSION_LOGIN } from '../config/consts';

    export default {
        setup() {
            const loginRequest = ref({
                Email: '',
                Password: ''
            });
            const router = useRouter();

            const loginUser = async () => {
                try {
                    const response = await axios.post(API_SESSION_LOGIN, loginRequest.value);
                    if (response.status >= 200 && response.status < 300) {
                        localStorage.setItem('authToken', response.data);
                        window.dispatchEvent(new CustomEvent('auth-change'));
                        router.push('/');
                    } else {
                        console.error('Login failed:', response.data.message);
                    }
                } catch (error) {
                    console.error('Login error:', error);
                }
            };

            return {
                loginRequest,
                loginUser
            };
        },
    };
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
