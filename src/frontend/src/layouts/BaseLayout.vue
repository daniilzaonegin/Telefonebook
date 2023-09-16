<template>
  <header>
    <nav>
      <ul class="site-bookmarks navigation-items">
        <li class="navigation-item">
          <a href="#" class="navbrand">
            <font-awesome-icon
              style="margin-right: 3px"
              icon="fa-regular fa-address-book"
              size="xl"
            />
            <h1 style="margin: 0 0">Telephone Book</h1></a
          >
        </li>
      </ul>
      <ul class="site-buttons navigation-items">
        <li class="navigation-item">
          <Button :label="'Logout'" v-if="currentTokenValue != null" icon="pi pi-sign-out" @click="logOut" />
        </li>
      </ul>
    </nav>
  </header>
  <div class="toolbar">
    <slot name="toolbar"></slot>
  </div>
  <div class="body">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
import router from "@/router";
import { useUserDataStore } from "@/store/usersStore";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import Button from "primevue/button";
import { onMounted, ref } from "vue";

const currentTokenValue = ref<string>();

onMounted(()=>{
  const store = useUserDataStore();
  const userInfo = store.getAuthInfo;
  currentTokenValue.value = userInfo?.token;
})

function logOut() {
  const store = useUserDataStore();
  store.logOut();
  router.push("/login");
}
</script>

<style lang="scss" scoped>
nav {
  display: flex;
  justify-content: space-between;
}

ul {
  list-style-type: none;
  margin: 0;
}

.site-bookmarks {
  ul.navigation-items {
    padding-left: 0;
  }

  li.navigation-item {
    display: inline;
    padding: 20px;
    a.navbrand {
      display: flex; 
      text-decoration: none;
      color: unset;
    }
  }

}

.site-buttons {
  display: flex;
  flex-direction: row-reverse;
  align-items: baseline;
}

.toolbar {
    margin: 5px 0 5px 0;
    button {
      margin-right: 3px;
    }
  }

</style>
