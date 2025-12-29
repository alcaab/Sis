export enum DayOfWeek {
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
}

export interface DayOfWeekDto {
    id: number;
    name: string;
    shortName: string;
    openTime: string | null;
    startTime: string | null;
    endTime: string | null;
    closeTime: string | null;
    isStartOfWeek?: boolean;
    isEndOfWeek?: boolean;
    isSchoolDay: boolean;
}

export interface WeeklyScheduleDto {
    days: DayOfWeekDto[];
}
