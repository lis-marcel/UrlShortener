<template>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container-fluid">
            <a class="navbar-brand">FoxNet</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <router-link to="/"><a class="nav-link">Home</a></router-link>
                    </li>
                    <li class="nav-item">
                        <router-link to="/account"><a class="nav-link">Account</a></router-link>
                    </li>
                </ul>
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item">
                        <router-link v-if="!tokenExists" to="/login"><a class="nav-link">Log in</a></router-link>
                        <a v-else class="nav-link" @click="logout">Log out</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>

<script>
import { ref, onBeforeUnmount } from 'vue';
import { useRouter } from 'vue-router';

export default {
    setup() {
        const tokenExists = ref(localStorage.getItem('authToken') !== null);
        const router = useRouter();

        const updateTokenStatus = () => {
            tokenExists.value = localStorage.getItem('authToken') !== null;
        };

        const logout = () => {
            localStorage.removeItem('authToken');
            updateTokenStatus();
            router.push('/');
        };

        const onAuthChange = () => {
            updateTokenStatus();
        };

        window.addEventListener('auth-change', onAuthChange);

        onBeforeUnmount(() => {
            window.removeEventListener('auth-change', onAuthChange);
        });

        return {
            tokenExists,
            logout,
        };
    },
}
</script>

<style scoped>
    .navbar-nav.ml-auto {
        margin-left: auto;
        display: flex;
        align-items: center;
        justify-content: flex-end;
    }

    .navbar-nav a {
        text-decoration: none;
    }

    .navbar-nav li.nav-item:hover a {
        background-color: #5e5e5e; /* Dark grey background */
        color: white; /* Change text color */
    }
</style>
