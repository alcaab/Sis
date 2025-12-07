import type { QueryDescriptor, QueryObject } from '../models';
import { createFilter } from './createFilter';
import { createOrderby } from './createOrderby';
import { createPaginate } from './createPaginate';
import { makeQuery } from './queryBuilder';

type IExpression = { _get: () => string };
type FilterMethods = object;
type RecursiveFilterProxy = object;
type OrderByProxy = object;
type PaginationOptions = object;

export interface IQueryBuilder {
    _descriptor: QueryDescriptor;

    // Method signatures are derived from their respective "create" functions
    filter(key: string, exp: (builder: FilterMethods) => IExpression): IQueryBuilder;
    filter(exp: (filter: RecursiveFilterProxy) => IExpression): IQueryBuilder;

    orderBy(key: string, order?: 'asc' | 'desc'): IQueryBuilder;
    orderBy(exp: (builder: OrderByProxy) => OrderByProxy): IQueryBuilder;

    paginate(pagesize: number, page: number): IQueryBuilder;
    paginate(options: PaginationOptions): IQueryBuilder;

    count(): IQueryBuilder;
    toObject(): QueryObject;
    toString(): string;
    _fluent: true;
}

export function createQueryDescriptor(key?: string): QueryDescriptor {
    return {
        key: key,
        skip: undefined,
        take: undefined,
        count: false,
        filters: [],
        orderby: [],
        search: undefined
    };
}

export function createQuery(descriptor: QueryDescriptor): IQueryBuilder {
    return {
        _descriptor: descriptor,
        filter: createFilter(descriptor),
        orderBy: createOrderby(descriptor),
        paginate: createPaginate(descriptor),
        count() {
            return createQuery({
                ...descriptor,
                count: true
            });
        },
        search(searchTerm: string) {
            return createQuery({
                ...descriptor,
                search: searchTerm
            });
        },
        toObject(): QueryObject {
            return makeQuery(descriptor).reduce((obj, x) => {
                obj[x.key as keyof QueryObject] = x.value;
                return obj;
            }, {} as QueryObject);
        },
        toString(): string {
            return makeQuery(descriptor)
                .map((p) => `${p.key}=${p.value}`)
                .join('&');
        },
        _fluent: true
    };
}
