<script setup lang="ts">
import { useI18n } from "vue-i18n";
import { onMounted } from "vue";
import Panel from "primevue/panel";
import { useForm } from "vee-validate";
import * as yup from "yup";
import { useAppSettingsStore } from "@/stores/appSettingsStore";
import { useNotifications } from "@/services/notificationService";
import EmailWeekDaysSetting from "@/components/appSettings/EmailWeekDaysSetting.vue";
import ExportPathSetting from "@/components/appSettings/ExportPathSetting.vue";
import ExportWeekDaysHoursSetting from "@/components/appSettings/ExportWeekDaysHoursSetting.vue";

const { t } = useI18n();
const store = useAppSettingsStore();
const notify = useNotifications();

function noLeadingTrailingSpaces(value?: string){
  return value?.trim();
}

const schema = yup.object({
  exportPathXml: yup
    .string()
    .required(t("appSettings.exportPath.pathNotEmpty"))
    .max(240, t("appSettings.exportPath.pathTooLong", { max: 240 }))
    .test(
      "no-trim-spaces",
      t("appSettings.exportPath.noTrimSpaces"),
      noLeadingTrailingSpaces
    )
    .matches(
      /^([a-zA-Z]:\\|\\\\)([^<>:"/\\|?*]+\\)*[^<>:"/\\|?*]+$/,
      t("appSettings.exportPath.invalidPath")
    ),
  exportPathImages: yup
    .string()
    .required(t("appSettings.exportPath.pathNotEmpty"))
    .max(240, t("appSettings.exportPath.pathTooLong", { max: 240 }))
    .test(
      "no-trim-spaces",
      t("appSettings.exportPath.noTrimSpaces"),
      noLeadingTrailingSpaces
    )
    .matches(
      /^([a-zA-Z]:\\|\\\\)([^<>:"/\\|?*]+\\)*[^<>:"/\\|?*]+$/,
      t("appSettings.exportPath.invalidPath")
    ),
  exportWeekDays: yup
    .number()
    .min(1, t("appSettings.exportPath.minimumDays"))
    .max(127, t("appSettings.exportPath.maximumSDays")),
  exportTime: yup
    .string()
    .nullable()
    .matches(/^\d{2}:\d{2}$/, t("appSettings.exportPath.exportTimeFormatError")),
  emailWeekDays: yup
    .number()
    .min(1, t("appSettings.emailSettings.minimumDays"))
    .max(127, t("appSettings.emailSettings.maximumDays")),
  emailTime: yup
    .string()
    .nullable()
    .matches(/^\d{2}:\d{2}$/, t("appSettings.emailSettings.emailTimeFormatError"))
});

const { defineField, errors, meta, resetForm } = useForm({
  validationSchema: schema,
  initialValues: {
    exportPathXml: store.exportPathXml,
    exportPathImages: store.exportPathImages,
    exportWeekDays: store.exportWeekDays,
    exportTime: store.exportTime,
    emailWeekDays: store.emailWeekDays,
    emailTime: store.emailTime
  },
});

const [exportPathXml] = defineField("exportPathXml");
const [exportPathImages] = defineField("exportPathImages");
const [exportWeekDays] = defineField("exportWeekDays");
const [exportTime] = defineField("exportTime");
const [emailWeekDays] = defineField("emailWeekDays");
const [emailTime] = defineField("emailTime");

async function fetchData() {
  await store.fetchAppSettingsData(notify);
  resetForm({
    values: {
      exportPathXml: store.exportPathXml,
      exportPathImages: store.exportPathImages,
      exportWeekDays: store.exportWeekDays,
      exportTime: store.exportTime,
      emailWeekDays: store.emailWeekDays,
      emailTime: store.emailTime
    },
  });
}

async function saveSettings() {
  if (!meta.value.valid) return;

  store.exportPathXml = exportPathXml.value;
  store.exportPathImages = exportPathImages.value;
  store.exportWeekDays = exportWeekDays.value;
  store.exportTime = exportTime.value;

  store.emailWeekDays = emailWeekDays.value;
  store.emailTime = emailTime.value;

  const saveResult = await store.saveSettings(notify);

  if (saveResult) {
    resetForm({
      values: {
        exportPathXml: store.exportPathXml,
        exportPathImages: store.exportPathImages,
        exportWeekDays: store.exportWeekDays,
        exportTime: store.exportTime,
        emailWeekDays: store.emailWeekDays,
        emailTime: store.emailTime
      },
    });
  }
}

onMounted(fetchData);
</script>

<template>
  <div>
    <div v-if="store.isLoading" class="flex justify-center p-6">
      <ProgressSpinner />
    </div>
    <div v-else>
      <BlockUI :blocked="store.isLoading" fullScreen>
        <Panel :header="t('appSettings.exportPath.title')" toggleable>
          <export-path-setting
            v-model="exportPathXml"
            v-model:writable="store.isWritableXml"
            :error="errors.exportPathXml"
            :label="t('appSettings.exportPath.exportPathXml')"
            :hint="t('appSettings.exportPath.exportPathXmlHint')"
          />

          <export-path-setting
            v-model="exportPathImages"
            v-model:writable="store.isWritableImages"
            :error="errors.exportPathImages"
            :label="t('appSettings.exportPath.exportPathImages')"
            :hint="t('appSettings.exportPath.exportPathImagesHint')"
          />

          <export-week-days-hours-setting
            :weekDays="exportWeekDays"
            :weekDaysError="errors.exportWeekDays"
            :time="exportTime"
            :timeError="errors.exportTime"
            :label="t('appSettings.exportPath.exportWeekDaysHours')"
            :hint="t('appSettings.exportPath.exportWeekDaysHint')"
            @update:weekDays="exportWeekDays = $event"
            @update:time="exportTime = $event"
          />
        </Panel>
        
        <Panel :header="t('appSettings.emailSettings.title')" toggleable class="mt-4">
          <email-week-days-setting
            :weekDays="emailWeekDays"
            :weekDaysError="errors.emailWeekDays"
            :time="emailTime"
            :timeError="errors.emailTime"
            :label="t('appSettings.emailSettings.emailWeekDays')"
            :hint="t('appSettings.emailSettings.emailWeekDaysHint')"
            @update:weekDays="emailWeekDays = $event"
            @update:time="emailTime = $event"
          />
        </Panel>
        <div class="flex justify-end mt-4">
          <Button
            :label="t('common.buttons.save')"
            :disabled="store.isLoading || !meta.valid || !meta.dirty"
            @click="saveSettings"
          />
        </div>
      </BlockUI>
    </div>
  </div>
</template>
