import React from "react";
import { useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import { documentService } from "../../services/documentService";

import dateFormatter from "../../utils/shared/dateFormatter";
import useDossierById from "../../hooks/useDossierById";
import useUploadDocument from "../../hooks/useUploadDocument";

import Table, { ColumnDef } from "../../components/shared/Table";

import { ACCEPTED_FILE_EXSTENSIONS } from "../../constants/files"

import { useAuth } from "../../hooks/useAuth";

import nameFormatter from "../../utils/shared/nameFormatter"
import PeopleSelector from "../../components/shared/PeopleSelector";
import Dialog from "../../components/shared/Dialog";

function View() {
    const navigate = useNavigate();
    const { id } = useParams();
    const { dossier, refetch, loading } = useDossierById(id!);
    const { upload, uploading } = useUploadDocument();
    const { user } = useAuth();
    const fileInputRef = useRef<HTMLInputElement>(null);
    const [isInviteDialogOpen, setIsInviteDialogOpen] = useState(false);

    const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {

        const file = e.target.files?.[0];

        if (!file || !id) return;

        await upload(id, file);

        refetch();

        e.target.value = "";
    };

    const handleDownload = async (docId: string, title: string) => {
        const blob = await documentService.download(docId);
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = title;
        a.click();
        URL.revokeObjectURL(url);
    };

    const handleAddUserToDossierClick = () => {
        setIsInviteDialogOpen(true);
    }

    const handleInviteComplete = () => {
        setIsInviteDialogOpen(false);
        refetch();
    }

    const columns: ColumnDef[] = [
        {
            key: "title",
            label: "Titel",
            format: (value, row) => String(value).replace(String(row.extensions), ""),
        },
        {
            key: "extensions",
            label: "Extensie",
            format: (value) => <span className="badge bg-secondary">{String(value)}</span>,
        },
        {
            key: "dateUploaded",
            label: "Geupload op",
            format: (value) => dateFormatter(String(value), "datetime-nl"),
        },
        {
            key: "id",
            label: "Acties",
            format: (value, row) => (
                <button
                    className="btn btn-sm btn-primary"
                    onClick={() => handleDownload(String(value), String(row.title))}
                >
                    Download
                </button>
            ),
        },
    ];

    if (loading) {
        return (
            <section className="row">
                <legend className="h1" tabIndex={0}>Dossiers detail</legend>
                <p className="col-12 text-muted mt-4">Laden...</p>
            </section>
        );
    }

    return (
        <>
            <section className="row">
                <input
                    ref={fileInputRef}
                    type="file"
                    className="d-none"
                    accept={ACCEPTED_FILE_EXSTENSIONS}
                    onChange={handleFileChange}
                />
                <article>
                    <header className="d-flex py-2">
                        <legend className="h3" style={{ textWrap: "nowrap", overflow: "hidden", textOverflow: "ellipsis" }} tabIndex={0}>{dossier?.title}</legend>

                        <div className="d-none d-sm-block">
                            <div className="d-flex" style={{ height: "stretch" }}>
                                <button
                                    className="btn btn-outline-primary mx-2 px-5"
                                    style={{ height: "stretch", fontSize: "1.25rem" }}
                                    onClick={() => navigate(-1)}
                                >
                                    Terug
                                </button>

                                <button
                                    className="btn btn-primary px-5"
                                    style={{
                                        height: "stretch",
                                        width: "max-content",
                                        fontSize: "1.25rem",
                                    }}
                                    disabled={uploading}
                                    onClick={() => fileInputRef.current?.click()}
                                >
                                    {uploading ? "Bezig..." : "Bestand uploaden"}
                                </button>
                            </div>
                        </div>
                    </header>
                    <p className="h4 fw-normal text-muted mb-4" tabIndex={0}>{dossier?.description || "Beschrijving niet aanwezig"}</p>
                    <p className="h4 fw-normal mb-2" tabIndex={0}>Aanmaak datum:</p>
                    <p className="h4 fw-normal text-muted mb-4" tabIndex={0}>{dateFormatter(dossier?.dateCreated, "datetime-nl")}</p>
                    <p className="h4 fw-normal mb-2" tabIndex={0}>Mensen met toegang: ({dossier?.invitedUsers.length})</p>
                    <div className="d-flex align-items-center flex-wrap gap-2 mb-2">
                        {dossier?.invitedUsers.map((invitedUser) => (
                            <div
                                key={invitedUser.userId}
                                className="col-auto d-flex align-items-center justify-content-center shadow-sm"
                                style={{
                                    height: "45px",
                                    width: "45px",
                                    borderRadius: "50%",
                                    backgroundColor: "#e3f2fd",
                                    border: "2px solid rgb(0, 192, 255)",
                                    color: "rgb(0, 192, 255)",
                                    fontSize: "0.9rem",
                                    fontWeight: "bold",
                                    position: "relative",
                                    cursor: "pointer"
                                }}
                                tabIndex={0}
                                role="img"
                                aria-label={`Toegang voor: ${invitedUser.userName}`}
                                title={invitedUser.userName}
                            >
                                {nameFormatter(invitedUser.userName)}
                            </div>
                        ))}

                        { user?.userId == dossier?.userId && (
                            <button
                                className="col-auto btn d-flex align-items-center justify-content-center shadow-sm"
                                onClick={handleAddUserToDossierClick}
                                style={{
                                    height: "45px",
                                    width: "45px",
                                    borderRadius: "50%",
                                    border: "2px dashed rgb(0, 192, 255)",
                                    backgroundColor: "transparent",
                                    color: "rgb(0, 192, 255)",
                                    transition: "all 0.2s ease"
                                }}
                                aria-label="Nieuwe gebruiker uitnodigen voor dit dossier"
                                title="Gebruiker toevoegen"
                            >
                                <i className="bi bi-person-plus-fill" style={{ fontSize: "1.2rem" }} />
                            </button>
                        )}
                    </div>
                    <p className="h4 fw-normal mb-2" tabIndex={0}>Bestanden: (toegestaande extensies: PDF, JPG, JPEG, PNG)</p>
                    <Table data={dossier?.documents || []} columns={columns} noResultsText="Geen gekoppelde bestanden bij dit dossier" />
                </article>
                <div className="d-sm-none col-12" style={{ height: 120 }} />
                <div className="d-sm-none position-fixed bg-white bottom-0 border-top shadow p-2">
                    <div className="d-flex flex-column">
                        <button
                            className="btn btn-outline-primary"
                            onClick={() => navigate(-1)}
                        >
                            Terug
                        </button>
                        <button
                            className="btn btn-primary w-100 my-2"
                            disabled={uploading}
                            onClick={() => fileInputRef.current?.click()}
                        >
                            {uploading ? "Bezig..." : "Bestand uploaden"}
                        </button>
                    </div>
                </div>
            </section>

            <Dialog isOpen={isInviteDialogOpen} title="Nodig iemand uit" onClose={() => setIsInviteDialogOpen(false)} size="md" footer={null}>
                <PeopleSelector dossierId={dossier?.id} onInviteComplete={handleInviteComplete} onClose={() => setIsInviteDialogOpen(false)} />
            </Dialog>
        </>
    );
}

export default View;
