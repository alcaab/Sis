<script setup lang="ts">
    import { useSidebarMenuStore } from "@/stores/sidebarMenu";
    import { useBreadcrumbs } from "@/layout/composables/useBreadcrumbs.ts";
    import { useI18n } from "vue-i18n";

    const store = useSidebarMenuStore();
    const { home, items } = useBreadcrumbs();
    const { t } = useI18n();

    const adminMenu = [
        {
            label: t("admin.panelGroups.userManagement.title"),
            icon: "pi pi-fw pi-shield",
            items: [
                { label: t("admin.panelGroups.userManagement.users"), to: "/admin/users" },
                { label: t("admin.panelGroups.userManagement.roles"), to: "/admin/roles" },
            ],
        },
    ];

    store.setMenu(adminMenu);
</script>

<template>
    <div class="card">
        <Breadcrumb
            :home="home"
            :model="items"
            class="mb-4"
        />
        <router-view></router-view>
    </div>
</template>
