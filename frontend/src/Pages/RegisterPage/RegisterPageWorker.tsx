import React, { useState } from 'react'
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useAuth } from "../../Context/useAuth";
import { useForm } from "react-hook-form";


type Props = {}


type RegisterFormsInputs = {
  accountType: string;
  email: string;
  userName: string;
  password: string;
};

const validation = Yup.object().shape({
  accountType: Yup.string().required("Account Type is required").default("Worker"),
  email: Yup.string()
  .required("Email is required")
  .email("invalid email address")
  .lowercase()
  .min(3, "it's too short"),
  userName: Yup.string().required("Username is required").lowercase(),
  password: Yup.string().required("Password is required").min(12, 'Password to short')
  .matches(/^(?=.*[a-z])/, 'Must contain at least one lowercase character')
  .matches(/^(?=.*[A-Z])/, 'Must contain at least one uppercase character')
  .matches(/^(?=.*[0-9])/, 'Must contain at least one number')
  .matches(/^(?=.*[!@#%&,.])/, 'Must contain at least one special character'),
});

const RegisterPageWorker = (props: Props) => {
  const { registerUser } = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormsInputs>({ resolver: yupResolver(validation) });

  const handleRegister = (form: RegisterFormsInputs) => {
    form.accountType="Worker";
    registerUser(form.email, form.userName, form.password, form.accountType);
    setIsButtonDisabled(true);
    setTimeout(() => setIsButtonDisabled(false), 1500);
  };

  const [isButtonDisabled, setIsButtonDisabled] = useState(false);
  

  return (
    <section className='bg-gray-50 '>
      <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0 xl:relative">
      <div className='w-full bg-gray-50 rounded-xl shadow-lg border-gray-600 md:mb-20 sm:max-w-md xl:p-0xl:absolute xl:top-5'>
          <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
            <h1 className='text-xl font-bold leading-normal text-center tracking-tight text-gray-800 md:text-2x1 '>
              Create A Service Provider Account
            </h1>
            <form className='space-y-4 md:space-y-6' onSubmit={handleSubmit(handleRegister)}>

              <div>
                <label
                  htmlFor="email"
                  className="block mb-2 text-sm font-medium text-gray-900 e"
                >
                  Email
                </label>
                <input
                  type="text"
                  id="email"
                  className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  placeholder="Email"
                  {...register("email")}
                />
                {errors.email ? (
                  <p className='text-red-600'>{errors.email.message}</p>
                ) : (
                  ""
                )}
              </div>
              <div>
                <label
                  htmlFor="username"
                  className="block mb-2 text-sm font-medium text-gray-900"
                >
                  Username
                </label>
                <input
                  type="text"
                  id="username"
                  className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 "
                  placeholder="Username"
                  {...register("userName")}
                />
                {errors.userName ? (
                  <p className="text-red-600">{errors.userName.message}</p>
                ) : (
                  ""
                )}
              </div>
              <div>
                <label
                  htmlFor="password"
                  className="block mb-2 text-sm font-medium text-gray-900"
                >
                  Password
                </label>
                <input
                  type="password"
                  id="password"
                  placeholder="••••••••"
                  className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 "
                  {...register("password")}
                />
                {errors.password ? (
                  <p className="text-red-600">{errors.password.message}</p>
                ) : (
                  ""
                )}
              </div>
              <button
                type="submit"
                disabled={isButtonDisabled}
                className='w-full text-white bg-cyan-800 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center '  
                >
                Sign Up
              </button>
            </form>
          </div>
        </div>
      </div>
    </section>
  );
};

export default RegisterPageWorker;