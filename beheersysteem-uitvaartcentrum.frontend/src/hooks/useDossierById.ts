import { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { useAlert } from "./useAlert";
import { dossierService } from "../services/dossierService";

type InvitedUser = {
    userId: string;
    userName: string;
};

type Dossier = {
    id: string;
    userId: string;
    invitedUsers: InvitedUser[];
    title: string;
    description?: string;
    dateCreated: string;
    documents: Record<string, unknown>[];
};

function useDossierById(id: string) {
    const [dossier, setDossier] = useState<Dossier | null>(null);
    const [loading, setLoading] = useState(true);
    const [trigger, setTrigger] = useState(0);

    const { showAlert } = useAlert();
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
                    console.error(error);

                    showAlert("Dossier kon niet worden gevonden of bestaat niet meer.", "danger");

                    navigate("/dossiers");
                }
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [id, trigger, navigate, showAlert]);

    return { dossier, loading, refetch };
}

export default useDossierById;