<template>
    <div class="container mt-5">
        <h2>Account info</h2>
        <p>Name: {{ user.name }}</p>
        <p>Email: {{ user.email }}</p>
    </div>
    <div class="links">
        <h3 @click="toggleLinksVisibility" style="cursor: pointer;">
            Your Links
        </h3>
        
        <!-- Conditionally rendered table of links -->
        <div v-if="showLinks">
            <table>
                <thead>
                    <tr>
                        <th>Original URL</th>
                        <th>Shortened URL</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="link in links" :key="link.id">
                        <td><a :href="link.url" target="_blank"> {{ link.url }} </a></td>
                        <td><a :href="link.shortUrl" target="_blank"> {{ link.shortUrl }} </a></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
    import { API_USER_ACCOUNT, API_USER_LINKS } from '../config/consts';
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';

    export default {
        setup() {
            const user = ref({
                email: '',
                name: '',
            });
            const links = ref([]);
            const showLinks = ref(false);
            const router = useRouter();

            const fetchUserData = async () => {
                try {
                    const response = await fetch(API_USER_ACCOUNT, {
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

            const fetchUserLinks = async () => {
                try {
                    const response = await fetch(API_USER_LINKS, { // Use the correct endpoint
                        method: 'GET', // Assuming it's a GET request
                        headers: {
                            'Authorization': `Bearer ${localStorage.getItem('authToken')}`
                        }
                    });

                    if (response.ok) {
                        links.value = await response.json();
                    } else {
                        console.error('Failed to fetch links');
                    }
                } catch (error) {
                    console.error(error);
                }
            };

            const toggleLinksVisibility = () => {
                showLinks.value = !showLinks.value;
            };

            onMounted(() => {
                fetchUserData();
                fetchUserLinks();
            });

            return {
                user,
                links,
                showLinks,
                toggleLinksVisibility
            };
        }
    }
</script>

<style scoped>
.container {
    max-width: 100%; /* Ensures the container does not exceed the viewport width */
    overflow-x: hidden; /* Prevents horizontal scrolling at the container level */
    padding: 0 20px; /* Adds some padding for small screens */
}

table {
    width: 100%;
    border-collapse: collapse;
    table-layout: fixed; /* Helps with making the table layout more stable */
}

.links {
    margin-left: 2%;
}

th, td {
    border: 1px solid #ddd;
    padding: 8px;
    word-wrap: break-word; /* Ensures long words can wrap and not cause horizontal overflow */
}

th {
    background-color: #f2f2f2;
}

/* Make table scrollable on small screens */
@media screen and (max-width: 768px) {
    .container {
        padding: 0 10px; /* Adjust padding for very small screens */
    }

    table {
        display: block;
        overflow-x: auto; /* Allows table to scroll horizontally */
        white-space: nowrap; /* Prevents cells from wrapping content to new lines */
    }
}
</style>

