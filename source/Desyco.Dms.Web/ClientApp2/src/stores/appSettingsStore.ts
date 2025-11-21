import { ref } from 'vue';
import { defineStore } from 'pinia';
import type { AppSettingsDto } from "@models";
import SettingsService from "@/services/settingsService";
import type { Notifications } from "@/services/notificationService";

export const useAppSettingsStore = defineStore("appSettings", () => {
  const settings = ref<AppSettingsDto>({} as AppSettingsDto);
  const isLoading = ref(false);
  const exportPathXml = ref('');
  const exportPathImages = ref('');
  const exportWeekDays = ref(0);
  const exportTime = ref('');
  const emailWeekDays = ref(0);
  const emailTime = ref('');
  const isWritableXml = ref<boolean | null>(null);
  const isWritableImages = ref<boolean | null>(null);

  async function fetchAppSettingsData(notify: Notifications) {
    isLoading.value = true;
    try {
      await SettingsService.get({
        onSuccess: (data: AppSettingsDto) => {
          settings.value = { ...data };
          exportPathXml.value = data.exportPathXml;
          exportPathImages.value = data.exportPathImages;
          exportWeekDays.value = data.exportWeekDays;
          exportTime.value = data.exportTime;
          emailWeekDays.value = data.emailWeekDays;
          emailTime.value = data.emailTime;
        },
        onError: () => {
          notify.showError('appSettings.loadError');
        },
      });
    } finally {
      isLoading.value = false;
    }
  }

  async function saveSettings(notify: Notifications): Promise<boolean> {
    isLoading.value = true;
    let success = false;
    try {
      await SettingsService.update(
        {
          exportPathXml: exportPathXml.value,
          exportPathImages: exportPathImages.value,
          exportWeekDays: exportWeekDays.value,
          exportTime: exportTime.value,
          emailWeekDays: emailWeekDays.value,
          emailTime: emailTime.value,
        },
        {
          onSuccess: (updated: AppSettingsDto) => {
            settings.value = { ...updated };
            isWritableXml.value = true;
            isWritableImages.value = true;
            notify.showSuccess('appSettings.saved');
            success = true;
          },
          onError: (e: Error) => {
            const data = e.response?.data?.validationErrors;
            if (data) {
              isWritableXml.value = !data.XmlPath;
              isWritableImages.value = !data.ImagesPath;
            }

            if (!isWritableXml.value || !isWritableImages.value) {
              notify.showError('appSettings.exportPath.pathNotWritable');
            }
          },
        }
      );
    } finally {
      isLoading.value = false;
    }
    return success;
  }

  return {
    settings,
    isLoading,
    exportPathXml,
    exportPathImages,
    exportWeekDays,
    exportTime,
    emailWeekDays: emailWeekDays,
    emailTime,
    isWritableXml,
    isWritableImages,
    fetchAppSettingsData,
    saveSettings,
  };
});
