export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
}

export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
}

export interface TokenResponse {
    accessToken: string;
    refreshToken: string; // Will be empty from server due to HttpOnly, but kept for type compat
    expiryDate: string;
    userId: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
}
