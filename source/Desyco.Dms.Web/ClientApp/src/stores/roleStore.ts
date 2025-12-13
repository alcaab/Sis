import { defineStore } from "pinia";
import { ref } from "vue";
import { roleService } from "@/service/roleService";
import { permissionService } from "@/service/permissionService";
import type { RoleDto, CreateRoleDto, UpdateRoleDto } from "@/types/role";
import type { PermissionSchema, FeaturePermission } from "@/types/permissions";

export const useRoleStore = defineStore("role", () => {
    const roles = ref<RoleDto[]>([]);
    const currentRolePermissions = ref<PermissionSchema | null>(null);
    const loading = ref(false);

    async function fetchAllRoles() {
        loading.value = true;
        try {
            roles.value = await roleService.getAll();
        } catch (error) {
            console.error("Failed to fetch roles:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function createRole(role: CreateRoleDto) {
        loading.value = true;
        try {
            const newRole = await roleService.create(role);
            await fetchAllRoles(); // Refresh list
            return newRole;
        } catch (error) {
            console.error("Failed to create role:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateRole(id: string, role: UpdateRoleDto) {
        loading.value = true;
        try {
            await roleService.update(id, role);
            await fetchAllRoles(); // Refresh list
        } catch (error) {
            console.error("Failed to update role:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function deleteRole(id: string) {
        loading.value = true;
        try {
            await roleService.delete(id);
            await fetchAllRoles(); // Refresh list
        } catch (error) {
            console.error("Failed to delete role:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function fetchRolePermissions(roleId: string) {
        loading.value = true;
        try {
            currentRolePermissions.value = await permissionService.getPermissionSchemaForRole(roleId);
        } catch (error) {
            console.error(`Failed to fetch permissions for role ${roleId}:`, error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateRolePermissions(roleId: string, permissions: FeaturePermission[]) {
        loading.value = true;
        try {
            await permissionService.updateRolePermissions(roleId, permissions);
            // Optionally, re-fetch permissions to ensure store is updated
            await fetchRolePermissions(roleId);
        } catch (error) {
            console.error(`Failed to update permissions for role ${roleId}:`, error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    // For Role Scope Management
    async function fetchAssignedFeatureIds(roleId: string): Promise<string[]> {
        loading.value = true;
        try {
            return await permissionService.getAssignedFeatureIds(roleId);
        } catch (error) {
            console.error(`Failed to fetch assigned features for role ${roleId}:`, error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateAssignedFeatures(roleId: string, featureIds: string[]): Promise<void> {
        loading.value = true;
        try {
            await permissionService.updateAssignedFeatures(roleId, featureIds);
            // After updating assigned features, permissions for that role might change,
            // so we should refresh the permission schema if it's currently loaded.
            if (currentRolePermissions.value?.entityId === roleId) {
                await fetchRolePermissions(roleId);
            }
        } catch (error) {
            console.error(`Failed to update assigned features for role ${roleId}:`, error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    return {
        roles,
        currentRolePermissions,
        loading,
        fetchAllRoles,
        createRole,
        updateRole,
        deleteRole,
        fetchRolePermissions,
        updateRolePermissions,
        fetchAssignedFeatureIds,
        updateAssignedFeatures,
    };
});
