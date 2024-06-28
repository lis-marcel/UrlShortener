import { createRouter, createWebHistory } from 'vue-router'
import MainView from '../views/MainView.vue'
import RegisterView from '../views/RegisterView.vue'
import LoginView from '../views/LoginView.vue'
import AccountView from '../views/AccountView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: MainView
  },
  {
    path: '/register',
    name: 'register',
    component: RegisterView
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/account',
    name: 'account',
    component: AccountView,
    meta: { requiresAuth: true }
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  // Check if the route requires authentication
  if (to.matched.some(record => record.meta.requiresAuth)) {
    // This is where you would verify the user's authentication status
    // For example, checking if a token exists in localStorage
    if (!localStorage.getItem('authToken')) {
      // Redirect the user to the login page if they are not authenticated
      next('/login');
    } else {
      // Proceed to the route if the user is authenticated
      next();
    }
  } else {
    // If the route does not require authentication, always proceed
    next();
  }
});

function isAuthenticated() {
  const token = localStorage.getItem('authToken');
  console.log('Token from localStorage:', token);
  return !!token;
}

export default router