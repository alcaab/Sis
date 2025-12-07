import type { FilterBuilder, FilterBuilderProp, FilterExpression } from './queryFilter';
import type { OrderBy, OrderByBuilder, OrderByExpression } from './queryOrderby';

export interface queryOptions<T> {
    count(): queryOptions<T>;

    filter(exp: (x: FilterBuilder<T>) => FilterExpression): queryOptions<T>;

    filter<TKey extends keyof T>(key: TKey, exp: (x: FilterBuilderProp<T[TKey]>) => FilterExpression): queryOptions<T>;

    orderBy<TKey extends keyof T>(key: TKey, order?: 'asc' | 'desc'): queryOptions<T>;

    orderBy(exp: (ob: OrderByBuilder<T>) => OrderBy | OrderByExpression): queryOptions<T>;

    paginate(pagesize: number, page?: number): queryOptions<T>;

    paginate(options: { pagesize: number; page?: number; count?: boolean }): queryOptions<T>;

    search(value: string): queryOptions<T>;

    toObject(): QueryObject;

    toString(): string;
}

export type QueryObject = {
    $apply?: string;
    $count?: string;
    $expand?: string;
    $filter?: string;
    $orderby?: string;
    $select?: string;
    $skip?: string;
    $top?: string;
};
