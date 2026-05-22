// import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import {createBrowserRouter, RouterProvider} from 'react-router-dom'
import {Main} from "./pages/Main/Main.tsx";
import {NotFound} from "./pages/NotFound/NotFound.tsx";
import {CONFIG} from "./utils/config.ts";

const router = createBrowserRouter([
    {path: CONFIG.paths.main, element: <Main/>},
    {path: '*', element: <NotFound/>},
]);

createRoot(document.getElementById('root')!).render(
    //<StrictMode>
    <RouterProvider router={router}/>
    //</StrictMode>,
)