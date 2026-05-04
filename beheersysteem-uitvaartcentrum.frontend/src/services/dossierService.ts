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
            credentials: "include"
        });

        console.log("POST", endpoint, data, res);

        if (!res.ok) {
            const error = await res.json();

            throw error
        }

        if (res.status == 204) {
            return null;
        }

        return res.json();
    }

    private async get(endpoint: string) {
        const res = await fetch(this.baseUrl + endpoint, {
            method: "GET",
            headers: { "Content-Type": "application/json" },
            credentials: "include"
        });

        if (!res.ok) throw res;
        return res.json();
    }

    async invite(data: unknown) {
        return this.post("dossier/invite", data);
    }

    async create(data: unknown) {
        return this.post("dossier/create", data);
    }

    async getAll() {
        return this.get("dossier");
    }

    async getById(id: string) {
        return this.get(`dossier/${id}`);
    }
}

export const dossierService = new DossierService(BASE_URL);