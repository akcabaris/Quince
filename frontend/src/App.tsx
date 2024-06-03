import React from 'react';
import logo from './logo.svg';
import './App.css';
import { Outlet } from 'react-router';
import { ToastContainer } from 'react-toastify';
import "react-toastify/dist/ReactToastify.css";
import { UserProvider } from './Context/useAuth';
import Navbar from './Components/Navbar/Navbar';
import Footer from './Components/Footer/Footer';
import Sidebar from './Components/Sidebar/Sidebar';




function App() {
  return (
    <UserProvider>
      <div className="flex flex-col min-h-screen">
        <Navbar />
        <div className="flex flex-1">
          <Sidebar />
          <main className="flex-1 p-4 md:ml-60">
            <Outlet />
            <Footer />
          </main>
        </div>
        
      </div>
      <ToastContainer />
    </UserProvider>
  );
}

export default App;