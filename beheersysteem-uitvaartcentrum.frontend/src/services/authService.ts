import { BASE_URL } from "../constants/routes";

class AuthService {
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

        if (!res.ok) {
            const error = await res.json();

            throw error
        }

        return res.json();
    }

    private async get(endpoint: string) {
        const res = await fetch(this.baseUrl + endpoint, {
            method: "GET",
            headers: { "Content-Type": "application/json" },
            credentials: "include"
        });

        if (!res.ok) {
            const error = await res.json();

            throw error;
        }

        return res.json();
    }

    async register(data: unknown) {
        return this.post("auth/register", data);
    }

    async login(data: unknown) {
        return this.post("auth/login", data);
    }

    async logout() {
        return this.post("auth/logout", {});
    }

    async status() {
        return this.get("auth/status");
    }
}

export const authService = new AuthService(BASE_URL);