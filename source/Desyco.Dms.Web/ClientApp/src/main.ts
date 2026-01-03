import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import router from "./router";
import i18n from "./i18n";
import * as yup from "yup";
import Aura from "@primeuix/themes/aura";
import PrimeVue from "primevue/config";
import ConfirmationService from "primevue/confirmationservice";
import ToastService from "primevue/toastservice";

import "@/assets/tailwind.css";
import "@/assets/styles.scss";

yup.setLocale({
    mixed: {
        required: () => "required",
    },
});

const app = createApp(App);

app.use(router);
app.use(createPinia());
app.use(i18n);
app.use(PrimeVue, {
    theme: {
        preset: Aura,
        options: {
            darkModeSelector: ".app-dark",
            cssLayer: {
                name: "primevue",
                order: "theme, base, primevue",
            },
        },
    },
});
app.use(ToastService);
app.use(ConfirmationService);

app.mount("#app");
