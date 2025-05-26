import './Close.css'

export default function CloseButton({ onClick }) {
    return (
        <button type="button" className='close-button' onClick={onClick}>
            &times;
        </button>
    )
}