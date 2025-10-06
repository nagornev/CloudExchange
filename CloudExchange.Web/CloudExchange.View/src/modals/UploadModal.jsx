import './UploadModal.css'
import { API } from '../consts/api';
import Action from "../components/buttons/Action";
import Input from "../components/inputs/Input";
import ModalSection from "../sections/ModalSection";
import { useState } from "react";

export default function UploadModal({ open, onClose }) {
    const [file, setFile] = useState();
    const [lifetime, setLifetime] = useState(1000);
    const [download, setDownload] = useState();
    const [root, setRoot] = useState();
    const [response, setResponse] = useState();

    async function uploadFile() {
        const beginUploadFileBody = getBeginUploadFileBody()
        const beginUploadFileResponse = await fetch(API.endpoints.file, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(beginUploadFileBody),
        })

        const beginUploadFile = await beginUploadFileResponse.json();

        const uploadedParts = [];
        const size = 5 * 1024 * 1024; // 5MB
        const parts = Math.ceil(file.size / size);

        for(let i=0;i<parts;i++){
            const start = i * size;
            const end = Math.min(file.size, start + size);
            const blob = file.slice(start, end);

            const continueUploadFileResponse = await fetch(API.endpoints.file, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    id: beginUploadFile.content.id,
                    key: beginUploadFile.content.key,
                    part: i + 1 
                })
            });
            const continueUploadFile = await continueUploadFileResponse.json();

            const uploadResponse = await fetch(continueUploadFile.content.url, {
                method: 'PUT',
                body: blob
            });

            const eTag = uploadResponse.headers.get("ETag");
            uploadedParts.push({number: i + 1 , 
                                tag: eTag });
        }
        
        const completeUploadFileResponse = await fetch(API.endpoints.file, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                id: beginUploadFile.content.id,
                key: beginUploadFile.content.key,
                parts: uploadedParts
            })
        });
        const completeUploadFile = await completeUploadFileResponse.json();

        setResponse({
            success: completeUploadFile.success,
            content:{
                id: beginUploadFile.content.descriptorId
            }
        });
    }

    function getBeginUploadFileBody() {
        return {
            name : file.name,
            weight: file.size,
            lifetime: lifetime,
            root: root,
            download: download
        };
    }

    return (
        <ModalSection open={open} onClose={onClose}>
            <div className='upload-modal-form' style={{ display: response ? "none" : "flex" }}>
                <form className='upload-modal-form-workspace'>
                    <Input type="file" placeholder="Select file" filename={file?.name} onChange={(event) => setFile(event.target.files[0])}></Input>
                    <Input placeholder="Lifetime in seconds" value={lifetime} onChange={(event) => setLifetime(event.target.value)}></Input>
                    <Input placeholder="Download password" value={download} onChange={(event) => setDownload(event.target.value)}></Input>
                    <Input placeholder="Root password" value={root} onChange={(event) => setRoot(event.target.value)}></Input>
                </form>

                <Action onClick={() => uploadFile()}>Upload</Action>
            </div>

            <div className='upload-modal-response' style={{ display: response ? "flex" : "none" }}>
                <div className='upload-modal-response-workspace'>
                    <h2 className={'upload-modal-response-' + response?.success}>
                        {response?.success ? "Uploading successful!" : "Uploading failure."}
                    </h2>

                    <p>{response?.success ?
                        "Use this ID to interact with this file: " + response?.content.id :
                        response?.error.message}
                        {response?.success ?
                            <svg className="copy" onClick={() => navigator.clipboard.writeText(response?.content.id)} xmlns="http://www.w3.org/2000/svg" fill="none" stroke="currentColor" strokeWidth="2" viewBox="0 0 24 24">
                                <rect x="9" y="9" width="13" height="13" rx="2"></rect>
                                <path d="M5 15V5a2 2 0 0 1 2-2h10"></path>
                            </svg> :
                            undefined}
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