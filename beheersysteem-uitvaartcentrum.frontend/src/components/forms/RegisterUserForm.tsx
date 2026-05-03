import React from "react";

import { useRef, useEffect } from "react";
import { useForm } from "../../hooks/useForm";
import { createUserSchema } from "../../constants/schemas";
import { FieldError } from "./FieldError";

interface RegisterUserFormProps {
    onSubmit: (data: { username: string; email: string; password: string; role: number }) => Promise<void>;
}

function RegisterUserForm({ onSubmit }: RegisterUserFormProps) {
    const { values, errors, isLoading, handleChange, handleSubmit } = useForm(
        { username: "", email: "", password: "", role: 0 },
        createUserSchema
    );

    const passwordRef = useRef<HTMLInputElement>(null);
    const roleRef = useRef<HTMLSelectElement>(null);
    const emailRef = useRef<HTMLInputElement>(null);
    const usernameRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (errors.password?.length) {
            passwordRef.current?.focus();
        }
        if (errors.role?.length) {
            roleRef.current?.focus();
        }
        if (errors.email?.length) {
            emailRef.current?.focus();
        }
        if (errors.username?.length) {
            usernameRef.current?.focus();
        }
    }, [errors]);

    const handleFormSubmit = handleSubmit(async (data) => {
        await onSubmit(data);
    });

    return (
        <form onSubmit={handleFormSubmit} noValidate>
            <fieldset>
                <legend>Account maken formulier</legend>
                <div className="mb-3">
                    <label htmlFor="username" className="form-label">Gebruikersnaam</label>
                    <input
                        ref={usernameRef}
                        type="text"
                        className={`form-control ${errors.username?.length ? "is-invalid" : ""}`}
                        id="username"
                        name="username"
                        value={values.username}
                        onChange={handleChange}
                        aria-describedby="username-error"
                    />
                    <span id="username-error">
                        <FieldError errors={errors.username} />
                    </span>
                </div>
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
                    />
                    <span id="email-error">
                        <FieldError errors={errors.email} />
                    </span>
                </div>
                <div className="mb-3">
                    <label htmlFor="role" className="form-label">Rol</label>
                    <select
                        className={`form-select ${errors.role?.length ? "is-invalid" : ""}`}
                        id="role"
                        name="role"
                        value={values.role}
                        onChange={handleChange}
                        aria-describedby="role-error"
                    >
                        <option value="0">Selecteer een rol</option>
                        <option value="1">Uitvaartondernemer</option>
                        <option value="2">Externe</option>
                    </select>
                    <span id="role-error">
                        <FieldError errors={errors.role} />
                    </span>
                </div>
                <div className="mb-3">
                    <label htmlFor="password" className="form-label">Wachtwoord (Minimaal 16 tekens)</label>
                    <input
                        ref={passwordRef}
                        type="password"
                        className={`form-control ${errors.password?.length ? "is-invalid" : ""}`}
                        id="password"
                        name="password"
                        value={values.password}
                        onChange={handleChange}
                        aria-describedby="password-error"
                    />
                    <span id="password-error">
                        <FieldError errors={errors.password} />
                    </span>
                </div>

                <p className="mb-3">
                    Al een account? <a href="/login">Login hier in</a>.
                </p>

                <button type="submit" className="btn btn-primary mb-3 w-100" disabled={isLoading}>
                    {isLoading ? "Bezig met aanmaken..." : "Account aanmaken"}
                </button>
            </fieldset>
        </form>
    );
}

export default RegisterUserForm;