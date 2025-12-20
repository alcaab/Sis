import api from "./api";
import type { RoleDto } from "@/types/role";

const ROLES_API_URL = "roles";

export const roleService = {
    async getAll(): Promise<RoleDto[]> {
        const response = await api.get<RoleDto[]>(ROLES_API_URL);
        return response.data;
    },

    async getNames(): Promise<string[]> {
        const response = await api.get<string[]>(`${ROLES_API_URL}/names`);
        return response.data;
    },

    async getById(id: string): Promise<RoleDto> {
        const response = await api.get<RoleDto>(`${ROLES_API_URL}/${id}`);
        return response.data;
    },

    async create(data: RoleDto): Promise<RoleDto> {
        const response = await api.post<RoleDto>(ROLES_API_URL, data);
        return response.data;
    },

    async update(id: string, data: RoleDto): Promise<void> {
        await api.put(`${ROLES_API_URL}/${id}`, data);
    },

    async delete(id: string): Promise<void> {
        await api.delete(`${ROLES_API_URL}/${id}`);
    },
};
