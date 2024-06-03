import React, { useState } from 'react'
import * as Yup from "yup";
import { useAuth } from '../../Context/useAuth';
import { yupResolver } from '@hookform/resolvers/yup'
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';

type Props = {}


type LoginFormsInputs = {
  userName: string;
  password: string;
}

const validation = Yup.object().shape({
  userName: Yup.string().required("Username is required"),
  password: Yup.string().required("Password is required"),
});

const LoginPage = (props: Props) => {
  const { loginUser } = useAuth();
  const [isButtonDisabled, setIsButtonDisabled] = useState(false);
  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm<LoginFormsInputs>({ resolver: yupResolver(validation) });

  const handleLogin = (form: LoginFormsInputs) => {
    loginUser(form.userName, form.password);
    setIsButtonDisabled(true);
    setTimeout(() => setIsButtonDisabled(false), 1500);
  }

  return (
    <section className='bg-gray-50 md'>
      <div className='flex  items-center justify-center  xl:relative px-6 py-3 mx-auto lg: md:h-screen lg:pt lg:py-0'>
        <div className='w-full bg-gray-50 rounded-xl shadow-lg xl:absolute xl:top-5 border-gray-600 leading-5 md:mb-20 sm:max-w-md xl:p-0'>
          <div className='p-6 space-y-4 md:space-y-6 sm:p-8'>
            <h1 className='text-xl font-bold text-center leading-normal tracking-tight text-gray-800 md:text-2x1 '>
              Sign in
            </h1>
            <form className='space-y-4 md:space-y-6' onSubmit={handleSubmit(handleLogin)}>
              <div>
                <label htmlFor="username" className='block mb-2 text-sm font-medium text-gray-900 '>
                  Username
                </label>
                <input type="text" id='username'
                  className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 "
                  placeholder='Username'
                  {...register("userName")}
                />
                {
                  errors.userName ? (
                    <p className='text-red-600'>{errors.userName.message}</p>
                  ) : (
                    ""
                  )
                }
              </div>
              <div>
                <label
                  htmlFor="password"
                  className="block mb-2 text-sm font-medium text-gray-900 "
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
              {/* <div className='flex items-center justify-betwwen'>
              <a
                href='#'
                className='text-sm text-white font-medium text-primary-600 hover:underline dark:text-primary-500'
              >
                Forgot password?
              </a>
            </div> */}
              <button
                type='submit'
                disabled={isButtonDisabled}
                className='w-full text-white bg-cyan-800 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center'
              >
                Sign in
              </button>
              <p
                className='text-sm font-light text-gray-500 '
              >
                Don't have an account yet?
                <Link
                  to={"/register"}
                  className='font-medium ml-2 text-primary-600 underline'
                >
                  Sign Up
                </Link>
              </p>
            </form>
          </div>
        </div>
      </div>
    </section>
  );
};

export default LoginPage;