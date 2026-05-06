import { BASE_URL } from "../constants/routes";

class UserService {
    private baseUrl: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
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

    async getAll() {
        return this.get("users");
    }
}

export const userService = new UserService(BASE_URL);
