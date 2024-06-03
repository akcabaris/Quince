import React from 'react';

type ModalProps = {
    title: string;
    description: string;
    onClose: () => void;
    onConfirm: () => void;
};

const Modal: React.FC<ModalProps> = ({ title, description, onClose, onConfirm }) => {
    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white p-4 rounded-lg shadow-lg md:w-1/3 m-2">
                <h2 className="text-lg font-semibold mb-4">{title}</h2>
                <p className="mb-4">{description}</p>
                <div className="flex justify-end gap-4">
                    <button onClick={onClose} className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400">Cancel</button>
                    <button onClick={onConfirm} className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600">Delete</button>
                </div>
            </div>
        </div>
    );
};

export default Modal;
