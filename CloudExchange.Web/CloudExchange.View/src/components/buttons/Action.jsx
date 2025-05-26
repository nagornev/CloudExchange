import './Action.css'

export default function Action({ children, onClick, ...props}) {
    return (
        <button type='button' className='action-button' onClick={onClick} {...props}>
            {children}
        </button>
    )
}