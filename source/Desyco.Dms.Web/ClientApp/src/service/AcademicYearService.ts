import api from "./api";
import type { AcademicYearDto } from "@/types/academic-year";
import type { QueryResult } from "@/types/common";
import type { AxiosResponse } from "axios";
import { QueryStringBuilder } from "@/utils/queryOptions/queryStringBuilder";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";

const BASE_ROUTE = "/academic-years";

export const AcademicYearService = {
    getAcademicYears(requestParams?: RequestParamsPayload): Promise<AxiosResponse<QueryResult<AcademicYearDto>>> {
        const queryString = QueryStringBuilder.buildQueryOptionsString(requestParams);

        return api.get<QueryResult<AcademicYearDto>>(`${BASE_ROUTE}${queryString}`);
    },

    getAcademicYear(id: number): Promise<AxiosResponse<AcademicYearDto>> {
        return api.get<AcademicYearDto>(`${BASE_ROUTE}/${id}`);
    },

    createAcademicYear(academicYear: AcademicYearDto): Promise<AxiosResponse<AcademicYearDto>> {
        return api.post<AcademicYearDto>(BASE_ROUTE, academicYear);
    },

    updateAcademicYear(id: number, academicYear: AcademicYearDto): Promise<AxiosResponse<AcademicYearDto>> {
        return api.put<AcademicYearDto>(`${BASE_ROUTE}/${id}`, academicYear);
    },

    deleteAcademicYear(id: number): Promise<AxiosResponse<void>> {
        return api.delete<void>(`${BASE_ROUTE}/${id}`);
    },
};
