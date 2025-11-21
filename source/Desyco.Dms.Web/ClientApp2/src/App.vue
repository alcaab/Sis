<script setup lang="ts">
import { AppContainer } from "netto-primevue";
import { useAuthStore } from "@/stores/authStore";
import { onMounted, ref} from "vue";
import { useI18n } from "vue-i18n";
import userManager from "@/oidcConfig";
import claims from "@/authorization/app-permissions"
import type { MenuItem } from "primevue/menuitem";

const { t } = useI18n();
const authStore = useAuthStore();
const items = ref<MenuItem[]>([]);

onMounted(async () => {
  await setMenuItems();
});

userManager.events.addUserLoaded(async () => {
  await setMenuItems();
});

async function setMenuItems(): Promise<void> {
  await authStore.updateUser();
  items.value = [
    {
      label: t("articlesPreview"),
      route: "/",
      visible: await authStore.hasClaims([claims.articlesPreviewRead]),
    },
    {
      separator: true,
      class: "mx-3",
    },
    {
      label: t("administration"),
      visible: await authStore.hasClaims(claims.administrationSection),
      items:[
        {
          label: t("appSettings.title"),
          route: "/admin/application-settings",
          visible: await authStore.hasClaims([claims.settingsRead]),
        },
        {
        label: t("customGroups"),
        route: "/admin/custom-groups",
        visible: await authStore.hasClaims([claims.customGroupsRead]),
        },
        {
          label: t("gtinArticle.title"),
          route: "/gtin-article",
          visible: await authStore.hasClaims([claims.gtinArticleRead]),
        },
        {
          label: t("emailSettings"),
          route: "/admin/email-settings",
          visible: await authStore.hasClaims([claims.emailSettingsRead]),
        },
        {
          label: t("emailTemplates"),
          route: "/admin/email-templates",
          visible: await authStore.hasClaims([claims.emailTemplatesRead]),
        }
      ]
    },
    {
      label: t("jobOverview"),
      url: `${import.meta.env.VITE_HANGFIRE_URL}`,
      visible: await authStore.hasClaims([claims.admin, claims.job]),
    },
  ];
}
</script>

<template>
  <Toast />
  <DynamicDialog />
  <AppContainer
    :app-name="t('applicationName')"
    :app-menu-items="items">
    <router-view></router-view>
  </AppContainer>
</template>

<style scoped></style>
