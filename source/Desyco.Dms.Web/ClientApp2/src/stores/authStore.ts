import { defineStore } from "pinia";
import { User } from "oidc-client-ts";
import { computed, ref } from "vue";
import { AuthenticationService, UserClaimsService } from "netto-primevue";
import userManager from "@/oidcConfig";
import axios from "axios";

export const useAuthStore = defineStore("auth", () => {
  const authenticationService = new AuthenticationService();
  const userClaimsService = new UserClaimsService();

  const user = ref<User | undefined>(undefined);
  const isAuthenticated = computed(
    () => user.value != undefined && !user.value?.expired
  );

  async function login(): Promise<void> {
    await setAxiosToken();
    user.value = await authenticationService.login();
  }

  async function setAxiosToken(): Promise<void> {
    axios.interceptors.request.use(async (config) => {
      config.headers.Authorization = `Bearer ${await getAccessToken()}`;
      return config;
    });
  }

  async function getAccessToken(): Promise<string | undefined> {
    if (!isAuthenticated.value) {
      throw new Error(
        "Error trying to get access token of unauthenticated user"
      );
    }

    return user.value?.access_token;
  }

  async function hasClaims(claims: string[]): Promise<boolean> {
    if (!isAuthenticated.value) {
      return false;
    }

    return await userClaimsService.hasAnyClaim("kassentableau_permission", claims);
  }

  async function getAllClaims(): Promise<string[]> {
    if (!isAuthenticated.value) {
      return false;
    }

    return await userClaimsService.getUserClaims("kassentableau_permission");
  }

  async function updateUser(): Promise<void> {
    const userFromStorage = await userManager.getUser();

    if (userFromStorage != null) {
      user.value = userFromStorage;
    }
  }

  return {
    user,
    isAuthenticated,
    login,
    hasClaims,
    updateUser,
    getAllClaims,
  };
});
