import type { RouteRecordRaw } from "vue-router";
import RouterOutlet from "@/components/common/RouterOutlet.vue";
import i18n from "@/i18n.ts";

const createAction = i18n.global.t(`common.crud.create`);
const updateAction = i18n.global.t(`common.crud.update`);

const adminRoutes: RouteRecordRaw[] = [
    {
        path: "/admin",
        name: "admin",
        redirect: { name: "users-list" },
        component: () => import("@/views/admin/AdminSettings.vue"),
        meta: {
            breadcrumb: "Administration",
            requiresAuth: true,
        },
        children: [
            {
                path: "roles",
                component: RouterOutlet,
                meta: {
                    breadcrumb: i18n.global.t(`admin.roles.title`),
                },
                children: [
                    {
                        path: "",
                        name: "roles-list",
                        component: () => import("@/views/admin/roles/RolesList.vue"),
                    },
                    {
                        path: ":id/permissions",
                        name: "role-permissions",
                        component: () => import("@/views/admin/roles/RolePermissions.vue"),
                        meta: {
                            breadcrumb: i18n.global.t(`admin.permissions.title`),
                        },
                    },
                ],
            },
            {
                path: "users",
                component: RouterOutlet,
                meta: {
                    breadcrumb: "Users",
                },
                children: [
                    {
                        path: "",
                        name: "users-list",
                        component: () => import("@/views/admin/users/UsersList.vue"),
                    },
                    {
                        path: "create",
                        name: "user-create",
                        component: () => import("@/views/admin/users/UserCreate.vue"),
                        meta: {
                            breadcrumb: createAction,
                        },
                    },
                    {
                        path: ":id/edit",
                        name: "user-edit",
                        component: () => import("@/views/admin/users/UserEdit.vue"),
                        meta: {
                            breadcrumb: updateAction,
                        },
                    },
                    {
                        path: ":id/permissions",
                        name: "user-permissions",
                        component: () => import("@/views/admin/users/UserPermissions.vue"),
                        meta: {
                            breadcrumb: i18n.global.t(`admin.permissions.title`),
                        },
                    },
                ],
            },
        ],
    },
];

export default adminRoutes;
