function nameFormatter(name: string) {
    const nameParts = name.split(" ");
    const formattedName = nameParts.map(part => part[0]).join("").toUpperCase().substring(0, 2);

    return formattedName;
}

export default nameFormatter;