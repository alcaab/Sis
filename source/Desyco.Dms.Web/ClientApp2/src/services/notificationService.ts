import { useToast } from 'primevue/usetoast';
import { useI18n } from 'vue-i18n';
import { ToastLife } from "@/models/common/toastLife";

export function useNotifications() {
  const toast = useToast();
  const { t } = useI18n();

  function showSuccess(detail: string, summary?: string){
    toast.add({
      severity: 'success',
      summary: summary || t('common.successfullySaved'),
      detail : t(detail),
      life: ToastLife.success,
    });
  }

  function showError(detail: string, summary?: string){
    toast.add({
      severity: 'error',
      summary: summary || t('common.errorTitle'),
      detail: t(detail),
      life: ToastLife.error,
    });
  }

  return {
    showSuccess,
    showError
  };
}

export type Notifications = ReturnType<typeof useNotifications>;
