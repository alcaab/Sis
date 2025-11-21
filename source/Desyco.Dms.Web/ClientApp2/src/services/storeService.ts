import type { CallbackOptions } from "@/services/common/callbackOptions";
import type { StoreDto } from "@models";
import axios from "axios";

class StoreService {
  private static readonly BASE_ROUTE = "v1/stores";
  static async getByDisplayName(
    { onSuccess, onError }: CallbackOptions<StoreDto[]> = {}
  ): Promise<void>
  {
    try
    {
      const response = await axios.get<StoreDto[]>(
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

export default StoreService;
