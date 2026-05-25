import { useState } from "react";
import { ZodSchema } from "zod";

type ServerError = {
    errors?: Record<string, string[]>;
};

export function useForm<T extends object>(initialValues: T, schema?: ZodSchema<T>) {
    const [values, setValues] = useState<T>(initialValues);
    const [errors, setErrors] = useState<Record<string, string[]>>({});
    const [isLoading, setIsLoading] = useState(false);

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
    ) => {
        const { name, value } = e.target;

        setValues((prev) => ({
            ...prev,
            [name]: name === "role" ? Number(value) : value,
        }));

        setErrors((prev) => ({
            ...prev,
            [name]: [],
        }));
    };

    const handleSubmit = (onSubmit: (values: T) => Promise<void>) =>
        async (e: React.FormEvent) => {
            e.preventDefault();
            setErrors({});

            if (schema) {
                const result = schema.safeParse(values);
                if (!result.success) {
                    setErrors(result.error.flatten().fieldErrors as Record<string, string[]>);
                    return;
                }
            }

            setIsLoading(true);
            try {
                await onSubmit(values);
            } catch (err: unknown) {
                const error = err as ServerError;

                if (error?.errors) {
                    const serverErrors = Object.fromEntries(
                        Object.entries(error.errors).map(([k, v]) => [
                            k.charAt(0).toLowerCase() + k.slice(1),
                            v
                        ])
                    );
                    setErrors(serverErrors);
                }
            } finally {
                setIsLoading(false);
            }
        };

    return { values, errors, isLoading, handleChange, handleSubmit };
}