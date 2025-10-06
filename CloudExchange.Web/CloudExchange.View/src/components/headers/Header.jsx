import './Header.css'
import logo from '../../assets/logo.svg'

export default function Header({description}) {
    return (
        <div className='header'>
            <a className='header-logo' href='/'>
                <img src={logo} alt='Home'></img>
            </a>
        </div>
    )
}