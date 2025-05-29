import './DownloadModal.css'
import ModalSection from '../sections/ModalSection'
import { useState } from 'react'
import Action from '../components/buttons/Action';
import Input from "../components/inputs/Input";
import { API } from '../consts/api';
import { BlobExtensions } from '../extensions/BlobExtensions';

export default function DownloadModal({ open, onClose }) {
    const [fileId, setFileId] = useState();
    const [download, setDownload] = useState();
    const [response, setResponse] = useState();

    async function downloadFile() {
        const url = getUrl();
        const response = await fetch(url);

        setResponse(response.ok ? 
                        await saveFile(response) : 
                        response.status != 401?
                            await response.json():
                            {success : false,
                             error: {
                                message: "Unauthorized."
                             }
                            });
    }

    function getUrl() {
        return `${API.endpoints.file}/${fileId}` + (download ? `?download=${download}` : "");
    }

    async function saveFile(response) {
        const name = getName(response);
        const blob = await response.blob();
        return BlobExtensions.saveFile(name, blob);
    }

    function getName(response) {
        return response.headers.get('Content-Disposition')
            .match(/filename[^;=\n]*=["']?([^;"'\n]+)/)?.[1] ?? "default";
    }

    return (
        <ModalSection open={open} onClose={onClose}>
            <div className='download-modal-form' style={{ display: response ? "none" : "flex" }}>
                <form className='download-modal-form-workspace'>
                    <Input placeholder="File ID" value={fileId} onChange={(event) => setFileId(event.target.value)}></Input>
                    <Input placeholder="Download password" value={download} onChange={(event) => setDownload(event.target.value)}></Input>
                </form>

                <Action onClick={() => downloadFile()}>Download</Action>
            </div>

            <div className='download-modal-response' style={{ display: response ? "flex" : "none" }}>
                <div className='download-modal-response-workspace'>
                    <h2 className={'download-modal-response-' + response?.success}>
                        {response?.success ? "Download successful!" : "Download failure."}
                    </h2>
                    
                    <p>
                        {response?.success ? `The file ${fileId} has been downloaded!` : response?.error?.message}
                    </p>
                </div>

                <Action onClick={response?.success ?
                    () => {
                        setResponse();
                        onClose();
                    } :
                    () => setResponse()
                }
                >{response?.success ? "Ok" : "Back"}</Action>
            </div>

        </ModalSection>
    )
}