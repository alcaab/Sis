export interface Filter {
  value?: string | number | boolean | Date | undefined | null | number[];
  matchMode?: string;
  type?: 'number' | 'string' | 'boolean' | 'date';
}

export type Filters = { [s: string]: Filter | Filter[] };

export type Sort = {
  field: string;
  order?: 'asc' | 'desc';
};

export interface Paginator {
  skip?: number;
  take?: number;
}

export interface RequestParamsPayload {
  skip?: number;
  take?: number;
  showCount?: boolean,
  paginator?: Paginator;
  filters?: Filters;
  search?: string;
  sorts?: Sort[] | string;
  extras?: { [s: string]: any };
}
