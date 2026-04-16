import { useAlert } from '../../hooks/useAlert';

function Alert() {
    const { alert, clearAlert } = useAlert();

    if (!alert) return null;

    return (
        <div className={`alert alert-${alert.type} alert-dismissible position-absolute col-md-4`} style={{ right: 10, top: 10 }} role="alert" aria-label="Melding sluiten">
            {alert.message}
            <button type="button" className="btn-close" onClick={clearAlert} />
        </div>
    );
}

export default Alert;