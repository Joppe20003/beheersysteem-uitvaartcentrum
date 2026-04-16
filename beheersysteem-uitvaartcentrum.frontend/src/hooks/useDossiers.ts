import { useEffect, useState } from "react";
import { dossierService } from "../services/dossierService";

function useDossiers() {
    const [dossiers, setDossiers] = useState([]);
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(true);
    useEffect(() => {
        dossierService.getAll()
            .then(setDossiers)
            .catch(() => setError("Kon dossiers niet ophalen, probeer later opnieuw..."))
            .finally(() => setLoading(false));
    }, []);
    return { dossiers, error, loading };
}

export default useDossiers;