import { defineStore } from 'pinia';
import { ref } from 'vue';
import { AcademicYearService } from '@/service/AcademicYearService';
import type { AcademicYearDto } from '@/types/academic-year';
import type { RequestParamsPayload } from '@/utils/queryOptions/queryOptionModels';

export const useAcademicYearStore = defineStore('academicYear', () => {
    const academicYears = ref<AcademicYearDto[]>([]);
    const totalRecords = ref(0);
    const loading = ref(false);

    async function fetchAcademicYears(params?: RequestParamsPayload) {
        loading.value = true;
        try {
            const response = await AcademicYearService.getAcademicYears(params);
            academicYears.value = response.data.results;
            totalRecords.value = response.data.count || 0;
        } catch (error) {
            console.error('Failed to fetch academic years', error);
        } finally {
            loading.value = false;
        }
    }

    async function createAcademicYear(data: AcademicYearDto) {
        loading.value = true;
        try {
            await AcademicYearService.createAcademicYear(data);
            await fetchAcademicYears();
        } catch (error) {
            console.error('Failed to create academic year', error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function updateAcademicYear(id: number, data: AcademicYearDto) {
        loading.value = true;
        try {
            await AcademicYearService.updateAcademicYear(id, data);
            await fetchAcademicYears();
        } catch (error) {
            console.error('Failed to update academic year', error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    async function deleteAcademicYear(id: number) {
        loading.value = true;
        try {
            await AcademicYearService.deleteAcademicYear(id);
            await fetchAcademicYears();
        } catch (error) {
            console.error('Failed to delete academic year', error);
            throw error;
        } finally {
            loading.value = false;
        }
    }

    return {
        academicYears,
        totalRecords,
        loading,
        fetchAcademicYears,
        createAcademicYear,
        updateAcademicYear,
        deleteAcademicYear
    };
});
