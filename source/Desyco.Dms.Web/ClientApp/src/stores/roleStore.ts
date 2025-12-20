import { defineStore } from "pinia";
import { ref } from "vue";
import { roleService } from "@/service/roleService";
import { permissionService } from "@/service/permissionService";
import type { RoleDto } from "@/types/role";
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

    async function createRole(role: RoleDto) {
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

    async function updateRole(id: string, role: RoleDto) {
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
    };
});
