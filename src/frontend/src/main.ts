import "primeflex/primeflex.css";
import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import PrimeVue from "primevue/config";
import Tooltip from 'primevue/tooltip';
import router from "./router";
import ToastService from 'primevue/toastservice';

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core';

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

/* import specific icons */
// import { faUserSecret } from '@fortawesome/free-solid-svg-icons';
import { faAddressBook } from '@fortawesome/free-regular-svg-icons';

// import "bootstrap/dist/js/bootstrap";
library.add(faAddressBook);
const pinia = createPinia();
const app = createApp(App);

app.directive('tooltip', Tooltip);
app.use(pinia);
app.use(PrimeVue);
app.use(ToastService);
app.use(router);
app.component('font-awesome-icon', FontAwesomeIcon);
app.mount("#app");
