import { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { dossierService } from "../services/dossierService";

type Dossier = {
    id: string;
    userId: string;
    invitedUsers: string[];
    title: string;
    description?: string;
    dateCreated: string;
    documents: Record<string, unknown>[];
};

function useDossierById(id: string) {
    const [dossier, setDossier] = useState<Dossier | null>(null);
    const [loading, setLoading] = useState(true);
    const [trigger, setTrigger] = useState(0);

    const refetch = useCallback(() => setTrigger(t => t + 1), []);
    const navigate = useNavigate();

    useEffect(() => {
        if (!id) return;

        let cancelled = false;

        dossierService.getById(id)
            .then((data) => {
                setLoading(true);

                if (!cancelled) setDossier(data);
            })
            .catch((error) => {
                if (!cancelled) {
                    console.error(error)

                    navigate(-1);
                }
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [id, trigger, navigate]);

    return { dossier, loading, refetch };
}

export default useDossierById;