export interface RoleDto {
    id: string; // Guid
    name: string;
    description: string | null;
}

export interface CreateRoleDto {
    name: string;
    description: string | null;
}

export interface UpdateRoleDto {
    name: string;
    description: string | null;
}
