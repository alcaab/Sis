<script setup lang="ts">
    import { useSidebarMenuStore } from "@/stores/sidebarMenu";
    import { useBreadcrumbs } from "@/layout/composables/useBreadcrumbs.ts";

    const store = useSidebarMenuStore();
    const { home, items } = useBreadcrumbs();

    const adminMenu = [
        {
            label: "Security",
            icon: "pi pi-fw pi-shield",
            items: [
                { label: "Roles & Permissions", to: "/admin/roles" },
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
