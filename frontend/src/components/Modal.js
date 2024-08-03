import React, { useEffect, useRef } from 'react';
import './styles/Modal.css';

const Modal = ({ children, onClose, blurBackground }) => {
  const modalRef = useRef(null);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (modalRef.current && !modalRef.current.contains(event.target)) {
        onClose();
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [onClose]);

  return (
    <>
      {blurBackground && <div className="modal-backdrop"></div>}
      <div className="modal" ref={modalRef}>
        <div className="modal-content">
          <button className="close-button" onClick={onClose}>×</button>
          {children}
        </div>
      </div>
    </>
  );
};

export default Modal;
