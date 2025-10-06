import './DeleteModal.css'
import { useState } from 'react'
import Input from '../components/inputs/Input'
import ModalSection from '../sections/ModalSection'
import Action from '../components/buttons/Action';
import { API } from '../consts/api';


export default function DeleteModal({ open, onClose }) {
    const [fileId, setFileId] = useState();
    const [root, setRoot] = useState();
    const [response, setResponse] = useState();

    async function deleteFile() {
        const url = getUrl()
        const body = getBody();
        const response = await fetch(url, {
            method: "DELETE",
            body: body,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        setResponse(response.ok? 
                        await response.json():
                        response.status != 401?
                            await response.json():
                            {success : false,
                             error: {
                                message: "Unauthorized."
                             }
                            });
    }

    function getUrl() {
        return API.endpoints.file;
    }

    function getBody() {
        return JSON.stringify(
            {
                descriptorId: fileId,
                root: root
            }
        )
    }

    return (
        <ModalSection open={open} onClose={onClose}>
            <div className='delete-modal-form' style={{ display: response ? "none" : "flex" }}>
                <form className='delete-modal-form-workspace'>
                    <Input placeholder='File ID' value={fileId} onChange={(event) => setFileId(event.target.value)} />
                    <Input placeholder='Root password' value={root} onChange={(event) => setRoot(event.target.value)} />
                </form>

                <Action onClick={() => deleteFile()}>Delete</Action>
            </div>

            <div className='delete-modal-response' style={{ display: response ? "flex" : "none" }}>
                <div className='delete-modal-response-workspace'>
                    <h2 className={'delete-modal-response-' + response?.success}>
                        {response?.success ? "Delete successful!" : "Delete failure."}
                    </h2>

                    <p>
                        {response?.success ? `The file ${fileId} has been deleted!` : response?.error?.message}
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