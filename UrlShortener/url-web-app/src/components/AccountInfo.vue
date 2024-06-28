<template>
    <div class="container mt-5">
        <h2>Account info</h2>
        <p>Email: {{ user.Name }}</p>
        <p>Name: {{ user.Email }}</p>
    </div>
</template>

<script>
import { API_URL } from '../config/consts';

export default {
    data() {
        return {
            user: {
                Email: '',
                Name: '',
            }
        };
    },
    async mounted() {
        try {
            const response = await fetch(`${API_URL}/user`, {
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('authToken')}`
                }
            });

            if (response.ok) {
                this.user = await response.json();
            } else {
                this.$router.push('/login');
            }
        } catch (error) {
            console.error(error);
        }
    }
}
</script>

<style scoped>
</style>