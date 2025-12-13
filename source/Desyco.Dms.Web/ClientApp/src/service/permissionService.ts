import api from "./api";
import type { PermissionSchema, FeatureDto, FeaturePermission } from "@/types/permissions";

const PERMISSIONS_API_URL = "permissions";

export const permissionService = {
    async getAllFeatures(): Promise<FeatureDto[]> {
        const response = await api.get<FeatureDto[]>(`${PERMISSIONS_API_URL}/features`);
        return response.data;
    },

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

    async getAssignedFeatureIds(roleId: string): Promise<string[]> {
        const response = await api.get<string[]>(`${PERMISSIONS_API_URL}/role-features/${roleId}`);
        return response.data;
    },

    async updateAssignedFeatures(roleId: string, featureIds: string[]): Promise<void> {
        await api.put(`${PERMISSIONS_API_URL}/role-features/${roleId}`, featureIds);
    },
};
