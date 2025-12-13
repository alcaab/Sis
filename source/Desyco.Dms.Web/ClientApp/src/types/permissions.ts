export enum PermissionAction {
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 3,
}

// Corresponds to PermissionSchemaDto in C#
export interface PermissionSchema {
    entityId: string; // Guid
    entityName: string;
    groups: PermissionGroup[];
}

// Corresponds to PermissionGroupDto in C#
export interface PermissionGroup {
    groupName: string;
    order: number;
    features: PermissionItem[];
}

// Corresponds to PermissionItemDto in C#
export interface PermissionItem {
    featureId: string; // Guid
    code: string;
    description: string;
    read: boolean;
    write: boolean;
    delete: boolean;
    customPermissions: string[]; // e.g., ["Print"] - Granted ones
    availableCustomPermissions: string[]; // e.g., ["Approve", "Reject"] - Possible ones
}

// Corresponds to FeatureDto in C#
export interface FeatureDto {
    id: string; // Guid
    code: string;
    description: string;
    group: string;
    order: number;
}

// Corresponds to FeaturePermission in C# (for update requests)
export interface FeaturePermission {
    featureId: string; // Guid
    featureCode: string;
    action: PermissionAction;
    customActionName?: string; // For custom actions (when Action is None)
    isGranted: boolean;
}

// Corresponds to RoleClaimDto in C# (for listing individual claims, less used in UI)
export interface RoleClaimDto {
    id: number;
    roleId: string; // Guid
    claimType: string;
    claimValue: string;
    featureId: string | null; // Guid
    permissionAction: PermissionAction | null;
    description: string | null;
}
