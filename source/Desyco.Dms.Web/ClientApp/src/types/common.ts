export interface QueryResult<T> {
    count?: number;
    results: T[];
}

export interface LookupModel {
    id: number | string;
    value: string;
    description: string;
}
