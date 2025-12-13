import type { RouteRecordRaw } from "vue-router";

const adminRoutes: RouteRecordRaw[] = [
    {
        path: "/admin",
        name: "admin",
        redirect: { name: "roles-list" },
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
            // ... Aquí podríamos añadir usuarios, etc.
        ],
    },
];

export default adminRoutes;
