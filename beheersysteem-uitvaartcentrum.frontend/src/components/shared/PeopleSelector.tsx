import { useEffect, useMemo, useState } from "react";
import { dossierService } from "../../services/dossierService";
import { useAlert } from "../../hooks/useAlert";
import { userService } from "../../services/userService";

interface PeopleSelectorProps {
    dossierId?: string;
    onInviteComplete?: () => void;
    onClose?: () => void;
}

type UserOverview = {
    id: string;
    userName?: string | null;
};

function PeopleSelector({ dossierId, onInviteComplete, onClose }: PeopleSelectorProps) {
    const [users, setUsers] = useState<UserOverview[]>([]);
    const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const { showAlert, clearAlert } = useAlert();

    useEffect(() => {
        let mounted = true;
        userService.getAll()
            .then((res) => {
                if (!mounted) return;
                const list = (res || []) as UserOverview[];
                setUsers(list.map(u => ({ id: u.id, userName: u.userName })));
            })
            .catch(() => {
                showAlert("Kan gebruikers niet ophalen.", "danger");
            });
        return () => { mounted = false; };
    }, [showAlert]);

    const filtered = useMemo(() => {
        return users.filter(u => (u.userName || "").toLowerCase());
    }, [users]);

    const handleInvite = async () => {
        if (!dossierId || !selectedUserId) return;
        setLoading(true);
        try {
            await dossierService.invite({ dossierId: dossierId, targetedUserId: selectedUserId });
            clearAlert();
            showAlert("Gebruiker succesvol uitgenodigd.", "success");
            onInviteComplete && onInviteComplete();
        } catch (error: unknown) {
            clearAlert();

            const errorMessage = (error instanceof Error) ? error.message : (typeof error === "object" && error !== null && "message" in error) ? (error as { message: string }).message : null;

            if (errorMessage && errorMessage !== "Failed to fetch") {
                showAlert(errorMessage, "danger");
            } else {
                showAlert("Kan geen mensen uitnodigen, probeer dit later opnieuw", "danger");
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <div style={{ maxHeight: 240, overflowY: "auto" }}>
                {filtered.length === 0 && <div className="text-muted">Geen gebruikers gevonden</div>}
                <ul className="list-group">
                    {filtered.map(user => (
                        <li
                            key={user.id}
                            className={`list-group-item d-flex justify-content-between align-items-center ${selectedUserId === user.id ? 'active' : ''}`}
                            role="button"
                            onClick={() => setSelectedUserId(user.id)}
                        >
                            <div>{user.userName}</div>
                            <div>
                                <input
                                    data-testid={`user-radio-${user.id}`}
                                    type="radio"
                                    name="selectedUser"
                                    checked={selectedUserId === user.id}
                                    onChange={() => setSelectedUserId(user.id)}
                                />
                            </div>
                        </li>
                    ))}
                </ul>
            </div>

            <div className="d-flex gap-2 mt-3">
                <button className="btn btn-secondary" onClick={() => onClose ? onClose() : undefined}>Sluiten</button>
                <button className="btn btn-primary ms-auto" onClick={handleInvite} aria-label="confirm-user-invite-to-dossier" disabled={!selectedUserId || !dossierId || loading}>
                    {loading ? 'Bezig...' : 'Uitnodigen'}
                </button>
            </div>
        </div>
    );
}

export default PeopleSelector;