import { defineStore } from 'pinia';
import { ref } from 'vue';
import {
    SpecialDayDto,
    CreateSpecialDayDto,
    UpdateSpecialDayDto,
    SpecialDayEducationalLevelDto,
} from "@/types/special-days";
import SpecialDayService from '@/service/SpecialDayService.ts';

export const useSpecialDayStore = defineStore('specialDay', () => {
    const specialDays = ref<SpecialDayDto[]>([]);
    const specialDayGroups = ref<SpecialDayEducationalLevelDto[]>([]);
    const loading = ref(false);
    const error = ref<string | null>(null);

    async function loadByAcademicYear(academicYearId: number) {
        if (!academicYearId) return;
        loading.value = true;
        error.value = null;
        try {
            specialDayGroups.value = await SpecialDayService.getByAcademicYear(academicYearId);
        } catch (err: any) {
            error.value = err.response?.data?.message || 'Error loading special days';
        } finally {
            loading.value = false;
        }
    }

    async function create(dto: CreateSpecialDayDto) {
        loading.value = true;
        error.value = null;
        try {
            const newItem = await SpecialDayService.create(dto);
            specialDays.value.push(newItem);
            return newItem;
        } catch (err: any) {
            error.value = err.response?.data?.message || 'Error creating special day';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    async function update(id: number, dto: UpdateSpecialDayDto) {
        loading.value = true;
        error.value = null;
        try {
            const updatedItem = await SpecialDayService.update(id, dto);
            const index = specialDays.value.findIndex(x => x.id === id);
            if (index !== -1) {
                specialDays.value[index] = updatedItem;
            }
            return updatedItem;
        } catch (err: any) {
            error.value = err.response?.data?.message || 'Error updating special day';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    async function remove(id: number) {
        loading.value = true;
        error.value = null;
        try {
            await SpecialDayService.delete(id);
            specialDays.value = specialDays.value.filter(x => x.id !== id);
        } catch (err: any) {
            error.value = err.response?.data?.message || 'Error deleting special day';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    return {
        specialDays,
        specialDayGroups,
        loading,
        error,
        loadByAcademicYear,
        create,
        update,
        remove
    };
});
