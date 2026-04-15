import { BASE_URL } from "../constants/routes";

class DossierService {
    private baseUrl: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    private async post(endpoint: string, data: unknown) {
        const res = await fetch(this.baseUrl + endpoint, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        });

        if (!res.ok) throw res;
        return res.json();
    }

    create(data: unknown) {
        return this.post("dossier", data);
    }
}

export const dossierService = new DossierService(BASE_URL);