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
            <button @click="copyToClipboard" class="btn btn-primary">Copy to clipboard</button>
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

            const submitLink = async () => {
                try {
                    const response = await axios.post(API_APP_SHORTEN, {
                        Url: Url.value,
                    }, {
                        headers: {
                            'Content-Type': 'application/json',
                        },
                    });
                    shortLink.value = response.data;
                } catch (error) {
                    console.error(error);
                }
            };

            const copyToClipboard = () => {
                navigator.clipboard.writeText(shortLink.value);
            };

            return {
                Url,
                shortLink,
                submitLink,
                copyToClipboard,
            };
        },
    };
</script>

<style scoped>
    .container {
        margin: 5vh;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        height: 35vh;
        width: 50%;
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

    .output-content {
        margin-bottom: 10px;
    }
</style>