export interface PaginatedResult<TData> {
  data: TData[];
  totalRecords: number;
}

export interface QueryResult<T> {
  items: T[];
  totalItems: number;
}
