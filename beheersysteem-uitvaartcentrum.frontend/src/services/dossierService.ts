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

    private async get(endpoint: string) {
        const res = await fetch(this.baseUrl + endpoint, {
            method: "GET",
            headers: { "Content-Type": "application/json" },
        });

        if (!res.ok) throw res;
        return res.json();
    }

    create(data: unknown) {
        return this.post("dossier", data);
    }

    getAll() {
        return this.get("dossier");
    }

    getById(id: string) {
        return this.get(`dossier/${id}`);
    }
}

export const dossierService = new DossierService(BASE_URL);