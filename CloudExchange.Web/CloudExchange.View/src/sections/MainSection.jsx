import './MainSection.css'
import Header from "../components/headers/Header"

export default function Main({children}) {
    return (
        <main>
            <Header />

            <div className='workspace'>
                {children}
            </div>
        </main>
    )
}