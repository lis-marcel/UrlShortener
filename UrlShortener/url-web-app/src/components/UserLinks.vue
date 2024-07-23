<template>
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
    import { API_USER_LINKS } from '../config/consts';
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';

    export default {
        setup() {
            const links = ref([]);
            const showLinks = ref(false);
            const router = useRouter();

            const toggleLinksVisibility = () => {
                showLinks.value = !showLinks.value;
            };

            onMounted(() => {
                fetchUserLinks();
            });

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
                        router.push('/login');
                    }
                } catch (error) {
                    console.error(error);
                }
            };

            return {
                links,
                showLinks,
                toggleLinksVisibility
            };
        }
    }
</script>