<script setup lang="ts">
    import { useI18n } from "vue-i18n";
    import { useSidebarMenuStore } from "@/stores/sidebarMenu";
    import { useBreadcrumbs } from "@/layout/composables/useBreadcrumbs.ts";

    const { t } = useI18n();
    const store = useSidebarMenuStore();
    const { home, items } = useBreadcrumbs();

    const staticMenu = [
        {
            label: t("schoolSettings.panelGroups.others.title"),
            icon: "pi pi-fw pi-calendar",
            items: [
                {
                    label: t("schoolSettings.panelGroups.others.manageTermTypes"),
                    to: "/school-settings/evaluation-period-type",
                },
            ],
        },
        {
            label: t("schoolSettings.panelGroups.yearsDaysTimes.title"),
            icon: "pi pi-fw pi-calendar",
            items: [
                {
                    label: t("schoolSettings.panelGroups.yearsDaysTimes.manageSchoolYears"),
                    to: "/school-settings/academic-year",
                },
                {
                    label: t("schoolSettings.panelGroups.yearsDaysTimes.manageTerms"),
                    to: "/school-settings/evaluation-period",
                },
                {
                    label: t("schoolSettings.panelGroups.yearsDaysTimes.daysOfTheWeek"),
                    to: "/school/years-days-times/days-of-the-week",
                },
                {
                    label: t("schoolSettings.panelGroups.yearsDaysTimes.manageSpecialDays"),
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
