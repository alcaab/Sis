import type { QueryDescriptor } from "../models";
import { createQuery } from "./createQuery";

interface OrderByMethods {
    _key: string;
    asc(): OrderByProxy;
    desc(): OrderByProxy;
}

type OrderByProxy = {
    [key: string]: OrderByProxy;
} & OrderByMethods;

function makeOrderby(key = ""): OrderByProxy {
    if (key.startsWith("/")) {
        key = key.slice(1);
    }

    return new Proxy(
        {},
        {
            get(_, prop: string) {
                if (prop === "_key") {
                    return key;
                }
                if (prop === "asc" || prop === "desc") {
                    return () => makeOrderby(`${key} ${prop}`);
                }

                return makeOrderby(`${key}/${prop}`);
            },
        },
    ) as OrderByProxy;
}

export function createOrderby(descriptor: QueryDescriptor) {
    return (keyOrExp: string | ((builder: OrderByProxy) => OrderByProxy), order?: "asc" | "desc") => {
        // 'expr' is now strongly typed.
        let expr: OrderByProxy = typeof keyOrExp === "string" ? makeOrderby(keyOrExp) : keyOrExp(makeOrderby());

        if (order) {
            expr = expr[order]();
        }

        return createQuery({
            ...descriptor,
            // Accessing '_key' is now type-safe.
            orderby: descriptor.orderby.concat(expr._key),
        });
    };
}
