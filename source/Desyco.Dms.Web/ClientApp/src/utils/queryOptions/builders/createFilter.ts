import type { QueryDescriptor, StringOptions } from '../models';
import { createQuery } from './createQuery';
import { isUUID } from '../isUUID';

export interface IExpression {
    _get(checkParetheses?: boolean): string;
    not(): IExpression;
    and(exp: IExpression): IExpression;
    or(exp: IExpression): IExpression;
}

type FilterProperty = { _key: string; getPropName?: unknown };

type StringFilterValue = string | FilterProperty;
type EqualityFilterValue = string | number | boolean | FilterProperty | null;
type DateFilterValue = string | Date | FilterProperty;
type NumberFilterValue = number | FilterProperty;
type SameDateFilterValue = string | number | Date | FilterProperty;

interface FilterMethods {
    _key: string;

    // Date methods
    inTimeSpan(y: number, m?: number, d?: number, h?: number, mm?: number): IExpression;
    isSame(x: SameDateFilterValue, g?: 'year' | 'month' | 'day' | 'hour' | 'minute' | 'second'): IExpression;
    isAfter(d: DateFilterValue): IExpression;
    isBefore(d: DateFilterValue): IExpression;
    isAfterOrEqual(d: DateFilterValue): IExpression;
    isBeforeOrEqual(d: DateFilterValue): IExpression;

    // Array methods
    empty(): IExpression;
    notEmpty(): IExpression;
    any(exp: (arg: RecursiveFilterProxy) => IExpression): IExpression;
    all(exp: (arg: RecursiveFilterProxy) => IExpression): IExpression;

    // String methods
    isNull(): IExpression;
    notNull(): IExpression;
    contains(s: StringFilterValue, opt?: StringOptions): IExpression;
    startsWith(s: StringFilterValue, opt?: StringOptions): IExpression;
    endsWith(s: StringFilterValue, opt?: StringOptions): IExpression;
    tolower(): RecursiveFilterProxy;
    toupper(): RecursiveFilterProxy;
    length(): RecursiveFilterProxy;
    trim(): RecursiveFilterProxy;
    indexof(s: string): RecursiveFilterProxy;
    substring(n: number): RecursiveFilterProxy;
    append(s: string): RecursiveFilterProxy;
    prepend(s: string): RecursiveFilterProxy;

    // Number methods
    biggerThan(n: NumberFilterValue): IExpression;
    lessThan(n: NumberFilterValue): IExpression;
    biggerThanOrEqual(n: NumberFilterValue): IExpression;
    lessThanOrEqual(n: NumberFilterValue): IExpression;

    // Generic methods
    equals(x: EqualityFilterValue, opt?: StringOptions): IExpression;
    notEquals(x: EqualityFilterValue, opt?: StringOptions): IExpression;
    in(arr: (number | string)[]): IExpression;
}

export type RecursiveFilterProxy = {
    [key: string]: RecursiveFilterProxy;
} & FilterMethods;

