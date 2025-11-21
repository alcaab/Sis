export interface CustomGroupSubGroupDto {
  id: string;
  parentId?: string;
  subGroupNumber: number;
  name: string;
}

export interface CustomGroupArticleDto {
  id: string;
  parentId?: string;
  articleNumber: number;
  salesVariant: number;
  name: string;
  displayName: string;
}

export interface CustomGroupDto {
  id?: string;
  customGroupNumber?: number;
  mainGroupNumber?: number;
  name?: string;
  subGroups: CustomGroupSubGroupDto[];
  articles: CustomGroupArticleDto[];
  subGroupNames?: string[];
  articleNames?: number[] | string;
}
