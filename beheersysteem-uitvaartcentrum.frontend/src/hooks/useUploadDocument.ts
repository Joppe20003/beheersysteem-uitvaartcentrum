import { useState } from "react";
import { useAlert } from "../hooks/useAlert"
import { documentService } from "../services/documentService";

function useUploadDocument() {
    const [uploading, setUploading] = useState(false);
    const { clearAlert, showAlert } = useAlert();

    const upload = async (dossierId: string, file: File) => {
        clearAlert();
        setUploading(true);

        try {
            await documentService.upload(dossierId, file);
        } catch (error: unknown) {
            clearAlert();

            const message = typeof error === "string" ? error : "Kan geen document uploaden, probeer dit later opnieuw";

            showAlert(message, "danger");
        } finally {
            setUploading(false);
        }
    };

    return { upload, uploading };
}

export default useUploadDocument;