export function getFuncArgs(func: (...args: unknown[]) => unknown): string[] {
    const [, , paramStr] = /(function)?(.*?)(?=[={])/.exec(func.toString()) ?? [];
    return (paramStr ?? '')
        .replace('=>', '')
        .replace('(', '')
        .replace(')', '')
        .split(',')
        .map((s) => s.trim());
}

export function dateToObject(d: Date | string) {
    if (typeof d === 'string') {
        d = new Date(d);
    }

    return {
        year: d.getFullYear(),
        month: d.getMonth(),
        day: d.getDay(),
        hour: d.getHours(),
        minute: d.getMinutes(),
        second: d.getSeconds()
    };
}

export function makeExp(exp: string): IExpression {
    function _get(checkParetheses = false) {
        if (!checkParetheses) {
            return exp;
        } else if (exp.indexOf(' or ') > -1 || exp.indexOf(' and ') > -1) {
            return `(${exp})`;
        } else {
            return exp;
        }
    }

    return {
        _get,
        not: () => makeExp(`not (${exp})`),
        and: (expression: IExpression) => makeExp(`${_get()} and ${expression._get(true)}`),
        or: (expression: IExpression) => makeExp(`${_get()} or ${expression._get(true)}`)
    };
}

function filterBuilder(key: string): FilterMethods {
    function arrFuncBuilder(method: 'any' | 'all') {
        return function (exp: (arg: RecursiveFilterProxy) => IExpression): IExpression {
            const [arg] = getFuncArgs(exp as (...args: unknown[]) => unknown);
            const builder = exp(makeFilter(arg));
            const expr = builder._get();
            return makeExp(`${key}/${method}(${arg}: ${expr})`);
        };
    }

    function strFuncBuilder(method: 'contains' | 'startswith' | 'endswith') {
        return function (s: StringFilterValue, opt?: StringOptions): IExpression {
            if (opt?.caseInsensitive) {
                return makeExp(`${method}(tolower(${key}), ${typeof s == 'string' ? `'${s.toLocaleLowerCase()}'` : `tolower(${(s as FilterProperty)._key})`})`);
            } else if ((s as FilterProperty).getPropName) {
                return makeExp(`${method}(${key}, ${(s as FilterProperty)._key})`);
            } else {
                return makeExp(`${method}(${key}, ${typeof s == 'string' ? `'${s}'` : (s as FilterProperty)._key})`);
            }
        };
    }

    function equalityBuilder(t: 'eq' | 'ne') {
        return function (x: EqualityFilterValue, opt?: StringOptions): IExpression {
            switch (typeof x) {
                case 'string':
                    if (isUUID(x) && !opt?.ignoreGuid) {
                        return makeExp(`${key} ${t} ${x}`); // no quote around ${x}
                    } else if (opt?.caseInsensitive) {
                        return makeExp(`tolower(${key}) ${t} '${x.toLocaleLowerCase()}'`);
                    } else {
                        return makeExp(`${key} ${t} '${x}'`);
                    }

                case 'number':
                    return makeExp(`${key} ${t} ${x}`);

                case 'boolean':
                    return makeExp(`${key} ${t} ${x}`);

                default:
                    if (x && opt?.caseInsensitive) {
                        return makeExp(`tolower(${key}) ${t} tolower(${(x as FilterProperty)._key})`);
                    } else {
                        return makeExp(`${key} ${t} ${(x as FilterProperty)?._key || null}`);
                    }
            }
        };
    }

    function dateComparison(compare: 'ge' | 'gt' | 'le' | 'lt') {
        return function (d: DateFilterValue): IExpression {
            if (typeof d === 'string') return makeExp(`${key} ${compare} ${d}`);
            else if (d instanceof Date) return makeExp(`${key} ${compare} ${d.toISOString()}`);
            else return makeExp(`${key} ${compare} ${d._key}`);
        };
    }

    function numberComparison(compare: 'ge' | 'gt' | 'le' | 'lt') {
        return function (n: NumberFilterValue): IExpression {
            return makeExp(`${key} ${compare} ${typeof n == 'number' ? n : n._key}`);
        };
    }

    return {
        _key: key,

        inTimeSpan: (y: number, m?: number, d?: number, h?: number, mm?: number) => {
            const exps = [`year(${key}) eq ${y}`];
            if (m != undefined) exps.push(`month(${key}) eq ${m}`);
            if (d != undefined) exps.push(`day(${key}) eq ${d}`);
            if (h != undefined) exps.push(`hour(${key}) eq ${h}`);
            if (mm != undefined) exps.push(`minute(${key}) eq ${mm}`);
            return makeExp('(' + exps.join(') and (') + ')');
        },

        isSame: (x: SameDateFilterValue, g?: 'year' | 'month' | 'day' | 'hour' | 'minute' | 'second') => {
            if (typeof x === 'string') {
                return makeExp(`${key} eq ${x}`);
            } else if (typeof x === 'number') {
                return makeExp(`${g}(${key}) eq ${x}`);
            } else if (x instanceof Date) {
                if (g == null) {
                    return makeExp(`${key} eq ${x.toISOString()}`);
                } else {
                    const o = dateToObject(x);
                    return makeExp(`${g}(${key}) eq ${o[g]}`);
                }
            } else {
                return makeExp(`${g}(${key}) eq ${g}(${x._key})`);
            }
        },

        isAfter: dateComparison('gt'),
        isBefore: dateComparison('lt'),
        isAfterOrEqual: dateComparison('ge'),
        isBeforeOrEqual: dateComparison('le'),

        // FilterBuilderArray
        empty: () => makeExp(`not ${key}/any()`),
        notEmpty: () => makeExp(`${key}/any()`),
        any: arrFuncBuilder('any'),
        all: arrFuncBuilder('all'),

        // FilterBuilderString
        isNull: () => makeExp(`${key} eq null`),
        notNull: () => makeExp(`${key} ne null`),
        contains: strFuncBuilder('contains'),
        startsWith: strFuncBuilder('startswith'),
        endsWith: strFuncBuilder('endswith'),
        tolower: () => makeFilter(`tolower(${key})`),
        toupper: () => makeFilter(`toupper(${key})`),
        length: () => makeFilter(`length(${key})`),
        trim: () => makeFilter(`trim(${key})`),
        indexof: (s: string) => makeFilter(`indexof(${key}, '${s}')`),
        substring: (n: number) => makeFilter(`substring(${key}, ${n})`),
        append: (s: string) => makeFilter(`concat(${key}, '${s}')`),
        prepend: (s: string) => makeFilter(`concat('${s}', ${key})`),

        // FilterBuilderNumber
        biggerThan: numberComparison('gt'),
        lessThan: numberComparison('lt'),
        biggerThanOrEqual: numberComparison('ge'),
        lessThanOrEqual: numberComparison('le'),

        // FilterBuilder Generic Methods
        equals: equalityBuilder('eq'),
        notEquals: equalityBuilder('ne'),

        in(arr: (number | string)[]) {
            const list = arr.map((x) => (typeof x === 'string' ? `'${x}'` : x)).join(',');
            return makeExp(`${key} in (${list})`);
        }
    };
}

function makeFilter(prefix = ''): RecursiveFilterProxy {
    return new Proxy(
        {},
        {
            get(_, prop) {
                const methods: FilterMethods = filterBuilder(prefix);
                const key = prefix ? `${prefix}/${String(prop)}` : String(prop);
                // eslint-disable-next-line @typescript-eslint/ban-ts-comment
                // @ts-ignore
                return methods?.[prop] ? methods[prop] : makeFilter(String(key));
            }
        }
    ) as RecursiveFilterProxy;
}

export function createFilter(descriptor: QueryDescriptor) {
    return (keyOrExp: string | ((filter: RecursiveFilterProxy) => IExpression), exp?: (builder: FilterMethods) => IExpression) => {
        const expr = typeof keyOrExp === 'string' && exp ? exp(filterBuilder(keyOrExp)) : (keyOrExp as (filter: RecursiveFilterProxy) => IExpression)(makeFilter());

        return createQuery({
            ...descriptor,
            filters: descriptor.filters.concat(expr._get())
        });
    };
}
