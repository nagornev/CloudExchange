import './UploadModal.css'
import { API } from '../consts/api';
import Action from "../components/buttons/Action";
import Input from "../components/inputs/Input";
import ModalSection from "../sections/ModalSection";
import { useState } from "react";

export default function UploadModal({ open, onClose }) {
    const [file, setFile] = useState();
    const [lifetime, setLifetime] = useState();
    const [download, setDownload] = useState();
    const [root, setRoot] = useState();
    const [response, setResponse] = useState();

    async function uploadFile() {
        const form = getForm()
        const response = await fetch(API.endpoints.file, {
            method: 'POST',
            body: form,
        })

        setResponse(await response.json());
    }

    function getForm() {
        const form = new FormData();

        file ? form.append("File", new Blob([file]), file.name) : undefined;
        lifetime ? form.append("Lifetime", lifetime) : undefined;
        download ? form.append("Download", download) : undefined;
        root ? form.append("Root", root) : undefined;

        return form;
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