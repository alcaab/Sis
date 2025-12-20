export interface UserDto {
    id: string; // Optional for creation, present for update/retrieval
    userName: string | null;
    email: string | null;
    phoneNumber: string | null;
    firstName: string | null;
    lastName: string | null;
    lockoutEnabled: boolean;
    roles: string[];
}
