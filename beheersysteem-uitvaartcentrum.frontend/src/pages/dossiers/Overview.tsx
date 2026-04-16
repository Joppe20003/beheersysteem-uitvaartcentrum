import { useNavigate } from "react-router-dom";

import useDossiers from "../../hooks/useDossiers";
import OverviewDossierCartComponent from "../../components/dossiers/OverviewDossierCartComponent";

function Overview() {
    const navigate = useNavigate();
    const { dossiers, error, loading } = useDossiers();

    return (
        <section className="row">
            <header className="col-12 pt-2 d-flex">
                <legend tabIndex={0}>Dossiers overzicht</legend>
                <button className="btn btn-primary d-none d-sm-block" style={{ height: "fit-content" }} onClick={() => navigate("aanmaken")}>
                    Toevoegen
                </button>
            </header>
            {loading && <p className="col-12 text-muted mt-4">Laden...</p>}
            {error && <p className="col-12 text-muted mt-4">{error}</p>}
            {!loading && !error && dossiers.length === 0 && <p className="col-12 text-muted mt-4">Er zijn nog geen dossiers gevonden...</p>}
            {dossiers.map((dossier) => (
                <article className="col-sm-6 col-md-4 col-lg-3" key={dossier.id}>
                    <OverviewDossierCartComponent id={dossier.id} title={dossier.title} />
                </article>)
            )}
            <div className="d-sm-none position-fixed bg-white bottom-0 border-top shadow p-2">
                <button className="btn btn-primary" style={{ width: "stretch" }} onClick={() => navigate("aanmaken")}>
                    Toevoegen
                </button>
            </div>
        </section>
    );
}

export default Overview;