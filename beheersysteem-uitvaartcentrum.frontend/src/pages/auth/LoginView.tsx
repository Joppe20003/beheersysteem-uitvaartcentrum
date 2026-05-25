import { useNavigate } from "react-router-dom";
import { useAlert } from "../../hooks/useAlert";
import { useAuth } from "../../hooks/useAuth";
import { authService } from "../../services/authService";
import LoginForm from "../../components/forms/LoginUserForm";

function LoginView() {
    const navigate = useNavigate();
    const { refreshStatus } = useAuth();
    const { showAlert, clearAlert } = useAlert();

    const handleSubmit = async (data: { email: string; password: string }) => {
        try {
            await authService.login(data);
            await refreshStatus();

            showAlert("Ingelogd!", "success");
            navigate("/");
        } catch (error: unknown) {
            clearAlert();

            const errorMessage = (error instanceof Error) ? error.message : (typeof error === "object" && error !== null && "message" in error) ? (error as { message: string }).message : null;

            if (errorMessage && errorMessage !== "Failed to fetch") {
                showAlert(errorMessage, "danger");
            } else {
                showAlert("Kan niet inloggen, probeer dit later opnieuw", "danger");
            }
        }
    }

    return (
        <div className="col-sm-8 bg-white border rounded shadow p-5">
            <LoginForm onSubmit={handleSubmit} />
        </div>
    )
}

export default LoginView;