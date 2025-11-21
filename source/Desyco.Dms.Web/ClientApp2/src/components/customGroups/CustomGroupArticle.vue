<script setup lang="ts">
import { useI18n } from "vue-i18n";
import { defineProps, ref } from "vue";
import type { CustomGroupArticleDto } from "@models";

const { t } = useI18n();

interface Props {
  value?: CustomGroupArticleDto[] | undefined
  availableOptions?: CustomGroupArticleDto[] | undefined
}

withDefaults(defineProps<Props>(), {
  value: () => [],
  availableOptions: () => []
});

const emit = defineEmits<{
  (e: 'addRow', item: CustomGroupArticleDto): void
  (e: 'removeRow', item: CustomGroupArticleDto): void
}>();

const selectedItem = ref<CustomGroupArticleDto | null>(null);

function onAddRow(item: CustomGroupArticleDto | null) {
  if(!item)
    return;
  
  emit('addRow', {...item });
  selectedItem.value = null;
}

function onRemoveRow(item: CustomGroupArticleDto | null) {
  if(!item)
    return;
  
  emit('removeRow', {...item });
  selectedItem.value = null;
}

</script>

<template>
  <DataTable
    ref="dt"
    :value="value"
    dataKey="id"
    striped-rows
    scrollable 
    scrollHeight="300px"
    responsiveLayout="scroll">
    <template #header>
      <InputGroup>
        <Select
          v-model="selectedItem"
          :options="availableOptions"
          optionLabel="displayName"
          filter
          showClear
          :placeholder="t('customGroup.selectPlaceholder')"
          class="w-[80%]">
        </Select>
        <InputGroupAddon>
          <Button
            @click="onAddRow(selectedItem)"
            :disabled="!selectedItem"
            :label="t('customGroup.addArticle')"
            v-tooltip="t('common.buttons.add')"
            icon="pi pi-plus"/>
        </InputGroupAddon>
      </InputGroup>
    </template>
    <Column :header="t('customGroup.articleDescription')" header-class="w-[50%]">
      <template #body="{ data }">
        {{data.articleNumber}} - {{ data.name}}
      </template>
    </Column>
    <Column :header="t('common.actions')">
      <template #body="{ data }">
        <Button
          icon="pi pi-trash"
          severity="danger"
          variant="text"
          v-tooltip="t('common.buttons.delete')"
          :disabled="!!selectedItem"
          @click="onRemoveRow(data)" />
      </template>
    </Column>
    <template #empty> {{ t("common.noEntries") }}</template>
  </DataTable>  
</template>

<style scoped>
::v-deep(.p-datatable-header) {
  padding-right: 0;
  padding-left: 0;
}
</style>
