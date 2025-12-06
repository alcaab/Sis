import api from './api';
import type { AcademicYearDto, CreateAcademicYearDto, UpdateAcademicYearDto } from '@/types/academic-year';
import type { QueryResult } from '@/types/common';
import type { AxiosResponse } from 'axios';

export const AcademicYearService = {
    getAcademicYears(params?: any): Promise<AxiosResponse<QueryResult<AcademicYearDto>>> {
        return api.get<QueryResult<AcademicYearDto>>('/academic-years', { params });
    },

    getAcademicYear(id: number): Promise<AxiosResponse<AcademicYearDto>> {
        return api.get<AcademicYearDto>(`/academic-years/${id}`);
    },

    createAcademicYear(academicYear: CreateAcademicYearDto): Promise<AxiosResponse<AcademicYearDto>> {
        return api.post<AcademicYearDto>('/academic-years', academicYear);
    },

    updateAcademicYear(id: number, academicYear: UpdateAcademicYearDto): Promise<AxiosResponse<AcademicYearDto>> {
        return api.put<AcademicYearDto>(`/academic-years/${id}`, academicYear);
    },

    deleteAcademicYear(id: number): Promise<AxiosResponse<void>> {
        return api.delete<void>(`/academic-years/${id}`);
    }
};