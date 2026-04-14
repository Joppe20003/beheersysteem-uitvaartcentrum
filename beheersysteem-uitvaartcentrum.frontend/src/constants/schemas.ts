import { z } from "zod";

export const createDossierSchema = z.object({
    title: z.string().min(1, "Titel is verplicht"),
    description: z.string().optional()
});

export type CreateDossierValues = z.infer<typeof createDossierSchema>;