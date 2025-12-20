export enum PermissionAction {
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 3,
}

export interface PermissionSchema {
    entityId: string; // Guid
    entityName: string;
    groups: PermissionGroup[];
}

export interface PermissionGroup {
    groupName: string;
    order: number;
    features: PermissionItem[];
}

export interface PredefinedPermission {
    value: boolean;
    inherited?: boolean;
}

export interface PermissionItem {
    featureId: string;
    code: string;
    description: string;
    read: PredefinedPermission;
    write: PredefinedPermission;
    delete: PredefinedPermission;
    customPermissions: string[]; // e.g., ["Print"] - Granted ones
    availableCustomPermissions: string[]; // e.g., ["Approve", "Reject"] - Possible ones
}

export interface FeatureDto {
    id: string;
    code: string;
    description: string;
    group: string;
    order: number;
}

export interface FeaturePermission {
    featureId: string;
    featureCode: string;
    action: PermissionAction;
    customActionName?: string;
    isGranted: boolean;
}

export interface RoleClaimDto {
    id: number;
    roleId: string;
    claimType: string;
    claimValue: string;
    featureId: string | null;
    permissionAction: PermissionAction | null;
    description: string | null;
}
