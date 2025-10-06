import service from '../../assets/service.svg'
import './Service.css'

export default function Service({name, description, onClick}){
    return (
        <div className="service">
            <div className="service-header" onClick={() => onClick(name)}>
                <img className='service-header-image' src={service}/>
                <h2 className='service-header-name'>{name}</h2>
            </div>
            <div className="service-description">
                <p>{description}</p>
            </div>
        </div>
    )
}