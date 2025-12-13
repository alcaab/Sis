import { useToast } from "primevue/usetoast";

export function useNotification() {
    const toast = useToast();

    const showSuccess = (message: string, title: string = "Success") => {
        toast.add({ severity: "success", summary: title, detail: message, life: 3000 });
    };

    const showError = (error: any, title: string = "Error") => {
        let message = "An unexpected error occurred.";

        if (error.response) {
            const data = error.response.data;
            const status = error.response.status;

            if (status === 403) {
                message = data?.detail || "You do not have permission to perform this action.";
                title = data?.title || "Forbidden";
            } else if (status === 401) {
                message = data?.detail || "Session expired. Please login again.";
                title = data?.title || "Unauthorized";
            } else if (data) {
                // ProblemDetails standard
                if (data.detail) {
                    message = data.detail;
                }
                // Validation errors (Dictionary<string, string[]>)
                else if (data.errors && typeof data.errors === "object") {
                    // Extract first error message or join them
                    const messages = Object.values(data.errors).flat();
                    message = messages.length > 0 ? String(messages[0]) : "Validation error";
                }
                // Custom Auth result (Errors array)
                else if (data.Errors && Array.isArray(data.Errors)) {
                    message = data.Errors[0];
                }
                // Simple string message
                else if (typeof data === "string") {
                    message = data;
                }
            }
        } else if (error.message) {
            message = error.message;
        }

        toast.add({ severity: "error", summary: title, detail: message, life: 5000 });
    };

    return {
        showSuccess,
        showError,
    };
}
