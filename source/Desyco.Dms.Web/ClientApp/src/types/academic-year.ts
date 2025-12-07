export enum AcademicYearStatus {
    Upcoming = 1,
    Current = 2,
    Closed = 3
}

export interface AcademicYearDto {
    id?: number;
    name: string;
    startDate?: Date | string | null;
    endDate?: Date | string | null;
    status: AcademicYearStatus;
}
