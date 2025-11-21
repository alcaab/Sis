import type { FilterData } from "@models";

function getUriString(filter: FilterData): string {
  const params: string[] = [];

  for (const [key, value] of filter.filters) {
    if (value !== null && value !== undefined && value !== "") {
      params.push(
        `filters[${encodeURIComponent(key)}]=${encodeURIComponent(value)}`
      );
    }
  }

  return params.join("&");
}

export const FilteringUtils = {
  getUriString,
};
