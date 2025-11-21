import axios from "axios";
import type { CallbackOptions } from "@/services/common/CallbackOptions";
import type { AppSettingsDto } from "@models";

class SettingsService {
  private static readonly BASE_ROUTE = "v1/settings";
  
  static async get(
    { onSuccess, onError }: CallbackOptions<AppSettingsDto> = {}
  ): Promise<void> {
    try {
      const response = await axios.get<AppSettingsDto>(this.BASE_ROUTE);
      onSuccess?.(response.data);
    } catch (e) {
      onError?.(e as Error);
    }
  }
  
  static async update(
    settingsDto: AppSettingsDto,
    { onSuccess, onError }: CallbackOptions<AppSettingsDto> = {}
  ): Promise<void> {
    try {
      const response = await axios.put<AppSettingsDto>(this.BASE_ROUTE, settingsDto);
      onSuccess?.(response.data);
    } catch (e) {
      onError?.(e as Error);
    }
  }
}

export default SettingsService;
