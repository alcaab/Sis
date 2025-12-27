import { defineStore } from "pinia";
import { dayOfWeekService } from "@/service/DayOfWeekService";
import type { DayOfWeekDto, UpdateWeeklyScheduleCommand } from "@/types/days-of-week";

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
        async updateAll(command: UpdateWeeklyScheduleCommand) {
            this.saving = true;
            try {
                this.days = await dayOfWeekService.updateBatch(command);
            } catch (error) {
                console.error("Error updating weekly schedule:", error);
                throw error;
            } finally {
                this.saving = false;
            }
        },
    },
});