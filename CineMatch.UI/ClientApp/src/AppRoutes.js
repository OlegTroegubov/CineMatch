import {Home} from "./components/Home";
import {Movie} from "./components/Movie";
import Registration from "./components/Registration";
import Login from "./components/Login";

const AppRoutes = [
    {
        index: true,
        element: <Home/>
    },
    {
        path: '/movies',
        element: <Movie/>
    },
    {
        path: '/login',
        element: <Login/>
    },
    {
        path: '/registration',
        element: <Registration/>
    },
];

export default AppRoutes;
