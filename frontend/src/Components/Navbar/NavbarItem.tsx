import React from 'react'
import { Link } from 'react-router-dom';

type Props = {
    to: string;
    text: string;
    Icon: React.ElementType;
}

const NavbarItem = ({to, text, Icon}: Props) => {
  return (
    <Link to={to} className="flex items-center mr-6 text-black no-underline hover:text-green-800" replace={true}>
        {Icon && <Icon className=""></Icon>}
        <p>{text}</p>
  </Link>
  )
}

export default NavbarItem