<template>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container-fluid">
        <a class="navbar-brand" href="#">FoxNet</a>
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
        </div>
        </div>
    </nav>
</template>