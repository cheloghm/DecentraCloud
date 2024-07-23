import React from 'react';
import './styles/Modal.css';

const Modal = ({ children, onClose, blurBackground }) => {
  return (
    <>
      {blurBackground && <div className="modal-backdrop"></div>}
      <div className="modal">
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>×</button>
          {children}
        </div>
      </div>
    </>
  );
};

export default Modal;
