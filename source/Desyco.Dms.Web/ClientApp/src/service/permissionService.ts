import api from "./api";
import type { PermissionSchema, FeaturePermission } from "@/types/permissions";

const PERMISSIONS_API_URL = "permissions";

export const permissionService = {
    async getPermissionSchemaForRole(roleId: string): Promise<PermissionSchema> {
        const response = await api.get<PermissionSchema>(`${PERMISSIONS_API_URL}/schema/role/${roleId}`);
        return response.data;
    },

    async updateRolePermissions(roleId: string, permissions: FeaturePermission[]): Promise<void> {
        await api.put(`${PERMISSIONS_API_URL}/roles/${roleId}`, permissions);
    },

    async getPermissionSchemaForUser(userId: string): Promise<PermissionSchema> {
        const response = await api.get<PermissionSchema>(`${PERMISSIONS_API_URL}/schema/user/${userId}`);
        return response.data;
    },

    async updateUserPermissions(userId: string, permissions: FeaturePermission[]): Promise<void> {
        await api.put(`${PERMISSIONS_API_URL}/users/${userId}`, permissions);
    },
};
