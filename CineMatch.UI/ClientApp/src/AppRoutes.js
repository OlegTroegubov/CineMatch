import {Home} from "./components/Home";
import {Movie} from "./components/Movie";

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
