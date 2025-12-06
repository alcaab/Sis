export enum AcademicYearStatus {
    Upcoming = 1,
    Current = 2,
    Closed = 3
}

export interface AcademicYearDto {
    id: number;
    name: string;
    startDate?: string | null;
    endDate?: string | null;
    status: AcademicYearStatus;
}

export interface CreateAcademicYearDto {
    name: string;
    startDate?: string | null;
    endDate?: string | null;
    status: AcademicYearStatus;
}

export interface UpdateAcademicYearDto {
    id: number;
    name: string;
    startDate?: string | null;
    endDate?: string | null;
    status: AcademicYearStatus;
}
