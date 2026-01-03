import api from "./api";
import  { SpecialDayDto, CreateSpecialDayDto, UpdateSpecialDayDto, SpecialDayEducationalLevelDto } from '@/types/special-days.ts';
import type { QueryResult } from '@/types/common.ts';

const BASE_URL = 'special-days';

export default {
    async getAll(params?: any): Promise<QueryResult<SpecialDayDto>> {
        const response = await api.get(BASE_URL, { params });
        return response.data;
    },

    async getById(id: number): Promise<SpecialDayDto> {
        const response = await api.get(`${BASE_URL}/${id}`);
        return response.data;
    },

    async getByPeriod(periodId: number): Promise<SpecialDayDto[]> {
        const response = await api.get(`${BASE_URL}/by-period/${periodId}`);
        return response.data;
    },

    async getByAcademicYear(academicYearId: number): Promise<SpecialDayEducationalLevelDto[]> {
        const response = await api.get(`${BASE_URL}/by-year/${academicYearId}`);
        return response.data;
    },

    async create(dto: CreateSpecialDayDto): Promise<SpecialDayDto> {
        const response = await api.post(BASE_URL, dto);
        return response.data;
    },

    async update(id: number, dto: UpdateSpecialDayDto): Promise<SpecialDayDto> {
        const response = await api.put(`${BASE_URL}/${id}`, dto);
        return response.data;
    },

    async delete(id: number): Promise<void> {
        await api.delete(`${BASE_URL}/${id}`);
    },
};
