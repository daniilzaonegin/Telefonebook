import HomePage from "@/pages/HomePage.vue";
import AddContactPage from "@/pages/AddContactPage.vue";
import { createRouter, createWebHistory } from "vue-router";
import { useContactsStore } from "@/store/contactsStore";
import LoginPage from "@/pages/LoginPage.vue";
import RegisterPage from "@/pages/RegisterPage.vue";

import { useUserDataStore } from "@/store/usersStore";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      component: HomePage,
    },
    {
      path: "/add",
      component: AddContactPage,
    },
    {
      path: "/edit/:id(\\d+)",
      component: AddContactPage,
    },    
    {
      path: "/login",
      component: LoginPage,
    },
    {
      path: "/register",
      component: RegisterPage,
    },
  ],
});


router.beforeEach(async (to, from) => {
  const store = useUserDataStore();
  console.log("token", store.authInfo?.token)
  if (
    // make sure the user is authenticated
    !store.authInfo?.token &&
    // ❗️ Avoid an infinite redirect
    to.path &&
    !(to.path.toLowerCase().startsWith('/login') ||
      to.path.toLowerCase().startsWith('/register'))
  ) {
    // redirect the user to the login page
    return { path: '/login' }
  }
});

export default router;
