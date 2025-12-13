import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { authService } from "@/service/authService";
import type { LoginRequest, User, TokenResponse } from "@/types/auth";
import api from "@/service/api";

export const useAuthStore = defineStore("auth", () => {
    const user = ref<User | null>(null);
    const token = ref<string | null>(localStorage.getItem("accessToken"));
    const isAuthenticated = computed(() => !!token.value);

    // Set initial header if token exists
    if (token.value) {
        api.defaults.headers.common["Authorization"] = `Bearer ${token.value}`;
    }

    async function login(credentials: LoginRequest) {
        // eslint-disable-next-line no-useless-catch
        try {
            const response: TokenResponse = await authService.login(credentials);
            setSession(response);
        } catch (error) {
            throw error;
        }
    }

    async function logout() {
        try {
            await authService.logout();
        } finally {
            clearSession();
        }
    }

    function setSession(response: TokenResponse) {
        token.value = response.accessToken;
        user.value = {
            id: response.userId,
            email: response.email,
            firstName: response.firstName,
            lastName: response.lastName,
            roles: response.roles,
        };

        localStorage.setItem("accessToken", response.accessToken);
        api.defaults.headers.common["Authorization"] = `Bearer ${response.accessToken}`;
    }

    function clearSession() {
        token.value = null;
        user.value = null;
        localStorage.removeItem("accessToken");
        delete api.defaults.headers.common["Authorization"];
        // Redirect handled by component or router guard usually, but safe fallback:
        // window.location.href = "/login";
    }

    return {
        user,
        token,
        isAuthenticated,
        login,
        logout,
    };
});
