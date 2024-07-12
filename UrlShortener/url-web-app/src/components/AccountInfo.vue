<template>
    <div class="container mt-5">
        <h2>Account info</h2>
        <p>Name: {{ user.name }}</p>
        <p>Email: {{ user.email }}</p>
    </div>
</template>

<script>
    import { API_URL } from '../config/consts';
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';

    export default {
        setup() {
            const user = ref({
                email: '',
                name: '',
            });
            const router = useRouter();

            const fetchUserData = async () => {
                try {
                    const response = await fetch(`${API_URL}/user`, {
                        method: 'POST',
                        headers: {
                            'Authorization': `Bearer ${localStorage.getItem('authToken')}`
                        }
                    });

                    if (response.ok) {
                        user.value = await response.json();
                        console.log(user.value);
                    } else {
                        router.push('/login');
                    }
                } catch (error) {
                    console.error(error);
                }
            };

            onMounted(() => {
                fetchUserData();
            });

            return {
                user
            };
        }
    }
</script>

<style scoped>
</style>
