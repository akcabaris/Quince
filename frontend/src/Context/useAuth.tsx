import React, { createContext, useEffect, useState } from 'react'
import { UserProfile } from '../Models/User'
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { loginAPI, registerAPI } from '../Service/AuthService';
import { toast } from 'react-toastify';



type UserContextType = {
    user: UserProfile | null;
    token: string | null;
    registerUser: (email: string, username: string, password: string, accountType:string) => void;
    loginUser: (username: string, password: string) => void;
    logout: () => void;
    isLoggedIn: () => boolean;
};

type Props = { children: React.ReactNode };

const UserContext = createContext<UserContextType>({} as UserContextType);

export const UserProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [token, setToken] = useState<string | null>(null);
    const [user, setUser] = useState<UserProfile | null>(null);
    const [isReady, setIsReady] = useState(false);

    useEffect(() => {
        const user = localStorage.getItem("user");
        const token = localStorage.getItem("token");
        if (user && token) {
            setUser(JSON.parse(user));
            setToken(token);
            axios.defaults.headers.common["Authorization"] = "Bearer " + token;
        }
        setIsReady(true);
    }, []);

    const loginUser = async (username: string, password: string) => {
        await loginAPI(username, password)
            .then((res) => {
                if (res) {
                    localStorage.setItem("token", res?.data.token);
                    const userObj = {
                        userName: res?.data.userName,
                        email: res?.data.email,
                        accountType: res?.data.accountType,
                    };
                    localStorage.setItem("user", JSON.stringify(userObj));
                    setToken(res?.data.token!);
                    setUser(userObj!);
                    toast.success("Login Success!");
                    axios.interceptors.request.use(
                        (config) => {
                          const token = localStorage.getItem('token');
                          if (token) {
                            config.headers['Authorization'] = 'Bearer ' + token;
                          }
                          return config;
                        },
                        (error) => {
                          return Promise.reject(error);
                        }
                      );
                    navigate("/");
                }
            })
            .catch((e) => toast.warning("Server error occured"));
    };


    const registerUser = async (email: string, username: string, password: string, accountType: string) => {
        await registerAPI(email, username, password, accountType).then((res) => {
            if (res) {
                localStorage.setItem("token", res?.data.token);
                const userObj = {
                    userName: res?.data.userName,
                    email: res?.data.email,
                    accountType: res?.data.accountType,
                };
                localStorage.setItem("user", JSON.stringify(userObj));
                setToken(res?.data.token!);
                setUser(userObj);
                toast.success("Successfully signed up");
                axios.interceptors.request.use(
                    (config) => {
                      const token = localStorage.getItem('token');
                      if (token) {
                        config.headers['Authorization'] = 'Bearer ' + token;
                      }
                      return config;
                    },
                    (error) => {
                      return Promise.reject(error);
                    }
                  );
                navigate("/");
            }
        }).catch((e) => toast.warning("Server error occured when singin up"));
    };

    const isLoggedIn = () => {
        return !!user;
    };

    const logout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        setUser(null);
        setToken("");
        navigate("/");
    }

    return (
        <UserContext.Provider
            value={{loginUser, user, token, logout, isLoggedIn, registerUser}}
        >
            {isReady ? children : null}
        </UserContext.Provider>
    );
};

export const useAuth = () => React.useContext(UserContext);