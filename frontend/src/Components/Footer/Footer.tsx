import React from 'react'
import { Link } from 'react-router-dom'

type Props = {}

const Footer = (props: Props) => {
  return (

<footer className="bg-white shadow m-4">
    <div className="w-full max-w-screen-xl mx-auto p-4 md:py-8">
        <div className="sm:flex sm:items-center sm:justify-between">

        </div>
       
        <span className="block text-sm text-gray-500 sm:text-center dark:text-gray-400 just">

          © 2024 <Link to="https://github.com/akcabaris/" className="hover:underline">Quince™</Link>. All Rights Reserved. <br />
          <span>
          {/* /About , /Privacy , /Contact */}
          <Link to="https://github.com/akcabaris/" className="hover:underline pr-5">About</Link>
          <Link to="https://github.com/akcabaris/" className="hover:underline pr-5">Privacy</Link>
          <Link to="https://github.com/akcabaris/" className="hover:underline pr-5">Contact</Link>
          </span>
          </span>
    </div>
</footer>


  )
}

export default Footer