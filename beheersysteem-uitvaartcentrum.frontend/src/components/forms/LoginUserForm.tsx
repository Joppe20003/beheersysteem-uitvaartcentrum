import { useRef, useEffect } from "react";
import { useForm } from "../../hooks/useForm";
import { loginSchema, LoginValues } from "../../constants/schemas";
import { FieldError } from "./FieldError";

interface LoginFormProps {
    onSubmit: (data: LoginValues) => Promise<void>;
}

function LoginForm({ onSubmit }: LoginFormProps) {
    const { values, errors, isLoading, handleChange, handleSubmit } = useForm<LoginValues>(
        { email: "", password: "" },
        loginSchema
    );

    const emailRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (errors.password?.length) {
            passwordRef.current?.focus();
        }
        if (errors.email?.length) {
            emailRef.current?.focus();
        }
    }, [errors]);

    const handleFormSubmit = handleSubmit(async (data) => {
        await onSubmit(data);
    });

    return (
        <form onSubmit={handleFormSubmit} noValidate>
            <fieldset>
                <legend>Inloggen</legend>

                <div className="mb-3">
                    <label htmlFor="email" className="form-label">E-mailadres</label>
                    <input
                        ref={emailRef}
                        type="email"
                        className={`form-control ${errors.email?.length ? "is-invalid" : ""}`}
                        id="email"
                        name="email"
                        value={values.email}
                        onChange={handleChange}
                        aria-describedby="email-error"
                        autoComplete="email"
                    />
                    <span id="email-error">
                        <FieldError errors={errors.email} />
                    </span>
                </div>

                <div className="mb-3">
                    <label htmlFor="password" className="form-label">Wachtwoord</label>
                    <input
                        ref={passwordRef}
                        type="password"
                        className={`form-control ${errors.password?.length ? "is-invalid" : ""}`}
                        id="password"
                        name="password"
                        value={values.password}
                        onChange={handleChange}
                        aria-describedby="password-error"
                        autoComplete="current-password"
                    />
                    <span id="password-error">
                        <FieldError errors={errors.password} />
                    </span>
                </div>

                <p className="mb-3">
                    Nog geen account? <a href="/registreren">Registreer hier</a>.
                </p>

                <button type="submit" className="btn btn-primary mb-3 w-100" disabled={isLoading}>
                    {isLoading ? "Bezig met inloggen..." : "Inloggen"}
                </button>
            </fieldset>
        </form>
    );
}

export default LoginForm;