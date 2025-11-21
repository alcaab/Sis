import { defineStore } from "pinia";
import { ref } from "vue";
import CustomGroupService from "@/services/customGroupService";
import type { CustomGroupArticleDto, CustomGroupDto, CustomGroupSubGroupDto } from "@models";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";

const newGuid: string = '00000000-0000-0000-0000-000000000000';

export const useCustomGroupStore = defineStore("customGroup", () => {
  const items = ref<CustomGroupDto[]>([]);
  const totalItems = ref(0);
  const loading = ref(false);
  const selectedItem = ref<CustomGroupDto>({ subGroups: [], articles: [] } as CustomGroupDto);
  const availableSubGroups = ref<CustomGroupSubGroupDto[]>([]);
  const availableArticles = ref<CustomGroupArticleDto[]>([]);
  
  async function loadCustomGroups(requestParams: RequestParamsPayload) {
    loading.value = true;
    const response = await CustomGroupService.getGroups(requestParams);
    items.value = response.items;
    totalItems.value = response.totalItems;
    loading.value = false;
  }

  async function loadGroup(groupId: string | undefined, mainGroupNumber: number) {
    selectedItem.value = groupId
      ? await CustomGroupService.getGroupById(groupId)
      : { id: groupId, mainGroupNumber, subGroups: [], articles: [] };
  }

  async function loadAvailableSubGroups(groupId: string | undefined, mainGroupNumber?: number) {
    const id = groupId ?? newGuid;
    const subGroups = await CustomGroupService.getAvailableSubGroups(id, mainGroupNumber);
    availableSubGroups.value = unifySubGroups(selectedItem.value.subGroups, subGroups);
  }

  async function loadAvailableArticles(groupId: string | undefined, mainGroupNumber?: number) {
    const id = groupId ?? newGuid;
    availableArticles.value = await CustomGroupService.getAvailableArticles(id, mainGroupNumber);
  }

  async function addArticle(article: CustomGroupArticleDto) {
    selectedItem.value = {
      ...selectedItem.value,
      articles: sortArticleList([...selectedItem.value.articles, article]),
    };
    
    availableArticles.value = availableArticles.value.filter(
      (e) => !(e.articleNumber === article.articleNumber && e.salesVariant === article.salesVariant)
    );
  }

  async function removeArticle(article: CustomGroupArticleDto) {
    availableArticles.value = sortArticleList([...availableArticles.value, article]);
    selectedItem.value = {
      ...selectedItem.value,
      articles: selectedItem.value.articles.filter(
        (e) => !(e.articleNumber === article.articleNumber && e.salesVariant === article.salesVariant)
      ),
    };
  }

  function unifySubGroups(a: CustomGroupSubGroupDto[], b: CustomGroupSubGroupDto[]): CustomGroupSubGroupDto[] {
    const ids = new Set(a.map(item => item.subGroupNumber));
    const result = [...a];
    for (const item of b) {
      if (!ids.has(item.subGroupNumber)) result.push(item);
    }
    
    return result;
  }

  function sortArticleList(list: CustomGroupArticleDto[]): CustomGroupArticleDto[] {
    return list.sort((a, b) => a.articleNumber.toString().localeCompare(b.articleNumber.toString()));
  }
  
  return {
    items,
    totalItems,
    loading,
    selectedItem,
    availableSubGroups,
    availableArticles,
    loadCustomGroups,
    loadGroup,
    loadAvailableSubGroups,
    loadAvailableArticles,
    addArticle,
    removeArticle,
  };
});
