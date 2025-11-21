
import axios from "axios";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
import { QueryStringBuilder } from "@/utils/queryOptions/queryStringBuilder";
import type { QueryResult } from "@/models/common/paginatedResult";
import type { ArticleDisplayDto } from "@models";

class GtinArticleService 
{
  private static readonly BASE_ROUTE = "v1/gtin-article";

  static async getArticles(requestParams?: RequestParamsPayload): Promise<QueryResult<ArticleDisplayDto>> 
  {
    const queryString = QueryStringBuilder.buildQueryOptionsString(requestParams);
    const response = await axios.get(`${this.BASE_ROUTE}${queryString}`);

    return response.data;
  }

  static async getAvailableArticles(): Promise<ArticleDisplayDto[]> 
  {
    const response = await axios.get(`${this.BASE_ROUTE}/available-articles`);

    return response.data;
  }

  static async updateGtinArticleVisibility(articleNumber: number, isVisible: boolean): Promise<void> 
  {
    await axios.put(`${this.BASE_ROUTE}/${articleNumber}?visible=${isVisible}`);
  }
}

export default GtinArticleService;
