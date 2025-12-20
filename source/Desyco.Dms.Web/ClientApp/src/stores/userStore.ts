import { defineStore } from "pinia";
import { ref } from "vue";
import { userService } from "@/service/userService";
import { permissionService } from "@/service/permissionService";
import type { UserDto } from "@/types/user";
import type { FeaturePermission, PermissionSchema } from "@/types/permissions";

export const useUserStore = defineStore("user", () => {
    const users = ref<UserDto[]>([]);
    const currentUserPermissions = ref<PermissionSchema | null>(null);
    const loading = ref(false);

    async function fetchAllUsers() {
        loading.value = true;
        try {
            users.value = await userService.getAll();
        } catch (error) {
            console.error("Failed to fetch users:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function createUser(user: UserDto) {
        loading.value = true;
        try {
            const newUser = await userService.create(user);
            await fetchAllUsers();
            return newUser;
        } catch (error) {
            console.error("Failed to create user:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateUser(id: string, user: UserDto) {
        loading.value = true;
        try {
            await userService.update(id, user);
            await fetchAllUsers(); // Refresh list
        } catch (error) {
            console.error("Failed to update user:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function deleteUser(id: string) {
        loading.value = true;
        try {
            await userService.delete(id);
            await fetchAllUsers(); // Refresh list
        } catch (error) {
            console.error("Failed to delete user:", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function fetchUserPermissions(userId: string) {
        loading.value = true;
        try {
            currentUserPermissions.value = await permissionService.getPermissionSchemaForUser(userId);
        } catch (error) {
            console.error(`Failed to fetch permissions for user ${userId}:`, error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateUserPermissions(userId: string, permissions: FeaturePermission[]) {
        loading.value = true;
        try {
            await permissionService.updateUserPermissions(userId, permissions);
            await fetchUserPermissions(userId);
        } catch (error) {
            console.error(`Failed to update permissions for user ${userId}:`, error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    return {
        users,
        currentUserPermissions,
        loading,
        fetchAllUsers,
        createUser,
        updateUser,
        deleteUser,
        fetchUserPermissions,
        updateUserPermissions,
    };
});
