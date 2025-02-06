import './App.css'
import Main from '../sections/MainSection'
import ServicesSection from '../sections/ServicesSection'
import { useState } from 'react'
import { DeleteService, DownloadService, UploadService } from '../consts/services';
import UploadModal from '../modals/UploadModal';
import DownloadModal from '../modals/DownloadModal';
import DeleteModal from '../modals/DeleteModal';

export default function App() {
  const [service, setService] = useState();

  return (
    <Main>
      <div className='welcome'>
        <h1>Welcome to Cloud Exchange!</h1>
      </div>

      <ServicesSection onSelect={setService} />
      
      <UploadModal open={service instanceof UploadService} onClose={()=>setService()}/>
      <DownloadModal open={service instanceof DownloadService} onClose={()=>setService()}/>
      <DeleteModal open={service instanceof DeleteService} onClose={()=>setService()}/>
    </Main>
  )
}
