import { useNavigate } from "react-router-dom"

function Overview() {
    const navigate = useNavigate()

    return (
        <section>
            <header className="col pt-2 d-flex">
                <legend>Dossiers overzicht</legend>
                <button className="btn btn-primary" style={{ height: "fit-content" }} onClick={() => navigate("aanmaken")}>
                    Toevoegen
                </button>
            </header>
            <article className="col-md-4">cart</article>
        </section>
    )
}

export default Overview