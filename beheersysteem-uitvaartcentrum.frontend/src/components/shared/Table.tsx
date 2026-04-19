import React from "react";

export interface ColumnDef {
    key: string;
    label?: string;
    format?: (
        value: unknown,
        row: Record<string, unknown>
    ) => React.ReactNode;
}

interface TableProps {
    data: Record<string, unknown>[];
    columns?: ColumnDef[];
    caption?: string;
    noResultsText: string;
}

function Table({
    data,
    columns,
    caption = "Gegevens tabel",
    noResultsText
}: TableProps) {
    if (data.length === 0) {
        return (
            <p tabIndex={0}>
                {noResultsText}
            </p>
        );
    }

    const cols: ColumnDef[] =
        columns ??
        Object.keys(data[0]).map((key) => ({
            key,
            label: key,
        }));

    return (
        <div
            className="table-responsive"
            tabIndex={0}
            aria-label={caption}
        >
            <table className="table table-striped table-hover mb-0">
                <thead>
                    <tr>
                        {cols.map((col) => (
                            <th
                                key={col.key}
                                scope="col"
                                tabIndex={0}
                            >
                                {col.label ?? col.key}
                            </th>
                        ))}
                    </tr>
                </thead>

                <tbody>
                    {data.map((row, rowIndex) => (
                        <tr key={rowIndex}>
                            {cols.map((col) => {
                                const content = col.format ? col.format(row[col.key], row) : String(row[col.key] ?? "");

                                return (
                                    <td
                                        key={col.key}
                                        tabIndex={0}
                                    >
                                        {content}
                                    </td>
                                );
                            })}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default Table;