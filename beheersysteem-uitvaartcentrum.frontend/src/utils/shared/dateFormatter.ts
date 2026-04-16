type DateFormat = "datetime-nl";

function dateFormatter(date: string | undefined, format: DateFormat) {
    if (!date) return "";

    const d = new Date(date);

    if (isNaN(d.getTime())) return "";

    switch (format) {
        case "datetime-nl":
            return `${d.toLocaleTimeString("nl-NL", {
                hour: "2-digit", minute: "2-digit"
            })}, ${d.toLocaleDateString("nl-NL", {
                day: "numeric", month: "numeric", year: "numeric"
            })}`;
    }
}

export default dateFormatter;