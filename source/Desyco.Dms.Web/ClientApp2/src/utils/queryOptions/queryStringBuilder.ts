import type { Filter, Filters, RequestParamsPayload, Sort } from "@/utils/queryOptions/queryOptionModels";
import queryOptionsFilter, { type queryOptions } from "@/utils/queryOptions/index";

export const operatorsFuncMap: Record<string, string | Record<string, string>> = {
  eq: 'equals',
  ne: 'notEquals',
  gt: { number: 'biggerThan', date: 'isAfter' },
  ge: { number: 'biggerThanOrEqual', date: 'isAfterOrEqual' },
  lt: { number: 'lessThan', date: 'isBefore' },
  le: { number: 'lessThanOrEqual', date: 'isBeforeOrEqual' },
  in :"in",
  contains: 'contains',
  startswith: 'startsWith',
  endswith: 'endsWith'
};

function queryStringBuilder(requestParams: RequestParamsPayload) : string{
  let builder = queryOptionsFilter<any>();
  
  if (requestParams.take && requestParams.take > 0) {
    builder = builder.paginate(requestParams.take, requestParams.skip);
  }

  const filters = requestParams.filters ?? {};

  if (Object.keys(filters).length > 0) builder = createFilters(builder, filters);

  if (requestParams?.search) {
    builder = builder.search(requestParams.search);
  }

  const sorts = requestParams.sorts ?? [];

  if (sorts.length > 0) {
    builder = createSorts(builder, sorts as Sort[]);
  }

  const result = builder.toString();

  if (!result) return '';

  return '?' + result;
}

function buildQueryOptionsString(requestParams?: RequestParamsPayload): string{
  let query = '';

  if (!requestParams) 
     return query;

  query = queryStringBuilder(requestParams);

  if (requestParams.extras) {
    const extras = new URLSearchParams(requestParams.extras).toString();
    query = query ? `${query}&${extras}` : `?${extras}`;
  }

  return query;
}

function createFilters<T>(builder: queryOptions<T>, filters: Filters): queryOptions<T> {
  
  for (const [field, filter] of Object.entries(filters)) {
    if (field === 'global') continue;

    if (Array.isArray(filter)) {
      filter.forEach((f: Filter) => {
        builder = applyFilter(builder, createSupportedNestedProp(field) as string, f);
      });
      continue;
    }

    builder = applyFilter(builder, createSupportedNestedProp(field) as string, filter as Filter);
  }

  return builder;
}

function createSorts<T>(builder: queryOptions<T>, sorts: Sort[]): queryOptions<T> {
  sorts.forEach((sort: Sort) => {
    builder = builder.orderBy(createSupportedNestedProp(sort.field), sort.order);
  });

  return builder;
}

function applyFilter<T>(builder: queryOptions<T>, field: string, filter: Filter): queryOptions<T> {
  const operator = filter.matchMode ?? 'eq';
  let methodName = operatorsFuncMap[operator];

  if(typeof methodName === 'object') {
    const type = getFilterType(filter);
    methodName = methodName[type];
  }
  
  return builder.filter((e) => e[field][methodName](filter.value));
}

function createSupportedNestedProp<T>(property: string): keyof T{
  if (!property || !property.includes('.')) 
     return property as keyof T;
  return property.replace(/\./g, '/') as keyof T;
}

function getFilterType(filter: Filter): string{
  const { type, value } = filter;
  
  if (type && ["number", "string", "boolean", "date"].includes(type)) {
    return type;
  }

  switch (typeof value) {
    case "string":
      return isValidDateString(value) ? "date" : "string";
    case "bigint":
    case "number":
      return "number";
    case "boolean":
      return "boolean";
    default:
      Error('unsupported type "' + type + '"');
  }
}

function isValidDateString(value: string): boolean{
  return !isNaN(new Date(value).getTime());
}

export const QueryStringBuilder = {
  buildQueryOptionsString
};
