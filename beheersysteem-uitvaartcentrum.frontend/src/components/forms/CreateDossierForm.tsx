import { useRef, useEffect } from "react";
import { useForm } from "../../hooks/useForm";
import { createDossierSchema } from "../../constants/schemas";
import { FieldError } from "./FieldError";

interface CreateDossierFormProps {
    onSubmit: (data: { title: string; description?: string }) => Promise<void>;
    onBack: () => void;
}

function CreateDossierForm({ onSubmit, onBack }: CreateDossierFormProps) {
    const { values, errors, isLoading, handleChange, handleSubmit } = useForm(
        { title: "", description: "" },
        createDossierSchema
    );
    const titleRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (errors.title?.length) {
            titleRef.current?.focus();
        }
    }, [errors]);

    const handleFormSubmit = handleSubmit(async (data) => {
        await onSubmit(data);
    });

    return (
        <form onSubmit={handleFormSubmit} noValidate>
            <fieldset>
                <legend>Dossier aanmaken formulier</legend>
                <div className="mb-3">
                    <label htmlFor="title" className="form-label">Titel</label>
                    <input
                        ref={titleRef}
                        type="text"
                        className={`form-control ${errors.title?.length ? "is-invalid" : ""}`}
                        id="title"
                        name="title"
                        value={values.title}
                        onChange={handleChange}
                        aria-describedby="title-error"
                    />
                    <span id="title-error">
                        <FieldError errors={errors.title} />
                    </span>
                </div>
                <div className="mb-3">
                    <label htmlFor="description" className="form-label">Beschrijving (optioneel)</label>
                    <textarea
                        className={`form-control ${errors.description?.length ? "is-invalid" : ""}`}
                        id="description"
                        name="description"
                        value={values.description}
                        onChange={handleChange}
                        aria-describedby="description-error"
                    />
                    <span id="description-error">
                        <FieldError errors={errors.description} />
                    </span>
                </div>
                <button type="button" className="btn btn-outline-primary mb-3 w-100" onClick={onBack}>
                    Terug
                </button>
                <button type="submit" className="btn btn-primary w-100" disabled={isLoading}>
                    {isLoading ? "Bezig met aanmaken..." : "Aanmaken"}
                </button>
            </fieldset>
        </form>
    );
}

export default CreateDossierForm;