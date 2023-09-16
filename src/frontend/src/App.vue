<template>
  <router-view></router-view>
  <Toast position="bottom-right" group="msg" />
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Toast from 'primevue/toast';
import { useUserDataStore } from "./store/usersStore";

export default defineComponent({
  name: "App",
  components: {
    Toast
  },
  created() {
    const store = useUserDataStore();
    store.autoLogin();
  }
});

</script>

<style lang="scss">
// @import "bootstrap/dist/css/bootstrap.min.css";
@import "primevue/resources/themes/bootstrap4-light-blue/theme.css";
@import "primevue/resources/primevue.min.css";
@import 'primeicons/primeicons.css';

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
</style>
