import React from 'react';
import { useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';

import roleFormatter from '../../utils/shared/roleFormatter';

function Header() {
    const { logOut, user } = useAuth();
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const onClickLogOut = () => {
        logOut();
    }

    return (
        <div className="row border-1 border-bottom position-relative align-items-center">
            <div className="col-auto d-md-none">
                <button
                    className="btn bi bi-list text-black"
                    style={{ fontSize: "2rem", padding: "0 0.5rem" }}
                    aria-label="Open menu"
                    aria-expanded={isMenuOpen}
                    onClick={() => setIsMenuOpen(s => !s)}
                />
                <span>Menu</span>
            </div>

            <Link className="col-auto header-item text-black text-decoration-none py-2 m-2 d-none d-md-block" to="/">Home</Link>
            <Link className="col-auto header-item text-black text-decoration-none py-2 m-2 d-none d-md-block" to="/dossiers">Dossiers</Link>

            {isMenuOpen && (
                <div className="d-md-none" style={{ position: 'fixed', inset: 0, backgroundColor: 'rgba(0,0,0,0.5)', zIndex: 1050 }} onClick={() => setIsMenuOpen(false)}>
                    <div style={{ position: 'absolute', top: 0, right: "15%", width: '85%', height: '100%', backgroundColor: '#fff', boxShadow: '-2px 0 8px rgba(0,0,0,0.2)' }} onClick={e => e.stopPropagation()}>
                        <div className="d-flex justify-content-end m-2 border-bottom">
                            <span>Menu</span>
                            <button className="ms-auto btn bi bi-x-lg" aria-label="Close menu" onClick={() => setIsMenuOpen(false)} />
                        </div>
                        <nav className="d-flex flex-column p-3">
                            <Link className="header-item text-black text-decoration-none py-2 px-3" to="/" onClick={() => setIsMenuOpen(false)}>Home</Link>
                            <Link className="header-item text-black text-decoration-none py-2 px-3" to="/dossiers" onClick={() => setIsMenuOpen(false)}>Dossiers</Link>

                            <div className="mt-3 border-top pt-3">
                                <div className="header-item-name">{user?.username}</div>
                                <div className="header-item-role">{roleFormatter(user?.role[0])}</div>
                                <div className="mt-2">
                                    <button className="btn btn-danger w-100 mt-3" onClick={() => { onClickLogOut(); setIsMenuOpen(false); }}>Uitloggen</button>
                                </div>
                            </div>
                        </nav>
                    </div>
                </div>
            )}

            <div className="col-auto ms-auto border-start d-flex flex-column justify-content-center px-3 d-none d-md-flex">
                <div className="header-item-name">{user?.username}</div>
                <div className="header-item-role">{roleFormatter(user?.role[0])}</div>
            </div>
            <button className="btn col-auto bi bi-x-circle-fill d-none d-md-block" style={{ fontSize: "2rem", color: "#00c0ff" }} aria-label="Uitloggen knop" onClick={onClickLogOut} />
        </div>
    );
}

export default Header;