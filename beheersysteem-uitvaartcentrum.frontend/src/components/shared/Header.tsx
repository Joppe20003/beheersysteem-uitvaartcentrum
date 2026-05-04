import { Link } from 'react-router-dom';
import { useState } from 'react';
import { useAuth } from '../../hooks/useAuth';

import roleFormatter from '../../utils/shared/roleFormatter';

function Header() {
    const { logOut, user } = useAuth();
    const [open, setOpen] = useState(false);

    const onClickLogOut = () => {
        logOut();
    }

    return (
        <header>
            <div className="row border-1 border-bottom align-items-center d-none d-md-flex">
                <Link className="col-auto header-item text-black text-decoration-none py-2 m-2" to="/">Home</Link>
                <Link className="col-auto header-item text-black text-decoration-none py-2 m-2" to="/dossiers">Dossiers</Link>
                <div className="col-auto ms-auto border-start d-flex flex-column justify-content-center px-3">
                    <div className="header-item-name" tabIndex={0} >{user?.username}</div>
                    <div className="header-item-role" tabIndex={0} >{roleFormatter(user?.role?.[0])}</div>
                </div>
                <button className="btn col-auto bi bi-x-circle-fill" style={{ fontSize: "2rem", color: "#00c0ff" }} aria-label="Uitloggen knop" onClick={onClickLogOut} />
            </div>

            <div className="d-flex d-md-none align-items-center justify-content-between border-bottom p-2">
                <div className="d-flex align-items-center gap-2">
                    <button aria-label="Menu" className="d-flex gap-2 align-items-center btn border p-1" onClick={() => setOpen(v => !v)}>
                        <i className="bi bi-list" style={{ fontSize: '2rem' }} />
                        <span style={{ fontSize: '1.5rem' }}>Menu</span>
                    </button>
                </div>
            </div>

            {open && (
                <div
                    className="d-md-none"
                    style={{ position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.5)', zIndex: 2000 }}
                    onClick={() => setOpen(false)}
                >
                    <div
                        role="dialog"
                        aria-modal="true"
                        className="bg-white rounded shadow"
                        style={{
                            borderRadius: 0,
                            padding: '1rem'
                        }}
                        onClick={(e) => e.stopPropagation()}
                    >
                        <nav className="d-flex flex-column">
                            <Link to="/" className="py-2 text-decoration-none text-dark" onClick={() => setOpen(false)}>Home</Link>
                            <Link to="/dossiers" className="py-2 text-decoration-none text-dark" onClick={() => setOpen(false)}>Dossiers</Link>

                            <div className="mt-3 pt-2 border-top">
                                <div className="fw-bold" tabIndex={0} >{user?.username}</div>
                                <div className="text-muted small" tabIndex={0} >{roleFormatter(user?.role?.[0])}</div>
                                <button className="btn btn-link mt-2 text-danger p-0" onClick={() => { setOpen(false); onClickLogOut(); }} aria-label="Uitloggen">Uitloggen</button>
                            </div>
                        </nav>
                    </div>
                </div>
            )}
        </header>
    );
}

export default Header;