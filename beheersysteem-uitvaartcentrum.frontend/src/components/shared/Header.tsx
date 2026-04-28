import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';

import roleFormatter from '../../utils/shared/roleFormatter';

function Header() {
    const { logOut, user } = useAuth();

    const onClickLogOut = () => {
        logOut();
    }

    return (
        <div className="row border-1 border-bottom">
            <Link className="col-auto header-item text-black text-decoration-none py-2 m-2" to="/" >Home</Link>
            <Link className="col-auto header-item text-black text-decoration-none py-2 m-2" to="/dossiers" >Dossiers</Link>
            <div className="col-auto ms-auto border-start d-flex flex-column justify-content-center px-3">
                <div className="header-item-name">{ user?.username }</div>
                <div className="header-item-role">{ roleFormatter(user?.role[0]) }</div>
            </div>
            <button className="btn col-auto bi bi-x-circle-fill" style={{ fontSize: "2rem", color: "#00c0ff" }} aria-label="Uitloggen knop" onClick={onClickLogOut} />
        </div>
    );
}

export default Header;