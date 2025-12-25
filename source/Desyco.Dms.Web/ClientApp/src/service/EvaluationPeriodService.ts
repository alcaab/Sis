import api from "./api";
import type { EvaluationPeriodDto } from "@/types/evaluation-period";
import type { QueryResult } from "@/types/common";
import type { AxiosResponse } from "axios";
import { QueryStringBuilder } from "@/utils/queryOptions/queryStringBuilder";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";

const BASE_ROUTE = "/evaluation-periods";

export const EvaluationPeriodService = {
    getEvaluationPeriods(requestParams?: RequestParamsPayload): Promise<AxiosResponse<QueryResult<EvaluationPeriodDto>>> {
        const queryString = QueryStringBuilder.buildQueryOptionsString(requestParams);

        return api.get<QueryResult<EvaluationPeriodDto>>(`${BASE_ROUTE}${queryString}`);
    },

    getEvaluationPeriod(id: number): Promise<AxiosResponse<EvaluationPeriodDto>> {
        return api.get<EvaluationPeriodDto>(`${BASE_ROUTE}/${id}`);
    },

    createEvaluationPeriod(data: EvaluationPeriodDto): Promise<AxiosResponse<EvaluationPeriodDto>> {
        return api.post<EvaluationPeriodDto>(BASE_ROUTE, data);
    },

    updateEvaluationPeriod(id: number, data: EvaluationPeriodDto): Promise<AxiosResponse<EvaluationPeriodDto>> {
        return api.put<EvaluationPeriodDto>(`${BASE_ROUTE}/${id}`, data);
    },

    deleteEvaluationPeriod(id: number): Promise<AxiosResponse<void>> {
        return api.delete<void>(`${BASE_ROUTE}/${id}`);
    },
};
