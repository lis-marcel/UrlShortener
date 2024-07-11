<template>
    <div class="container mt-5">
        <h2>Account info</h2>
        <p>Name: {{ user.name }}</p>
        <p>Email: {{ user.email }}</p>
    </div>
</template>

<script>
import { API_URL } from '../config/consts';

export default {
    data() {
        return {
            user: {
                email: '',
                name: '',
            }
        };
    },
    async mounted() {
        try {
            const response = await fetch(`${API_URL}/user`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('authToken')}`
                }
            });

            if (response.ok) {
                this.user = await response.json();

                console.log(this.user);
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