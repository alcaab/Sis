export type CallbackOptions<T = void> = {
  onSuccess?: (data: T) => void;
  onError?: (error: Error) => void;
};
