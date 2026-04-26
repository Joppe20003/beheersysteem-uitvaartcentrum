import { useNavigate } from "react-router-dom";
import RegisterUserForm from "../../components/forms/RegisterUserForm";
import { useAlert } from "../../hooks/useAlert";
import { authService } from "../../services/authService";

function RegisterView() {
    const navigate = useNavigate();
    const { showAlert, clearAlert } = useAlert();

    const handleSubmit = async (data: { username: string, email: string; role: number, password: string }) => {
        try {
            await authService.register(data);

            showAlert("Account succesvol aangemaakt!", "success");
            navigate("/login");
        } catch (error: unknown) {
            clearAlert();

            if (typeof error === "object" && error !== null && "message" in error) {
                showAlert((error as { message: string }).message, "danger");
            } else {
                showAlert("Kan geen account aanmaken, probeer dit later opnieuw", "danger");
            }
        }
    }

    return (
        <div className="col-sm-8 bg-white border rounded shadow p-5">
            <RegisterUserForm onSubmit={handleSubmit} />
        </div>
    )
}

export default RegisterView;