<script setup lang="ts">
    import { useI18n } from "vue-i18n";
    import { useSidebarMenuStore } from "@/stores/sidebarMenu";
    import { useBreadcrumbs } from "@/layout/composables/useBreadcrumbs.ts";

    const { t } = useI18n();
    const store = useSidebarMenuStore();
    const { home, items } = useBreadcrumbs();

    const staticMenu = [
        {
            label: t("schoolSettings.yearsDaysTimes.title"),
            icon: "pi pi-fw pi-calendar",
            items: [
                { label: t("schoolSettings.yearsDaysTimes.manageSchoolYears"), to: "/school-settings/academic-year" },
                { label: t("schoolSettings.yearsDaysTimes.manageTerms"), to: "/school/years-days-times/manage-terms" },
                {
                    label: t("schoolSettings.yearsDaysTimes.daysOfTheWeek"),
                    to: "/school/years-days-times/days-of-the-week",
                },
                {
                    label: t("schoolSettings.yearsDaysTimes.manageSpecialDays"),
                    to: "/school/years-days-times/manage-special-days",
                },
            ],
        },
    ];

    store.setMenu(staticMenu);
</script>

<template>
    <div class="card">
        <Breadcrumb
            :home="home"
            :model="items"
        />
        <router-view></router-view>
    </div>
</template>

<style scoped lang="scss"></style>
