export enum SpecialDayType {
    SchoolClosure = 0,
    TimingChange = 1
}

export interface SpecialDayDto {
    id: number;
    evaluationPeriodId: number;
    type: SpecialDayType;
    name: string;
    description: string;
    date: string; // ISO DateOnly YYYY-MM-DD
    openTime?: string; // ISO TimeOnly HH:mm:ss
    startTime?: string;
    endTime?: string;
    closeTime?: string;
}

export interface SpecialDayEvaluationPeriodDto {
    id: number;
    name: string;
    startDate: Date; // Formato ISO "YYYY-MM-DD"
    endDate: Date; // Formato ISO "YYYY-MM-DD"
    sequence: number | null;
    days: SpecialDayDto[];
    numberOfMonths: number;
}

export interface SpecialDayEducationalLevelDto {
    id: number;
    name: string | null;
    periods: SpecialDayEvaluationPeriodDto[];
}

export interface CreateSpecialDayDto extends Omit<SpecialDayDto, 'id'> {}
export interface UpdateSpecialDayDto extends SpecialDayDto {}
