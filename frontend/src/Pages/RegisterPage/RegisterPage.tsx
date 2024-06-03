import React from 'react';
import { Link } from 'react-router-dom';

const RegisterPage = () => {
  return (
    <div className="flex justify-center mt-8 w-4/5 mx-auto flex-wrap md:flex-nowrap">
      <div className="flex w-full md:w-2/5 border rounded-lg overflow-hidden hover:shadow-xl hover:bg-slate-200 transition duration-300 ease-in-out mb-4 md:mb-0 md:mr-4">
        <Link to="/register-customer" className="w-1/2 p-6 block">
          <h2 className="text-lg font-bold mb-4 text-center">Join as a Customer</h2>
          <p className="text-sm text-center">By joining Quince.com, you can easily find the services you need and discover professional service providers to meet your needs.</p>
        </Link>
        <div className="w-1/2">
          <div className="aspect-w-16 aspect-h-9">
            <img src="/img/customer.png" className="object-cover object-center" alt="Join as a Customer" />
          </div>
        </div>
      </div>
      <div className="flex w-full md:w-2/5 border rounded-lg overflow-hidden hover:shadow-xl hover:bg-slate-200 transition duration-300 ease-in-out">
        <div className="w-1/2">
          <div className="aspect-w-16 aspect-h-9">
            <img src="/img/worker.png" className="object-cover object-center" alt="Join us as a Service Provider" />
          </div>
        </div>
        <Link to="/register-service-provider" className="w-1/2 p-6 block">
          <h2 className="text-lg font-bold mb-4 text-center">Join us as a Service Provider</h2>
          <p className="text-sm text-center">Quince.com receives thousands of service requests from customers every year. If you are good at what you do and you want to grow your business, join us.</p>
        </Link>
      </div>
    </div>
  );
  
};

export default RegisterPage;
