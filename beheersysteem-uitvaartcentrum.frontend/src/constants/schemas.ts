import { z } from "zod";

export const createDossierSchema = z.object({
    title: z.string().min(1, "Titel is verplicht"),
    description: z.string().optional()
});

export const createUserSchema = z.object({
    username: z.string().min(1, "Gebruikersnaam is verplicht"),
    email: z.string().email("Ongeldig e-mailadres"),
    role: z.number().min(1, "Rol is verplicht"),
    password: z.string().min(18, "Wachtwoord moet minimaal 18 tekens bevatten")
});

export const loginSchema = z.object({
    email: z.string().email("Ongeldig e-mailadres"),
    password: z.string().min(1, "Wachtwoord is verplicht")
});

export type CreateDossierValues = z.infer<typeof createDossierSchema>;
export type CreateUserValues = z.infer<typeof createUserSchema>;
export type LoginValues = z.infer<typeof loginSchema>;