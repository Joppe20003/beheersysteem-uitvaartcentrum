import { BASE_URL } from "../constants/routes";

const post = async (url: string, data: unknown) => {
    const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
    });

    if (!res.ok) throw await res;

    return res.json();
};

export const dossierService = {
    create: (data: unknown) => post(BASE_URL + "dossier", data),
};