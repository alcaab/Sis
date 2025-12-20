import api from "./api";
import type { UserDto } from "@/types/user";

const USERS_API_URL = "users";

export const userService = {
    async getAll(): Promise<UserDto[]> {
        const response = await api.get<UserDto[]>(USERS_API_URL);
        return response.data;
    },

    async getById(id: string): Promise<UserDto> {
        const response = await api.get<UserDto>(`${USERS_API_URL}/${id}`);
        return response.data;
    },

    async create(data: UserDto): Promise<UserDto> {
        const response = await api.post<UserDto>(USERS_API_URL, data);
        return response.data;
    },

    async update(id: string, data: UserDto): Promise<void> {
        await api.put(`${USERS_API_URL}/${id}`, data);
    },

    async delete(id: string): Promise<void> {
        await api.delete(`${USERS_API_URL}/${id}`);
    },
};
