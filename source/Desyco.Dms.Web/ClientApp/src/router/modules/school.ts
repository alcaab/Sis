import type { RouteRecordRaw } from 'vue-router';

const schoolRoutes: RouteRecordRaw[] = [
    {
        path: '/school-settings',
        name: 'school-settings',
        component: () => import('@/views/school/SchoolSettings.vue'),
        meta: {
            breadcrumb: 'School Settings'
        },
        children: [
            {
                path: 'academic-year',
                name: 'academic-year-list',
                component: () => import('@/views/school/academic-years/AcademicYear.vue'),
                meta: {
                    breadcrumb: 'Academic Years'
                }
            },
            {
                path: 'academic-year/create',
                name: 'academic-year-create',
                component: () => import('@/views/school/academic-years/AcademicYearCreate.vue'),
                meta: {
                    breadcrumb: 'Create Academic Year'
                }
            },
            {
                path: 'academic-year/:id/edit',
                name: 'academic-year-edit',
                component: () => import('@/views/school/academic-years/AcademicYearEdit.vue'),
                meta: {
                    breadcrumb: 'Edit Academic Year'
                }
            }
        ]
    }
];

export default schoolRoutes;
