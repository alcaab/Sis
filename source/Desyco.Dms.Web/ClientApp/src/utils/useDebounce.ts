import { ref, watch, onUnmounted } from 'vue';

export function useDebounce(initialValue: string, delay: number = 500) {
    const inputValue = ref(initialValue);
    const debouncedValue = ref(initialValue);
    const isDebouncing = ref(false);

    let timer: any | null = null;

    watch(inputValue, (newValue) => {
        clearTimeout(timer as number);

        isDebouncing.value = true;

        timer = setTimeout(() => {
            debouncedValue.value = newValue;
            isDebouncing.value = false;
        }, delay);
    });

    onUnmounted(() => {
        clearTimeout(timer as number);
    });

    return {
        inputValue,
        debouncedValue,
        isDebouncing
    };
}
