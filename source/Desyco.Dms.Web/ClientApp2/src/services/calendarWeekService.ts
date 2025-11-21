import axios from "axios";
import type { CallbackOptions } from "@/services/common/callbackOptions";
import type { CalendarWeek } from "@models";

class CalendarWeekService {
  private static readonly BASE_ROUTE = "v1/calendar-weeks";

  static async getRange(
    weeksFromThePast: number,
    weeksFromTheFuture: number,
    { onSuccess, onError }: CallbackOptions<CalendarWeek[]> = {}
  ): Promise<void>
  {
    try
    {
      const response = await axios.get<CalendarWeek[]>(
        `${this.BASE_ROUTE}?weeksFromThePast=${weeksFromThePast}&weeksFromTheFuture=${weeksFromTheFuture}`
      );
      onSuccess?.(response.data);
    }
    catch (e)
    {
      onError?.(e as Error);
    }
  }
}

export default CalendarWeekService;
