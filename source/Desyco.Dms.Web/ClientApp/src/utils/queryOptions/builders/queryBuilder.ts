import type { QueryDescriptor } from '../models';

export interface KeyValue<T> {
    key: string;
    value: T;
}

export function makeQuery(qd: QueryDescriptor): KeyValue<string>[] {
    const params: {
        key: string;
        value: string;
    }[] = [];

    if (qd.filters.length) {
        if (qd.filters.length > 1) {
            params.push({
                key: '$filter',
                value: `${qd.filters.map(makeQueryParentheses).join(' and ')}`
            });
        } else {
            params.push({
                key: '$filter',
                value: `${qd.filters.join()}`
            });
        }
    }

    if (qd.orderby.length) {
        params.push({
            key: '$orderby',
            value: `${qd.orderby.join(', ')}`
        });
    }

    if (qd.skip != null) {
        params.push({
            key: '$skip',
            value: `${qd.skip}`
        });
    }

    if (qd.take != null) {
        params.push({
            key: '$top',
            value: `${qd.take}`
        });
    }

    if (qd.count == true) {
        params.push({
            key: '$count',
            value: `true`
        });
    }

    if (qd.search != null) {
        params.push({
            key: '$search',
            value: `${qd.search}`
        });
    }

    return params;
}

export function makeQueryParentheses(query: string): string {
    if (query.indexOf(' or ') > -1 || query.indexOf(' and ') > -1) {
        return `(${query})`;
    }

    return query;
}

export function makeRelationQuery(rqd: QueryDescriptor): string {
    let expand: string = rqd.key || '';

    if (rqd.filters.length || rqd.orderby.length || rqd.skip != null || rqd.take != null || rqd.count != false) {
        expand += `(`;

        const operators = [];

        if (rqd.skip != null) {
            operators.push(`$skip=${rqd.skip}`);
        }

        if (rqd.take != null) {
            operators.push(`$top=${rqd.take}`);
        }

        if (rqd.count == true) {
            operators.push(`$count=true`);
        }

        if (rqd.orderby.length) {
            operators.push(`$orderby=${rqd.orderby.join(',')}`);
        }

        if (rqd.filters.length) {
            if (rqd.filters.length > 1) {
                operators.push(`$filter=${rqd.filters.map(makeQueryParentheses).join(' and ')}`);
            } else {
                operators.push(`$filter=${rqd.filters.join()}`);
            }
        }

        expand += operators.join(';') + ')';
    }

    return expand;
}
