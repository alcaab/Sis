import axios from "axios";
import type { LoginRequest, TokenResponse } from "@/types/auth";

const AUTH_API_URL = "/api/auth";

export const authService = {
    async login(credentials: LoginRequest): Promise<TokenResponse> {
        const response = await axios.post<TokenResponse>(`${AUTH_API_URL}/login`, credentials, {
            withCredentials: true,
        });
        return response.data;
    },

    async logout(): Promise<void> {
        await axios.post(
            `${AUTH_API_URL}/logout`,
            {},
            {
                withCredentials: true,
            },
        );
    },
};
