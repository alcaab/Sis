import { createQuery, createQueryDescriptor } from "./builders";
import type { queryOptions } from "./models";

export function queryOptionsFilter<T>(): queryOptions<T> {
    const defaultDescriptor = createQueryDescriptor();

    return createQuery(defaultDescriptor);
}

export * from "./models";
export default queryOptionsFilter;
