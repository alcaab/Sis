import axios from "axios";
import type { CallbackOptions } from "@/services/common/callbackOptions";
import type {
  ArticleDisplayDto,
  ArticlePreviewDisplay,
  FilterData,
  PaginationData,
  SortingData
} from "@models";
import { FilteringUtils, PaginationUtils, SortingUtils } from "@utils";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
import { QueryStringBuilder } from "@/utils/queryOptions/queryStringBuilder";

class ArticleDisplayService {
  private static readonly BASE_ROUTE = "v1/articles";

  static async getAll(
    pagination: PaginationData,
    sorting: SortingData,
    filtering: FilterData,
    { onSuccess, onError }: CallbackOptions<ArticleDisplayDto[]> = {}
  ): Promise<void> {
    try {
      const queryParams = [
        PaginationUtils.getUriString(pagination),
        SortingUtils.getUriString(sorting),
        FilteringUtils.getUriString(filtering),
      ].join("&");

      const url = `${this.BASE_ROUTE}?${queryParams}`;
      const response = await axios.get(url);
      onSuccess?.(response.data);
    } catch (e) {
      console.error(e);
      onError?.(e);
    }
  }
  
  static async getPreviewArticles(requestParams?: RequestParamsPayload): Promise<ArticlePreviewDisplay> {
    const queryString = QueryStringBuilder.buildQueryOptionsString(requestParams);
    const response = await axios.get(`${this.BASE_ROUTE}${queryString}`);
  
    return response.data;
  }
}

export default ArticleDisplayService;
