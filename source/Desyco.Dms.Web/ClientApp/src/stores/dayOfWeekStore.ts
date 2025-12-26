import { defineStore } from "pinia";
import { dayOfWeekService } from "@/service/DayOfWeekService";
import type { DayOfWeekDto, UpdateDayOfWeekCommand } from "@/types/days-of-week";

interface State {
    days: DayOfWeekDto[];
    loading: boolean;
    saving: boolean;
}

export const useDayOfWeekStore = defineStore("dayOfWeek", {
    state: (): State => ({
        days: [],
        loading: false,
        saving: false,
    }),
    actions: {
        async fetchDays() {
            this.loading = true;
            try {
                this.days = await dayOfWeekService.getAll();
            } catch (error) {
                console.error("Error fetching days:", error);
            } finally {
                this.loading = false;
            }
        },
        async updateDay(id: number, command: UpdateDayOfWeekCommand) {
            this.saving = true;
            try {
                const updatedDay = await dayOfWeekService.update(id, command);
                const index = this.days.findIndex((d) => d.id === id);
                if (index !== -1) {
                    this.days[index] = updatedDay;
                }
                return updatedDay;
            } catch (error) {
                console.error("Error updating day:", error);
                throw error;
            } finally {
                this.saving = false;
            }
        },
    },
});
