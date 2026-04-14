interface Props {
    errors?: string[];
}

export function FieldError({ errors }: Props) {
    if (!errors?.length) return null;

    return <div className="invalid-feedback d-block">{errors[0]}</div>;
}