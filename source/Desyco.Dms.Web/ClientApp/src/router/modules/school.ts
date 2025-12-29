import type { RouteRecordRaw } from "vue-router";
import i18n from "@/i18n";
import RouterOutlet from "@/components/common/RouterOutlet.vue";

const createAction = i18n.global.t(`common.crud.create`);
const updateAction = i18n.global.t(`common.crud.update`);

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
                component: RouterOutlet,
                meta: {
                    breadcrumb: i18n.global.t(`schoolSettings.academicYear.title`),
                },
                children: [
                    {
                        path: "",
                        name: "academic-year-list",
                        component: () => import("@/views/school/academic-years/AcademicYearList.vue"),
                    },
                    {
                        path: "create",
                        name: "academic-year-create",
                        component: () => import("@/views/school/academic-years/AcademicYearCreate.vue"),
                        meta: {
                            breadcrumb: createAction,
                        },
                    },
                    {
                        path: ":id/edit",
                        name: "academic-year-edit",
                        component: () => import("@/views/school/academic-years/AcademicYearEdit.vue"),
                        meta: {
                            breadcrumb: updateAction,
                        },
                    },
                ],
            },
            {
                path: "evaluation-period",
                component: RouterOutlet,
                meta: {
                    breadcrumb: i18n.global.t(`schoolSettings.evaluationPeriod.title`),
                },
                children: [
                    {
                        path: "",
                        name: "evaluation-period-list",
                        component: () => import("@/views/school/evaluation-periods/EvaluationPeriodList.vue"),
                    },
                    {
                        path: "create",
                        name: "evaluation-period-create",
                        component: () => import("@/views/school/evaluation-periods/EvaluationPeriodCreate.vue"),
                        meta: {
                            breadcrumb: createAction,
                        },
                    },
                    {
                        path: ":id/edit",
                        name: "evaluation-period-edit",
                        component: () => import("@/views/school/evaluation-periods/EvaluationPeriodEdit.vue"),
                        meta: {
                            breadcrumb: updateAction,
                        },
                    },
                ],
            },
            {
                path: "days-of-week",
                name: "days-of-week",
                component: () => import("@/views/school/days-of-week/WeeklyScheduleView.vue"),
                meta: {
                    breadcrumb: i18n.global.t(`schoolSettings.dayOfWeek.title`),
                },
            },
            {
                path: "special-days",
                name: "special-days",
                component: () => import("@/views/school/special-days/SpecialDaysView.vue"),
                meta: {
                    breadcrumb: i18n.global.t(`schoolSettings.specialDays.title`),
                },
            },
            {
                path: "educational-level",
                name: "educational-level-list",
                component: () => import("@/views/school/educational-levels/EducationalLevelList.vue"),
                meta: {
                    breadcrumb: i18n.global.t(`schoolSettings.educationalLevel.title`),
                },
            },
        ],
    },
];

export default schoolRoutes;
