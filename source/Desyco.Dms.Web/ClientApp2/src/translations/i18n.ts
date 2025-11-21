import { createI18n } from "vue-i18n";
import * as de from "@/translations/de.json";

export const i18n = createI18n({
  locale: "de",
  legacy: false,
  messages: {
    de,
  },
  globalInjection: true,
});
