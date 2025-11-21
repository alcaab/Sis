import type { CallbackOptions } from "@/services/common/callbackOptions";
import type { ArticleImageDto } from "@models";
import axios from "axios";

class ArticleImageService {
  private static readonly BASE_ROUTE = "v1/articles";
  
  static async getByArticleNumber(
    articleNumber: number,
    { onSuccess, onError }: CallbackOptions<ArticleImageDto> = {}
  ): Promise<void> {
    if (!articleNumber) {
      return;
    }
    
    try {
      const response = await axios.get<ArticleImageDto>(
        `${this.BASE_ROUTE}/${articleNumber}/images`
      );
      onSuccess?.(response.data);
    } catch (e) {
      onError?.(e as Error);
    }
  }
}

export default ArticleImageService;
