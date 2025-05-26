import './ServicesSection.css'
import { Services } from '../consts/services'
import Service from '../components/services/Service'

export default function ServicesSection({ onSelect }) {
  return (
    <div className='services'>
      {
        Services.map(service => (
          <Service key={service.name} name={service.name} description={service.description} onClick={()=>onSelect(service)}/>
        ))
      }
    </div>
  )
}