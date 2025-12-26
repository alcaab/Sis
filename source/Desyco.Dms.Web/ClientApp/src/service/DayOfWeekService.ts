import axios from "axios";
import type { DayOfWeekDto, UpdateDayOfWeekCommand } from "@/types/days-of-week";

export class DayOfWeekService {
    async getAll(): Promise<DayOfWeekDto[]> {
        const response = await axios.get<DayOfWeekDto[]>("/api/v1/days-of-week");
        return response.data;
    }

    async update(id: number, command: UpdateDayOfWeekCommand): Promise<DayOfWeekDto> {
        const response = await axios.put<DayOfWeekDto>(`/api/v1/days-of-week/${id}`, command);
        return response.data;
    }
}

export const dayOfWeekService = new DayOfWeekService();
