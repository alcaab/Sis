import {createRouter, createWebHistory} from "vue-router";
import {useAuthStore} from "@/stores/authStore"
import claims from "@/authorization/app-permissions";
import { i18n } from "@/translations/i18n";
import EmailSettings from "@/components/EmailSettings.vue";
import EmailTemplates from "@/components/EmailTemplates.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: i18n.global.t("articlesPreview"),
      meta: {
        requiresAuth: true,
        claims: [claims.articlesPreviewRead],
      },
      component: () => import("@/views/HomeView.vue"),
    },
    {
      path: "/admin/application-settings",
      name: "Allgemein",
      meta: {
        requiresAuth: true,
        claims: [claims.settingsRead],
      },
      component: () => import("@/views/AppSettingsView.vue"),
    },
    {
      path: "/admin/custom-groups",
      name: i18n.global.t("customGroups"),
      meta: {
        requiresAuth: true,
        claims: [claims.customGroupsRead],
      },
      component: () => import("@/views/GroupView.vue"),
    },
    {
      path: "/gtin-article",
      name: i18n.global.t("gtinArticle.title"),
      meta: {
        requiresAuth: true,
        claims: [claims.gtinArticleRead],
      },
      component: () => import("@/components/articles/GtinArticleList.vue"),
    },
    {
      path: "/unauthorized",
      name: i18n.global.t("unauthorized"),
      component: () => import("@/views/UnauthorizedView.vue"),
    },
    {
      path: "/:pathMatch(.*)*",
      name: i18n.global.t("notFound"),
      meta: {
        requiresAuth: false,
        permissions: [],
      },
      component: () => import("@/views/NotFoundView.vue"),
    },
    {
      path: "/admin/email-settings",
      name: i18n.global.t("emailSettings"),
      meta: {
        title: i18n.global.t("emailSettings"),
        requiresAuth: true,
        claims: [claims.emailSettingsRead],
      },

      component: EmailSettings,
    },
    {
      path: "/admin/email-templates",
      name: i18n.global.t("emailTemplates"),
      meta: {
        title: i18n.global.t("emailTemplates"),
        requiresAuth: true,
        claims: [claims.emailTemplatesRead],
      },

      component: EmailTemplates,
    }
  ],
});

router.beforeEach(async (to, _, next) => {
  const authStore = useAuthStore();
  
  if (!authStore.user) {
    await authStore.login();
  }

  if (to.matched.some((record) => record.meta.requiresAuth) && authStore.user && Object.keys(authStore.user).length > 0) {
    if (!authStore.isAuthenticated || !(await authStore.hasClaims(to.meta.claims as string[]))) {
      return next("/unauthorized");
    }
  }

  return next();
});

export default router;
