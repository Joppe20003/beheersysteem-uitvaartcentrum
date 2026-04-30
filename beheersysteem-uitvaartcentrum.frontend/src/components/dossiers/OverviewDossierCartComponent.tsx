import { Link } from 'react-router-dom'; 

type OverviewDossierCardProps = { 
    id: string; 
    title: string; 
}; 

function OverviewDossierCardComponent({ id, title }: OverviewDossierCardProps) { 
    return ( 
        <Link to={`view/${id}`} className="d-block text-center text-decoration-none border mt-2 p-2 pb-4" > 
            <i className="bi bi-folder-fill text-warning text-center" style={{ fontSize: "5rem"}} /> 
            <h5 className="fw-normal text-black" style={{ textWrap: "nowrap", overflow: "hidden", textOverflow: "ellipsis" }} >{title}</h5>
        </Link> 
    ); 
} 

export default OverviewDossierCardComponent;