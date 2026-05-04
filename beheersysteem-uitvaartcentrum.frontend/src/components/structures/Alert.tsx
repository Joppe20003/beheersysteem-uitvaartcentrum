import { useEffect, useRef } from 'react';
import { useAlert } from '../../hooks/useAlert';

function Alert() {
    const { alert, clearAlert } = useAlert();
    const alertRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (alert) {
            alertRef.current?.focus();
        }
    }, [alert]);

    if (!alert) return null;

    return (
        <div
            ref={alertRef}
            className={`alert alert-${alert.type} alert-dismissible col-md-4 border border-${alert.type} shadow`}
            style={{ right: 10, top: 10, position: 'fixed', zIndex: 2000 }}
            role="alert"
            tabIndex={0}
        >
            {alert.message}
            <button type="button" className="btn-close" aria-label={`${alert.message}, melding sluiten`} onClick={clearAlert} />
        </div>
    );
}

export default Alert;