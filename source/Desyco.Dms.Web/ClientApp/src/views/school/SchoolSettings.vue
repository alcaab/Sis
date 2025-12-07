<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import { useSidebarMenuStore } from '@/stores/sidebarMenu';
import { useBreadcrumbs } from '@/layout/composables/useBreadcrumbs.ts';

const { t } = useI18n();
const store = useSidebarMenuStore();
const { home, items } = useBreadcrumbs();

const staticMenu = [
    {
        label: t('schoolSettings.assess.title'),
        icon: 'pi pi-fw pi-chart-bar',
        items: [
            { label: t('schoolSettings.assess.markbookSettings'), to: '/school/assess/markbook-settings' },
            { label: t('schoolSettings.assess.formalAssessmentSettings'), to: '/school/assess/formal-assessment-settings' },
            { label: t('schoolSettings.assess.manageGradeScales'), to: '/school/assess/manage-grade-scales' },
            { label: t('schoolSettings.assess.manageExternalAssessments'), to: '/school/assess/manage-external-assessments' },
            { label: t('schoolSettings.assess.trackingSettings'), to: '/school/assess/tracking-settings' }
        ]
    },
    {
        label: t('schoolSettings.groupings.title'),
        icon: 'pi pi-fw pi-sitemap',
        items: [
            { label: t('schoolSettings.groupings.manageYearGroups'), to: '/school/groupings/manage-year-groups' },
            { label: t('schoolSettings.groupings.manageFormGroups'), to: '/school/groupings/manage-form-groups' },
            { label: t('schoolSettings.groupings.manageHouses'), to: '/school/groupings/manage-houses' },
            { label: t('schoolSettings.groupings.manageDepartments'), to: '/school/groupings/manage-departments' }
        ]
    },
    {
        label: t('schoolSettings.learn.title'),
        icon: 'pi pi-fw pi-book',
        items: [
            { label: t('schoolSettings.learn.plannerSettings'), to: '/school/learn/planner-settings' },
            { label: t('schoolSettings.learn.librarySettings'), to: '/school/learn/library-settings' },
            { label: t('schoolSettings.learn.resourceSettings'), to: '/school/learn/resource-settings' },
            { label: t('schoolSettings.learn.activitySettings'), to: '/school/learn/activity-settings' }
        ]
    },
    {
        label: t('schoolSettings.other.title'),
        icon: 'pi pi-fw pi-cog',
        items: [
            { label: t('schoolSettings.other.facilitySettings'), to: '/school/other/facility-settings' },
            { label: t('schoolSettings.other.manageFacilities'), to: '/school/other/manage-facilities' },
            { label: t('schoolSettings.other.dashboardSettings'), to: '/school/other/dashboard-settings' },
            { label: t('schoolSettings.other.financeSettings'), to: '/school/other/finance-settings' },
            { label: t('schoolSettings.other.emailSummarySettings'), to: '/school/other/email-summary-settings' },
            { label: t('schoolSettings.other.messengerSettings'), to: '/school/other/messenger-settings' },
            { label: t('schoolSettings.other.manageFileExtensions'), to: '/school/other/manage-file-extensions' }
        ]
    },
    {
        label: t('schoolSettings.people.title'),
        icon: 'pi pi-fw pi-users',
        items: [
            { label: t('schoolSettings.people.admissionsSettings'), to: '/school/people/admissions-settings' },
            { label: t('schoolSettings.people.behaviourSettings'), to: '/school/people/behaviour-settings' },
            { label: t('schoolSettings.people.attendanceSettings'), to: '/school/people/attendance-settings' },
            { label: t('schoolSettings.people.individualNeedsSettings'), to: '/school/people/individual-needs-settings' },
            { label: t('schoolSettings.people.manageMedicalConditions'), to: '/school/people/manage-medical-conditions' },
            { label: t('schoolSettings.people.manageAlertLevels'), to: '/school/people/manage-alert-levels' }
        ]
    },
    {
        label: t('schoolSettings.yearsDaysTimes.title'),
        icon: 'pi pi-fw pi-calendar',
        items: [
            { label: t('schoolSettings.yearsDaysTimes.manageSchoolYears'), to: '/school-settings/academic-year' },
            { label: t('schoolSettings.yearsDaysTimes.manageTerms'), to: '/school/years-days-times/manage-terms' },
            { label: t('schoolSettings.yearsDaysTimes.daysOfTheWeek'), to: '/school/years-days-times/days-of-the-week' },
            { label: t('schoolSettings.yearsDaysTimes.manageSpecialDays'), to: '/school/years-days-times/manage-special-days' }
        ]
    }
];

store.setMenu(staticMenu);
</script>

<template>
    <div class="card">
        <Breadcrumb :home="home" :model="items" />
        <router-view></router-view>
    </div>
</template>

<style scoped lang="scss"></style>
