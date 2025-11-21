import { createApp } from "vue";
import App from "@/App.vue";
import axios from "axios";
import VueAxios from "vue-axios";
import "netto-primevue/style.css";
import NettoPrimeVue from "netto-primevue";
import "netto-primevue/tailwind.css";
import router from "@/router";
import { createPinia } from "pinia";
import { createI18n } from "vue-i18n";
import * as de from "@/translations/de.json";
import oidcConfig from "@/oidcConfig";
import ConfirmationService from "primevue/confirmationservice";
import DialogService from 'primevue/dialogservice';
import { components } from "@/components";
import ToastService from 'primevue/toastservice';

const app = createApp(App);
const pinia = createPinia();
const i18n = createI18n({
  locale: "de",
  legacy: false,
  messages: {
    de,
  },
});

app
  .use(VueAxios, axios)
  .use(router)
  .use(ToastService)
  .use(NettoPrimeVue, {
    appChromeUrl: import.meta.env.VITE_APP_CHROME_URL,
    baseUrl: import.meta.env.VITE_APP_URL,
    authorityUrl: import.meta.env.VITE_AUTHORITY_URL,
    authorityApiUrl: import.meta.env.VITE_AUTHORITY_API_URL,
    i18n: i18n,
    pinia: pinia,
  })
  .use(ConfirmationService)
  .use(DialogService);

axios.defaults.baseURL = import.meta.env.VITE_APP_URL + "/api/";

app.provide("axios", app.config.globalProperties.axios);
app.provide("oidcClient", oidcConfig);
app.provide("router", router);

for(const comp of components) {
  app.component(comp.name, comp.type);
}

app.mount("#app");
