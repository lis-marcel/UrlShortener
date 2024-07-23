<template>
    <div class="container mt-5">
        <div class="header-container">
            <h2>Account info</h2>
            <button @click="editUserData" class="edit-user-info-btn">Edit</button>
        </div>
        <p>Name: {{ user.name }}</p>
        <p>Email: {{ user.email }}</p>
    </div>
    
</template>

<script>
    import { API_USER_ACCOUNT } from '../config/consts';
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

            onMounted(() => {
                fetchUserData();
            });

            return {
                user,
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

    .header-container {
        display: flex;
        align-items: center; /* Align items vertically */
    }

    .edit-user-info-btn {
        padding: 5px 10px;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
    }

    .edit-user-info-btn:hover {
        background-color: #0056b3;
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

