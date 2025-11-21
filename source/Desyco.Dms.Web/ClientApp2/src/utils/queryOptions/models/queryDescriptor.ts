export interface QueryDescriptor {
    count?: boolean;
    key?: string;
    skip?: number;
    take?: number;
    filters: string[];
    orderby: string[];
}
