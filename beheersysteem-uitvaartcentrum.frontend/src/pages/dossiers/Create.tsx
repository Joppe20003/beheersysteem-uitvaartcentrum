import { useNavigate } from "react-router-dom";
import CreateDossierForm from "../../components/forms/CreateDossierForm";
import { useAlert } from "../../context/AlertContext";
import { dossierService } from "../../services/dossierService";

function Create() {
    const navigate = useNavigate();
    const { showAlert, clearAlert } = useAlert();

    const handleSubmit = async (data: { title: string; description?: string }) => {
        try {
            await dossierService.create(data);

            showAlert("Dossier succesvol aangemaakt!", "success");
            navigate("/dossiers");
        } catch {
            clearAlert();
            showAlert("Kan geen dossier aanmaken, probeer later opnieuw", "danger");
        }
    };

    const handleBack = () => navigate(-1);

    return (
        <div className="col-lg-12 my-2">
            <CreateDossierForm onSubmit={handleSubmit} onBack={handleBack} />
        </div>
    );
}


export default Create;