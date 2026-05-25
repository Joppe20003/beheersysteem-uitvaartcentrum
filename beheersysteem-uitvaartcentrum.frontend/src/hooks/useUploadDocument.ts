import { useState } from "react";
import { useAlert } from "../hooks/useAlert"
import { documentService } from "../services/documentService";
import { ACCEPTED_FILE_MIMES, ACCEPTED_FILE_EXTENSIONS, MAX_UPLOAD_SIZE_BYTES } from "../constants/files";

function useUploadDocument() {
    const [uploading, setUploading] = useState(false);
    const { clearAlert, showAlert } = useAlert();

    const upload = async (dossierId: string, file: File) => {
        clearAlert();
        setUploading(true);

        if (!ACCEPTED_FILE_MIMES.includes(file.type) && !ACCEPTED_FILE_EXTENSIONS.includes((file.name.match(/\.[0-9a-z]+$/i) || [""])[0].toLowerCase())) {
            setUploading(false);
            showAlert("Bestandstype niet toegestaan. Toegestane types: PDF, PNG, JPG, JPEG", "danger");
            return;
        }

        if (file.size > MAX_UPLOAD_SIZE_BYTES) {
            setUploading(false);
            showAlert("Bestand is te groot. Maximaal toegestaan: 1 GB", "danger");
            return;
        }

        try {
            await documentService.upload(dossierId, file);
        } catch (error: unknown) {
            clearAlert();

            const errorMessage = (error instanceof Error) ? error.message : (typeof error === "object" && error !== null && "message" in error) ? (error as { message: string }).message : null;

            if (errorMessage && errorMessage !== "Failed to fetch") {
                showAlert(errorMessage, "danger");
            } else {
                showAlert("Kan geen document uploaden, probeer dit later opnieuw", "danger");
            }
        } finally {
            setUploading(false);
        }
    };

    return { upload, uploading };
}

export default useUploadDocument;