import type { QueryDescriptor } from "../models";
import { createQuery } from "./createQuery";

interface PaginationOptions {
    page: number;
    pagesize: number;
    count?: boolean;
}

export function createPaginate(descriptor: QueryDescriptor) {
    function paginate(pagesize: number, page: number): ReturnType<typeof createQuery>;
    function paginate(options: PaginationOptions): ReturnType<typeof createQuery>;
    function paginate(sizeOrOptions: number | PaginationOptions, page?: number): ReturnType<typeof createQuery> {
        let data: PaginationOptions;

        if (typeof sizeOrOptions === "number") {
            data = {
                page: page!,
                pagesize: sizeOrOptions,
                count: true,
            };
        } else {
            data = sizeOrOptions;
            if (data.count === undefined) {
                data.count = true;
            }
        }

        const queryDescriptor: QueryDescriptor = {
            ...descriptor,
            take: data.pagesize,
            skip: data.page,
            count: data.count,
        };

        if (!queryDescriptor.skip) {
            queryDescriptor.skip = undefined;
        }
        return createQuery(queryDescriptor);
    }

    return paginate;
}
