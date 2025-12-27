import axios from "axios";
import type { DayOfWeekDto, UpdateWeeklyScheduleCommand } from "@/types/days-of-week";

export class DayOfWeekService {
    async getAll(): Promise<DayOfWeekDto[]> {
        const response = await axios.get<DayOfWeekDto[]>("/api/v1/days-of-week");
        return response.data;
    }

    async updateBatch(command: UpdateWeeklyScheduleCommand): Promise<DayOfWeekDto[]> {
        const response = await axios.put<DayOfWeekDto[]>("/api/v1/days-of-week", command);
        return response.data;
    }
}

export const dayOfWeekService = new DayOfWeekService();