import CloseButton from '../components/buttons/Close'
import './ModalSection.css'
import { createPortal } from 'react-dom'
import { useRef, useEffect } from 'react'

export default function ModalSection({open, onClose, children }) {
    const dialog = useRef()

    useEffect(() => open ? dialog.current.showModal() : dialog.current.close(), [open])

    return createPortal(
        <dialog ref={dialog}>
            <div className='modal'>
                <CloseButton onClick={() => onClose()} />

                <div className='modal-workspace'>
                    {children}
                </div>
            </div>
        </dialog>,
        document.getElementById("root")
    )
}