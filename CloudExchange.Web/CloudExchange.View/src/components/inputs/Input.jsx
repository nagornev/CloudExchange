import './Input.css'

export default function Input({childern, ...props}){
    return (
        <input {...props}>{childern}</input>
    )
}