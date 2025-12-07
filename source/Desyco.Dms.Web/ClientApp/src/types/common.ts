export interface QueryResult<T> {
    count?: number;
    results: T[];
}
