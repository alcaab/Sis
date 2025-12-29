import api from "./api";
import type { DayOfWeekDto, WeeklyScheduleDto } from "@/types/days-of-week";

export class DayOfWeekService {
    async getAll(): Promise<DayOfWeekDto[]> {
        const response = await api.get<DayOfWeekDto[]>("/days-of-week");
        return response.data;
    }

    async updateBatch(model: WeeklyScheduleDto): Promise<DayOfWeekDto[]> {
        const response = await api.put<DayOfWeekDto[]>("/days-of-week", model);
        return response.data;
    }
}

export const dayOfWeekService = new DayOfWeekService();
