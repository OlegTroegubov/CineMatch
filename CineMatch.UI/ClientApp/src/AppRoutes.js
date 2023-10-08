import {Home} from "./components/Home";

const AppRoutes = [
    {
        index: true,
        element: <Home/>
    },
    {
        path: '/getMovie',
        element: <Movie/>
    },
];

export default AppRoutes;
