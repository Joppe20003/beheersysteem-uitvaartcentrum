function roleFormatter(role: string | undefined) {
    if (!role) return "";

    const parts = role.split(/(?=[A-Z])/);

    return parts.join(" ");
}

export default roleFormatter;