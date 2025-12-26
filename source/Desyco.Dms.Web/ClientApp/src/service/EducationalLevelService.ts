import api from "./api";
import type { EducationalLevelDto } from "@/types/educational-level";
import type { QueryResult } from "@/types/common";
import type { AxiosResponse } from "axios";
import { QueryStringBuilder } from "@/utils/queryOptions/queryStringBuilder";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";

const BASE_ROUTE = "/educational-levels";

export const EducationalLevelService = {
    getEducationalLevels(
        requestParams?: RequestParamsPayload,
    ): Promise<AxiosResponse<QueryResult<EducationalLevelDto>>> {
        const queryString = QueryStringBuilder.buildQueryOptionsString(requestParams);

        return api.get<QueryResult<EducationalLevelDto>>(`${BASE_ROUTE}${queryString}`);
    },

    getEducationalLevel(id: number): Promise<AxiosResponse<EducationalLevelDto>> {
        return api.get<EducationalLevelDto>(`${BASE_ROUTE}/${id}`);
    },

    createEducationalLevel(data: EducationalLevelDto): Promise<AxiosResponse<EducationalLevelDto>> {
        return api.post<EducationalLevelDto>(BASE_ROUTE, data);
    },

    updateEducationalLevel(id: number, data: EducationalLevelDto): Promise<AxiosResponse<EducationalLevelDto>> {
        return api.put<EducationalLevelDto>(`${BASE_ROUTE}/${id}`, data);
    },

    deleteEducationalLevel(id: number): Promise<AxiosResponse<void>> {
        return api.delete<void>(`${BASE_ROUTE}/${id}`);
    },
};
