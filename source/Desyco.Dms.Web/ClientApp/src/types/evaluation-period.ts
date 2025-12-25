export interface EvaluationPeriodDto {
    id?: number;
    academicYearId: number;
    levelTypeId: number;
    name: string;
    shortName: string;
    startDate: string | Date | null;
    endDate: string | Date | null;
    sequence?: number | null;
    weight: number;
}
