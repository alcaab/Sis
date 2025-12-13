import api from "./api";
import type { RoleDto, CreateRoleDto, UpdateRoleDto } from "@/types/role";

const ROLES_API_URL = "/api/roles";

export const roleService = {
    async getAll(): Promise<RoleDto[]> {
        const response = await api.get<RoleDto[]>(ROLES_API_URL);
        return response.data;
    },

    async getById(id: string): Promise<RoleDto> {
        const response = await api.get<RoleDto>(`${ROLES_API_URL}/${id}`);
        return response.data;
    },

    async create(data: CreateRoleDto): Promise<RoleDto> {
        const response = await api.post<RoleDto>(ROLES_API_URL, data);
        return response.data;
    },

    async update(id: string, data: UpdateRoleDto): Promise<void> {
        await api.put(`${ROLES_API_URL}/${id}`, data);
    },

    async delete(id: string): Promise<void> {
        await api.delete(`${ROLES_API_URL}/${id}`);
    }
};
