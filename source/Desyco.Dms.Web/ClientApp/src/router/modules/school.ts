import type { RouteRecordRaw } from "vue-router";

const schoolRoutes: RouteRecordRaw[] = [
    {
        path: "/school-settings",
        name: "school-settings",
        redirect: { name: "academic-year-list" },
        component: () => import("@/views/school/SchoolSettings.vue"),
        meta: {
            breadcrumb: "School Settings",
        },
        children: [
            {
                path: "academic-year",
                name: "academic-year-list",
                component: () => import("@/views/school/academic-years/AcademicYearList.vue"),
                meta: {
                    breadcrumb: "Academic Years",
                },
            },
            {
                path: "academic-year/create",
                name: "academic-year-create",
                component: () => import("@/views/school/academic-years/AcademicYearCreate.vue"),
                meta: {
                    breadcrumb: "Create Academic Year",
                },
            },
            {
                path: "academic-year/:id/edit",
                name: "academic-year-edit",
                component: () => import("@/views/school/academic-years/AcademicYearEdit.vue"),
                meta: {
                    breadcrumb: "Edit Academic Year",
                },
            },
            {
                path: "evaluation-period",
                name: "evaluation-period-list",
                component: () => import("@/views/school/evaluation-periods/EvaluationPeriodList.vue"),
                meta: {
                    breadcrumb: "Evaluation Periods",
                },
            },
            {
                path: "evaluation-period/create",
                name: "evaluation-period-create",
                component: () => import("@/views/school/evaluation-periods/EvaluationPeriodCreate.vue"),
                meta: {
                    breadcrumb: "Create Evaluation Period",
                },
            },
            {
                path: "evaluation-period/:id/edit",
                name: "evaluation-period-edit",
                component: () => import("@/views/school/evaluation-periods/EvaluationPeriodEdit.vue"),
                meta: {
                    breadcrumb: "Edit Evaluation Period",
                },
            },
            {
                path: "days-of-week",
                name: "days-of-week",
                component: () => import("@/views/school/days-of-week/WeeklyScheduleContainer.vue"),
                meta: {
                    breadcrumb: "Days of the Week",
                },
            },
            {
                path: "educational-level",
                name: "educational-level-list",
                component: () => import("@/views/school/educational-levels/EducationalLevelList.vue"),
                meta: {
                    breadcrumb: "Educational Levels",
                },
            },
        ],
    },
];

export default schoolRoutes;
