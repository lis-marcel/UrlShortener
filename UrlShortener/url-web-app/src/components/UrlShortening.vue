<template>
    <div class="container">
        <div class="input">
            <form @submit.prevent="submitLink">
                <h2>Paste the URL to be shortened</h2>
                <div class="form-group mx-sm-3 mb-2">
                    <input v-model="Url" type="text" class="form-control" placeholder="Enter link here">
                    <button type="submit" class="btn btn-primary mb-2">Shorten URL</button>
                </div>
            </form>
        </div>
        <br>
        <div v-if="shortLink" class="output">
            <div class="output-content">
                <h2>Your shortened link is:</h2>
                <p>{{ shortLink }}</p>
            </div>
            <button class="btn btn-primary mb-2" :class="{ 'btn-copied': isCopied }" @click="copyToClipboard">
                {{ isCopied ? 'Copied!' : 'Copy' }}
            </button>
        </div>
    </div>
</template>

<script>
    import { ref } from 'vue';
    import axios from 'axios';
    import { API_APP_SHORTEN } from '../config/consts';

    export default {
        setup() {
            const Url = ref('');
            const shortLink = ref('');
            const isCopied = ref(false);

            const submitLink = async () => {
                try {
                    const response = await axios.post(API_APP_SHORTEN, {
                        Url: Url.value,
                    }, {
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': `Bearer ${localStorage.getItem('authToken')}`
                        },
                    });
                    shortLink.value = response.data;
                } catch (error) {
                    console.error(error);
                }
            };

            const copyToClipboard = () => {
                navigator.clipboard.writeText(shortLink.value);

                isCopied.value = true;

                setTimeout(() => {
                    isCopied.value = false;
                }, 3000);
            };

            return {
                Url,
                shortLink,
                submitLink,
                copyToClipboard,
                isCopied,
            };
        },
    };
</script>

<style scoped>
    .container {
        margin: 5vh auto; /* Center the container horizontally */
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        min-height: 35vh; /* Set a minimum height */
        width: 80%; /* Make width responsive */
        max-width: 600px; /* Set a maximum width */
        min-width: 300px; /* Set a minimum width to ensure content fits */
        border: 1px solid #ccc;
        border-radius: 5px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }

    .form-group {
        display: flex;
        align-items: center;
    }

    .form-control {
        margin-right: 10px;
    }

    .btn-primary {
        margin-left: 10px;
    }

    .output {
        text-align: center;
        margin-top: 20px;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .btn-copied, .btn-copied:hover {
        background-color: green;
        color: white;
    }

    .output-content {
        margin-bottom: 10px;
    }

    @media (max-width: 768px) {
        .container {
            width: 90%; /* Increase width for smaller screens */
        }
    }

    @media (max-width: 480px) {
        .container {
            flex-direction: column;
            width: 95%; /* Further increase width for very small screens */
        }

        .form-group {
            flex-direction: column; /* Stack input and button vertically on small screens */
            align-items: stretch;
        }

        .form-control, .btn-primary {
            width: 100%; /* Make input and button take full width */
            margin: 10px 0; /* Add some margin for spacing */
        }
    }
</style>