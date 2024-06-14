import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../Context/useAuth';

type Props = { children: React.ReactNode };

const ProtectedRoute = ({ children }: Props) => {
  const location = useLocation();
  const { isLoggedIn } = useAuth();

  if (!isLoggedIn()) {
    return <Navigate to="/login" state={{ from: location }} replace={true} />;
  }

  return <>{children}</>;
};

const ProtectedRouteWorker = ({ children }: Props) => {
  const location = useLocation();
  const { isLoggedIn, user } = useAuth();

  if (!isLoggedIn()) {
    return <Navigate to="/login" state={{ from: location }} replace={true} />;
  }

  if (user != null && user.accountType !== 'Worker') {
    return <Navigate to="/" replace={true} />;
  }

  return <>{children}</>;
};

const ProtectedRouteCustomer = ({ children }: Props) => {
  const location = useLocation();
  const { isLoggedIn, user } = useAuth();

  if (!isLoggedIn()) {
    return <Navigate to="/login" state={{ from: location }} replace={true} />;
  }

  if (user != null && user.accountType !== 'Customer') {
    return <Navigate to="/unauthorized" replace={true} />;
  }

  return <>{children}</>;
};

const ProtectedRouteNotLoggedIn = ({ children }: Props) => {
  const { isLoggedIn } = useAuth();

  if (!isLoggedIn()) { return <>{children}</>; }
  
  return <Navigate to="/" replace={true} />;
}

export { ProtectedRoute, ProtectedRouteWorker, ProtectedRouteCustomer, ProtectedRouteNotLoggedIn };
