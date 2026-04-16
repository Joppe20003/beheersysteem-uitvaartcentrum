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
        } catch (err: any) {
            const message =
                err?.errors?.message?.[0] ||
                err?.title ||
                err?.message ||
                "Upload mislukt";

            showAlert(message, "danger");
        } finally {
            setUploading(false);
        }
    };

    return { upload, uploading };
}

export default useUploadDocument;