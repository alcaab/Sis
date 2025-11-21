import { ref } from "vue";
import { defineStore } from "pinia";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
import GtinArticleService from "@/services/gtinArticleService";
import type { ArticleDisplayDto } from "@models";
import type { Notifications } from "@/services/notificationService";

export const useGtinArticleStore = defineStore("gtinArticle", () => {
  const items = ref<ArticleDisplayDto[]>([]);
  const totalItems = ref(0);
  const loading = ref(false);
  const availableArticles = ref<ArticleDisplayDto[]>([]);

  async function loadArticlesList(requestParams: RequestParamsPayload, notify: Notifications) 
  {
    loading.value = true;
    try {
      const response = await GtinArticleService.getArticles(requestParams);
      items.value = response.items;
      totalItems.value = response.totalItems;
    } catch {
      notify.showError("gtinArticle.loadError");
    } finally {
      loading.value = false;
    }
  }

  async function loadAvailableArticles(notify: Notifications) {
    try {
      availableArticles.value = await GtinArticleService.getAvailableArticles();
    } catch {
      notify.showError("gtinArticle.loadAvailableError");
    }
  }

  async function addArticle(article: ArticleDisplayDto, notify: Notifications) {
    try 
    {
      await GtinArticleService.updateGtinArticleVisibility(article.number, true);
      items.value.push(article);
      items.value.sort((a, b) => a.displayText!.localeCompare(b.displayText!));
      totalItems.value++;
      availableArticles.value = availableArticles.value.filter(a => a.number !== article.number);

      notify.showSuccess("gtinArticle.added");
      return true;
    } 
    catch 
    {
      notify.showError("gtinArticle.addError");
      return false;
    }
  }

  async function removeArticle(article: ArticleDisplayDto, notify: Notifications) {
    try 
    {
      await GtinArticleService.updateGtinArticleVisibility(article.number, false);

      items.value = items.value.filter(item => item.number !== article.number);
      totalItems.value--;
      availableArticles.value.push(article);
      availableArticles.value.sort((a, b) => a.displayText.localeCompare(b.displayText));

      notify.showSuccess("gtinArticle.deleted");
      return true;
    } 
    catch 
    {
      notify.showError("gtinArticle.deleteError");
      return false;
    }
  }

  return {
    items,
    totalItems,
    loading,
    availableArticles,
    loadArticlesList,
    loadAvailableArticles,
    addArticle,
    removeArticle
  };
});
