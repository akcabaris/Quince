import { createBrowserRouter } from "react-router-dom";
import HomePage from "../Pages/HomePage/HomePage";
import LoginPage from "../Pages/LoginPage/LoginPage";
import App from "../App";
import RegisterPage from "../Pages/RegisterPage/RegisterPage";
import RegisterPageCustomer from "../Pages/RegisterPage/RegisterPageCustomer";
import RegisterPageWorker from "../Pages/RegisterPage/RegisterPageWorker";
import PostSearchPage from "../Pages/PostPages/PostSearchPage";
import MessagesPage from "../Pages/MessagesPage/MessagesPage";
import { ProtectedRoute, ProtectedRouteCustomer, ProtectedRouteNotLoggedIn, ProtectedRouteWorker } from "./ProtectedRoute";
import CustomerProfilePage from "../Pages/UsersPage/CustomerDetails";
import ReviewsPage from "../Pages/UsersPage/ReviewsPage";
import WorkerDetails from "../Pages/UsersPage/WorkerDetails";
import WorkerPostsPage from "../Pages/PostPages/WorkerPostsPage";
import CustomerReservationsPage from "../Pages/UsersPage/CustomerReservationsPage";
import NotFoundPage from "../Pages/Help/NotFoundPage";



export const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        children: [
            { path: "", element: <HomePage /> },
            { path: "/login", element: <ProtectedRouteNotLoggedIn><LoginPage /></ProtectedRouteNotLoggedIn> },
            { path: "/register", element: <ProtectedRouteNotLoggedIn><RegisterPage /></ProtectedRouteNotLoggedIn> },
            { path: "/register-customer", element: <ProtectedRouteNotLoggedIn><RegisterPageCustomer /> </ProtectedRouteNotLoggedIn> },
            { path: "/register-service-provider", element: <ProtectedRouteNotLoggedIn> <RegisterPageWorker /></ProtectedRouteNotLoggedIn> },
            { path: "/post-search", element: <PostSearchPage /> },
            {
                path: "/messages", element: (
                    <ProtectedRoute>
                        <MessagesPage />
                    </ProtectedRoute>
                )
            },
            { path: "/worker-profile", element: <ProtectedRouteWorker> <WorkerDetails /></ProtectedRouteWorker> },
            { path: "/reviews", element: <ProtectedRouteWorker> <ReviewsPage /></ProtectedRouteWorker> },
            { path: "/my-posts", element: (<ProtectedRouteWorker> <WorkerPostsPage /> </ProtectedRouteWorker>) },


            {
                path: "/customer-profile", element: (
                    <ProtectedRouteCustomer><CustomerProfilePage />
                    </ProtectedRouteCustomer>
                )
            },
            {
                path: "/my-reservations", element: (
                    <ProtectedRouteCustomer><CustomerReservationsPage />
                    </ProtectedRouteCustomer>
                )
            },


        ]
    },
    { path: "*", element: <NotFoundPage /> },
])