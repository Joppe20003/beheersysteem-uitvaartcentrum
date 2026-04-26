import { BASE_URL } from "../constants/routes";

class DocumentService {
    private baseUrl: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    async upload(dossierId: string, file: File) {
        const formData = new FormData();

        formData.append("DossierId", dossierId);
        formData.append("File", file);

        const res = await fetch(this.baseUrl + "Document", {
            method: "POST",
            body: formData,
            credentials: "include"
        });

        if (!res.ok) {

            const responseText = await res.text();

            throw responseText || res.statusText || "Server fout";
        }

        return res.json();
    }

    async download(id: string) {
        return fetch(this.baseUrl + `Document/${id}/download`, {
            method: "GET",
        }).then((res) => {
            if (!res.ok) throw res;
            return res.blob();
        });
    }
}

export const documentService = new DocumentService(BASE_URL);