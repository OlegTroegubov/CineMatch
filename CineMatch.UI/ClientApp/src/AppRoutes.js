import {Home} from "./components/Home";
import {Movie} from "./components/Movie/Movie";
import Registration from "./components/User/Registration";
import Login from "./components/User/Login";
import Logout from "./components/User/Logout";

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
    {
        path: '/logout',
        element: <Logout/>
    }
];

export default AppRoutes;
