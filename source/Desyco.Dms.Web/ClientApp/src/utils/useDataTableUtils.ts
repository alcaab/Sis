import type { Filter, Filters, RequestParamsPayload, Sort } from "@/utils/queryOptions/queryOptionModels";
import type { DataTableSortEvent } from "primevue/datatable";
import { unref } from "vue";

function convertToPrimeVueSort({ sortField, sortOrder }: DataTableSortEvent): Sort[] {
    if (!sortField) return [];
    function direction(order: number) {
        return order === -1 ? "asc" : "desc";
    }
    return [{ field: sortField as string, order: direction(sortOrder ?? 0) }];
}

function convertToPrimeVueFilters({ filters }: DataTableSortEvent): Filters {
    const filterObject: Filters = {};

    if (!filters) return filterObject;

    for (const [key, filter] of Object.entries(unref(filters) as Filter)) {
        filterObject[unref(key)] = { ...unref(filter) };
    }

    return filterObject;
}

function transformLazyLoadParams(event: DataTableSortEvent): RequestParamsPayload {
    const sorts = convertToPrimeVueSort(event);
    const filters: any = event.filters;
    return {
        skip: event.first,
        take: event.rows,
        sorts,
        filters,
        search: filters["global"] ? filters["global"].value : null,
    };
}

const defaultPageSize = 10;

export function useLazyLoadParams(initialArgs?: DataTableSortEvent) {
    let args: DataTableSortEvent = initialArgs ?? ({ first: 0, rows: defaultPageSize } as DataTableSortEvent);

    function updateArgs(newArgs: DataTableSortEvent | undefined) {
        args = { ...args, ...newArgs };
    }

    return {
        get args() {
            return args;
        },
        updateArgs,
    };
}

export function useDataTableUtils(callback: (event: RequestParamsPayload) => void) {
    const lazyLoadEvent = useLazyLoadParams();

    function paginate(event?: DataTableSortEvent) {
        callback(transformLazyLoadParams(event ?? lazyLoadEvent.args ?? ({} as DataTableSortEvent)));
        lazyLoadEvent.updateArgs(event);
    }

    return {
        paginate,
        convertToPrimeVueFilters,
        convertToPrimeVueSort,
    };
}
