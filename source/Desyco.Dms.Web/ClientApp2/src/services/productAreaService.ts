import type { CallbackOptions } from "@/services/common/callbackOptions";
import axios from "axios";
import type { ProductArea } from "@/models/productArea/productArea";

class ProductAreaService {
  private static readonly BASE_ROUTE = "v1/product-areas";
  static async getProductAreas(
    { onSuccess, onError }: CallbackOptions<ProductArea[]> = {}
  ): Promise<void>
  {
    try
    {
      const response = await axios.get<ProductArea[]>(
        `${this.BASE_ROUTE}`
      );
      onSuccess?.(response.data);
    }
    catch (e)
    {
      onError?.(e as Error);
    }
  }
}

export default ProductAreaService;
