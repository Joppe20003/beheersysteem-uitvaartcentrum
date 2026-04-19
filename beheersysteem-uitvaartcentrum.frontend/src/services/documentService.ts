import { BASE_URL } from "../constants/routes";

class DocumentService {
    private baseUrl: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    upload(dossierId: string, file: File) {
        const formData = new FormData();
        formData.append("DossierId", dossierId);
        formData.append("File", file);

        return fetch(this.baseUrl + "Document", {
            method: "POST",
            body: formData,
        }).then(async (res) => {
            const data = await res.json().catch(() => null);

            if (!res.ok) {
                throw data;
            }

            return data;
        });
    }

    download(id: string) {
        return fetch(this.baseUrl + `Document/${id}/download`, {
            method: "GET",
        }).then((res) => {
            if (!res.ok) throw res;
            return res.blob();
        });
    }
}

export const documentService = new DocumentService(BASE_URL);