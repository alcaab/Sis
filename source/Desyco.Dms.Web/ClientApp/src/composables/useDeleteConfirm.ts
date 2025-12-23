import { ref } from "vue";
import { useConfirm } from "primevue/useconfirm";
import { useNotification } from "@/composables/useNotification";
import { useI18n } from "vue-i18n";

interface DeleteOptions<T> {
    item: T;
    deleteAction: (id: any) => Promise<void>;
    onSuccess?: () => void;
    message?: string;
    header?: string;
}

/**
 * Composable to handle delete confirmation dialogs and logic.
 * Centralizes PrimeVue ConfirmDialog and Notifications.
 */
export function useDeleteConfirm() {
    const confirm = useConfirm();
    const notify = useNotification();
    const { t } = useI18n();

    const deletingItemId = ref<any | null>(null);

    const confirmDelete = <T extends { id?: any; name?: string | null }>({
        item,
        deleteAction,
        onSuccess,
        message,
        header,
    }: DeleteOptions<T>) => {
        confirm.require({
            message: message || t("common.notifications.confirmDelete", { name: item.name || "" }),
            header: header || t("common.notifications.confirmDeleteTitle"),
            icon: "pi pi-exclamation-triangle",
            rejectProps: {
                label: t("common.buttons.cancel"),
                severity: "secondary",
                outlined: true,
            },
            acceptProps: {
                label: t("common.buttons.delete"),
                severity: "danger",
            },
            accept: async () => {
                if (item.id === undefined || item.id === null) return;

                deletingItemId.value = item.id;
                try {
                    await deleteAction(item.id);
                    notify.showSuccess(t("common.notifications.deleteSuccess"));
                    if (onSuccess) onSuccess();
                } catch (error: any) {
                    notify.showError(error, t("common.notifications.deleteError"));
                } finally {
                    deletingItemId.value = null;
                }
            },
        });
    };

    return {
        deletingItemId,
        confirmDelete,
    };
}
