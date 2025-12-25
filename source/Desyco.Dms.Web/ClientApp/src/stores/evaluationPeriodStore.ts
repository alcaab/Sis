import { defineStore } from "pinia";
import { ref } from "vue";
import { EvaluationPeriodService } from "@/service/EvaluationPeriodService";
import type { EvaluationPeriodDto } from "@/types/evaluation-period";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";

export const useEvaluationPeriodStore = defineStore("evaluationPeriod", () => {
    const evaluationPeriods = ref<EvaluationPeriodDto[]>([]);
    const totalRecords = ref(0);
    const loading = ref(false);

    async function fetchEvaluationPeriods(params?: RequestParamsPayload) {
        loading.value = true;
        try {
            const response = await EvaluationPeriodService.getEvaluationPeriods(params);
            evaluationPeriods.value = response.data.results;
            totalRecords.value = response.data.count || 0;
        } catch (error) {
            console.error("Failed to fetch evaluation periods", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function fetchByAcademicYear(academicYearId: number) {
        loading.value = true;
        try {
            const response = await EvaluationPeriodService.getEvaluationPeriodsByAcademicYear(academicYearId);
            evaluationPeriods.value = response.data;
        } catch (error) {
            console.error("Failed to fetch evaluation periods by academic year", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function createEvaluationPeriod(data: EvaluationPeriodDto) {
        loading.value = true;
        try {
            await EvaluationPeriodService.createEvaluationPeriod(data);
        } catch (error) {
            console.error("Failed to create evaluation period", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateEvaluationPeriod(id: number, data: EvaluationPeriodDto) {
        loading.value = true;
        try {
            await EvaluationPeriodService.updateEvaluationPeriod(id, data);
        } catch (error) {
            console.error("Failed to update evaluation period", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function deleteEvaluationPeriod(id: number) {
        loading.value = true;
        try {
            await EvaluationPeriodService.deleteEvaluationPeriod(id);
        } catch (error) {
            console.error("Failed to delete evaluation period", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    return {
        evaluationPeriods,
        totalRecords,
        loading,
        fetchEvaluationPeriods,
        createEvaluationPeriod,
        updateEvaluationPeriod,
        deleteEvaluationPeriod,
        fetchByAcademicYear,
    };
});
