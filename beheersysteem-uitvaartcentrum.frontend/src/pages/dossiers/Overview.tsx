import { useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import useDossiers from "../../hooks/useDossiers";
import OverviewDossierCartComponent from "../../components/dossiers/OverviewDossierCartComponent";

function Overview() {
    const navigate = useNavigate();
    const { dossiers, error, loading } = useDossiers();
    const { hasPermission } = useAuth();

    return (
        <section className="row">
            <header className="col-12 pt-2 d-flex">
                <legend tabIndex={0}>Dossiers overzicht</legend>
                {hasPermission("dossier:create") && (
                    <button
                        className="btn btn-primary d-none d-sm-block px-5"
                        style={{ height: "stretch" }}
                        onClick={() => navigate("aanmaken")}
                    >
                        Toevoegen
                    </button>
                )}
            </header>

            {loading ? (
                <p className="col-12 text-muted mt-2" aria-busy="true">Laden...</p>
            ) : error ? (
                <p className="col-12 text-muted mt-2">{error}</p>
            ) : dossiers.length === 0 ? (
                <p className="col-12 text-muted mt-2">Er zijn nog geen dossiers gevonden...</p>
            ) : (
                dossiers.map((dossier) => (
                    <article className="col-sm-6 col-md-4 col-lg-3" key={dossier.id}>
                        <OverviewDossierCartComponent id={dossier.id} title={dossier.title} />
                    </article>
                ))
            )}

            <div className="d-sm-none col-12" style={{ height: 70 }} />
            <div className="d-sm-none position-fixed bg-white bottom-0 border-top shadow p-2" style={{ width: "100%", left: 0 }}>
                {hasPermission("dossier:create") && (
                    <button
                        className="btn btn-primary w-100"
                        onClick={() => navigate("aanmaken")}
                    >
                        Toevoegen
                    </button>
                )}
            </div>
        </section>
    );
}

export default Overview;