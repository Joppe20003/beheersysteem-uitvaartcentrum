import React from "react";

interface DialogProps {
    isOpen: boolean;
    title?: string;
    onClose: () => void;
    children?: React.ReactNode;
    footer?: React.ReactNode;
    size?: "sm" | "md" | "lg" | "xl";
}

function getDialogClass(size?: string) {
    switch (size) {
        case "sm":
            return "modal-sm";
        case "lg":
            return "modal-lg";
        case "xl":
            return "modal-xl";
        default:
            return "modal-md";
    }
}

export default function Dialog({ isOpen, title, onClose, children, footer, size }: DialogProps) {
    if (!isOpen) return null;

    return (
        <div className="modal d-block" tabIndex={-1} role="dialog" aria-modal="true" style={{ backgroundColor: "rgba(0,0,0,0.5)" }} onClick={onClose}>
            <div className={`modal-dialog modal-dialog-centered ${getDialogClass(size)}`} role="document" onClick={(e) => e.stopPropagation()}>
                <div className="modal-content">
                    <div className="modal-header">
                        {title && <h5 className="modal-title">{title}</h5>}
                        <button type="button" className="btn-close" aria-label="Sluiten" onClick={onClose}></button>
                    </div>
                    <div className="modal-body">{children}</div>
                    {footer && <div className="modal-footer">{footer}</div>}
                </div>
            </div>
        </div>
    );
}
