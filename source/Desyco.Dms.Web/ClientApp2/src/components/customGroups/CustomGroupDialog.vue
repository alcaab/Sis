<script lang="ts" setup>
import { inject, ref, onMounted, watch, nextTick } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";
import FormField from "@/components/FormField.vue";
import type { CustomGroupDto } from "@models";
import { useCustomGroupSchema } from "./customGroupSchema";
import { useCustomGroupStore } from "@/stores/customGroupStore";
import { storeToRefs } from "pinia";
import CustomGroupArticle from "@/components/customGroups/CustomGroupArticle.vue";
import Fieldset from "primevue/fieldset";

const dialogRef = inject<any>("dialogRef");
const { t } = useI18n();
const { schema } = useCustomGroupSchema(t);
const {
  loadAvailableSubGroups,
  loadAvailableArticles,
  addArticle,
  removeArticle,
} = useCustomGroupStore();
const { selectedItem, availableSubGroups, availableArticles } = storeToRefs(
  useCustomGroupStore()
);

const { handleSubmit, meta, errors, setValues, defineField, resetForm } =
  useForm<CustomGroupDto>({
    validationSchema: schema,
    initialValues: {
      name: "",
      subGroups: [],
      articles: [],
    },
  });

const isNew = ref<boolean>();
const [name] = defineField("name");
const [subGroups] = defineField("subGroups");
const [articles] = defineField("articles");

watch(
  () => selectedItem.value?.articles,
  async (value) => {
    await nextTick();
    articles.value = value ?? [];
  },
  { immediate: true }
);

onMounted(() => {
  isNew.value = dialogRef.value.data.isNew;
  
  if (selectedItem) {
    loadAvailableSubGroups(selectedItem.value.id, selectedItem.value.mainGroupNumber);
    loadAvailableArticles(selectedItem.value.id, selectedItem.value.mainGroupNumber);
    
    articles.value = selectedItem.value.articles;
    
    setValues({
      name: selectedItem.value.name,
      subGroups: selectedItem.value.subGroups,
      articles: selectedItem.value.articles,
    });
  }

  resetForm({
    values: {
      name: selectedItem.value?.name || "",
      subGroups: selectedItem.value?.subGroups || [],
      articles: selectedItem.value?.articles || [],
    },
  });
});

const save = handleSubmit((formData: CustomGroupDto) => {
  dialogRef.value.close({
    ...selectedItem.value,
    ...formData,
  });
});

function cancel(){
  dialogRef.value.close();
}

</script>

<template>
  <form class="flex flex-col gap-1" @submit.prevent="save">
    <FormField :error="errors.name" :label="t('customGroup.name')" required>
      <InputText v-model="name" autofocus fluid inputId="name" required />
    </FormField>
    <FormField
      :error="errors.subGroups"
      :label="t('customGroup.subGroups')"
      required>
      <MultiSelect
        v-model="subGroups"
        :maxSelectedLabels="10"
        :options="availableSubGroups"
        :placeholder="t('customGroup.subGroupPlaceholder')"
        class="w-full md:w-20rem"
        display="chip"
        filter
        optionLabel="name" />
    </FormField>
    <div class="flex flex-col mb-4">
      <Fieldset :legend="t('customGroup.articleList')">
        <CustomGroupArticle
          :availableOptions="availableArticles"
          :value="articles"
          @addRow="addArticle"
          @removeRow="removeArticle">
        </CustomGroupArticle>
      </Fieldset>
    </div>
    <div class="flex flex-wrap gap-2 justify-end">
      <Button
        :disabled="!meta.valid || !meta.dirty"
        :label="t('common.buttons.save')"
        icon="pi pi-save"
        type="submit" />
      <Button
        :label="t('common.buttons.close')"
        severity="secondary"
        @click="cancel" />
    </div>
  </form>
</template>
