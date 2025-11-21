import axios from 'axios';
import type { CustomGroupArticleDto, CustomGroupDto, CustomGroupSubGroupDto } from "@models";
import { QueryStringBuilder } from "@/utils/queryOptions/queryStringBuilder";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
import type { QueryResult } from "@/models/common/paginatedResult";

class CustomGroupService {
  private static readonly BASE_ROUTE = '/v1/custom-groups';

  static async getGroups(requestParams?: RequestParamsPayload): Promise<QueryResult<CustomGroupDto>> {
    const queryString = QueryStringBuilder.buildQueryOptionsString(requestParams);
    const response = await axios.get(`${this.BASE_ROUTE}${queryString}`);

    return response.data;
  }

  static async getGroupById(groupId: string | undefined): Promise<CustomGroupDto> {
    const response = await axios.get(`${this.BASE_ROUTE}/${groupId}`);

    return response.data;
  }
  
  static async getAvailableSubGroups(groupId?: string, mainGroupNumber?: number): Promise<CustomGroupSubGroupDto[]> {
    const response = await axios.get(`${this.BASE_ROUTE}/${groupId}/available-subgroups?mainGroupNumber=${mainGroupNumber}`);

    return response.data;
  }

  static async getAvailableArticles(groupId?: string, mainGroupNumber?: number): Promise<CustomGroupArticleDto[]> {
    const response = await axios.get(`${this.BASE_ROUTE}/${groupId}/available-articles?mainGroupNumber=${mainGroupNumber}`);

    return response.data;
  }

  static async createGroup(group: Omit<CustomGroupDto, 'id' | 'subGroups'>): Promise<CustomGroupDto> {
    const response = await axios.post(this.BASE_ROUTE, group);

    return response.data;
  }

  static async updateGroup(groupId: string, group: CustomGroupDto): Promise<CustomGroupDto> {
    const response = await axios.put(`${this.BASE_ROUTE}/${groupId}`, group);

    return response.data;
  }

  static async deleteGroup(groupId: string | undefined): Promise<void> {
    await axios.delete(`${this.BASE_ROUTE}/${groupId}`);
  }
}

export default CustomGroupService;
