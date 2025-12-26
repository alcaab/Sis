import { defineStore } from "pinia";
import { ref } from "vue";
import { EducationalLevelService } from "@/service/EducationalLevelService";
import type { EducationalLevelDto } from "@/types/educational-level";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";

export const useEducationalLevelStore = defineStore("educationalLevel", () => {
    const educationalLevels = ref<EducationalLevelDto[]>([]);
    const totalRecords = ref(0);
    const loading = ref(false);

    async function fetchEducationalLevels(params?: RequestParamsPayload) {
        loading.value = true;
        try {
            const response = await EducationalLevelService.getEducationalLevels(params);
            educationalLevels.value = response.data.results;
            totalRecords.value = response.data.count || 0;
        } catch (error) {
            console.error("Failed to fetch educational levels", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function createEducationalLevel(data: EducationalLevelDto) {
        loading.value = true;
        try {
            await EducationalLevelService.createEducationalLevel(data);
        } catch (error) {
            console.error("Failed to create educational level", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateEducationalLevel(id: number, data: EducationalLevelDto) {
        loading.value = true;
        try {
            await EducationalLevelService.updateEducationalLevel(id, data);
        } catch (error) {
            console.error("Failed to update educational level", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function deleteEducationalLevel(id: number) {
        loading.value = true;
        try {
            await EducationalLevelService.deleteEducationalLevel(id);
        } catch (error) {
            console.error("Failed to delete educational level", error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    return {
        educationalLevels,
        totalRecords,
        loading,
        fetchEducationalLevels,
        createEducationalLevel,
        updateEducationalLevel,
        deleteEducationalLevel,
    };
});
