import { useNavigate } from "react-router-dom";
import { useAlert } from "../../hooks/useAlert";
import { authService } from "../../services/authService";
import LoginForm from "../../components/forms/LoginUserForm";

function LoginView() {
    const navigate = useNavigate();
    const { showAlert, clearAlert } = useAlert();

    const handleSubmit = async (data: { email: string; password: string }) => {
        try {
            await authService.login(data);

            showAlert("Ingelogd!", "success");
            navigate("/");
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
            <LoginForm onSubmit={handleSubmit} />
        </div>
    )
}

export default LoginView;