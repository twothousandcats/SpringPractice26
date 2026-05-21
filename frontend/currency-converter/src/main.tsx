import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import {createBrowserRouter, RouterProvider} from 'react-router-dom'
import {Converter} from "./pages/Converter/Converter.tsx";
import {NotFound} from "./pages/NotFound/NotFound.tsx";
import {CONFIG} from "./utils/config.ts";

const router = createBrowserRouter([
    {path: CONFIG.paths.converter, element: <Converter/>},
    {path: '*', element: <NotFound/>},
]);

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <RouterProvider router={router}/>
    </StrictMode>,
)
