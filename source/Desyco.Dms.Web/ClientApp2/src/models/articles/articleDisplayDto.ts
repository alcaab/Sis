import type { StoreDto } from "@models";
import type { QueryResult } from "@/models/common/paginatedResult";

export interface ArticleDisplayDto {
  number: number;
  text: string[];
  image: string | null;
  displayText?: string;
}

export interface CalendarWeek {
  year: number;
  week: number;
  calendarWeekStart: string;
  calendarWeekEnd: string;
  isCurrentWeek: boolean;
  displayValue: string;
}

export interface ArticleFilters {
  stores: StoreDto[];
  weeks: CalendarWeek[];
}

export interface ArticlePreviewDisplay {
  articles : ArticleDisplayDto[];
  page: PaginationData;
  TotalItems: number;
  columns: number;
  results : QueryResult<ArticleDisplayDto>,
  rows: number;
}
