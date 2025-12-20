import type { RouteRecordRaw } from "vue-router";

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
                name: "roles-list",
                component: () => import("@/views/admin/roles/RolesList.vue"),
                meta: {
                    breadcrumb: "Roles",
                },
            },
            {
                path: "roles/:id/permissions",
                name: "role-permissions",
                component: () => import("@/views/admin/roles/RolePermissions.vue"),
                meta: {
                    breadcrumb: "Role Permissions",
                },
            },
            {
                path: "users",
                name: "users-list",
                component: () => import("@/views/admin/users/UsersList.vue"),
                meta: {
                    breadcrumb: "Users",
                },
            },
            {
                path: "users/create",
                name: "user-create",
                component: () => import("@/views/admin/users/UserCreate.vue"),
                meta: {
                    breadcrumb: "Create User",
                },
            },
            {
                path: "users/:id/edit",
                name: "user-edit",
                component: () => import("@/views/admin/users/UserEdit.vue"),
                meta: {
                    breadcrumb: "Edit User",
                },
            },
            {
                path: "users/:id/permissions",
                name: "user-permissions",
                component: () => import("@/views/admin/users/UserPermissions.vue"),
                meta: {
                    breadcrumb: "User Permissions",
                },
            },
        ],
    },
];

export default adminRoutes;
