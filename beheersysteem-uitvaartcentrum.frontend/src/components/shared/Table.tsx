import React from "react";

export interface ColumnDef {
    key: string;
    label?: string;
    format?: (value: unknown, row: Record<string, unknown>) => React.ReactNode;
}

interface TableProps {
    data: Record<string, unknown>[];
    columns?: ColumnDef[];
}

function Table({ data, columns }: TableProps) {
    const cols: ColumnDef[] = columns ?? Object.keys(data[0]).map((key) => ({ key }));

    if (data.length === 0) {
        return <p className="text-muted">Nog geen bestand aan dit dossier gekoppeld.</p>;
    }



    return (
        <div className="table-responsive">
            <table className="table table-striped table-hover mb-0">
                <thead>
                    <tr>
                        {cols.map((col, i) => (
                            <th key={`${col.key}-${i}`} scope="col">
                                {col.label ?? col.key}
                            </th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {data.map((row, i) => (
                        <tr key={i}>
                            {cols.map((col, j) => {
                                const value = row[col.key];

                                return (
                                    <td key={`${col.key}-${j}`}>
                                        {col.format
                                            ? col.format(value, row)
                                            : String(value ?? "")}
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