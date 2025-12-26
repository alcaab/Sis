export enum EducationalLevelType {
    EarlyChildhood = 0,
    Primary = 1,
    Secondary = 2
}

export interface EducationalLevelDto {
    id: number;
    levelTypeId: EducationalLevelType;
    name: string;
    isActive: boolean;
}
