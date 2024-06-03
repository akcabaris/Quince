import React, { useState } from 'react'
import { Link } from 'react-router-dom';
import MenuIcon from '@mui/icons-material/Menu';

type MenuItem = {
  Icon: React.ElementType;
  text: string;
  to: string;
};

type Props = {
  items: MenuItem[];
};

const Menu = ({ items }: Props) => {
  const [isOpen, setIsOpen] = useState(false);
  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="relative lg:hidden">
      <div className=" flex ml-3" onClick={toggleDropdown}>
        <MenuIcon />
      </div>
      {isOpen && (
        <ul className="absolute top-full z-10 space-y-1 bg-white shadow-md border border-gray-200">
          {items.map((item, index) => (
            <Link to={item.to}
              className="px-4 py-4 h-10 w-auto text-gray-800 hover:bg-gray-200 flex items-center"
            >
              <item.Icon />
                <span className='ml-2'>{item.text}</span>
            </Link>

          ))}
        </ul>
      )}
    </div>
  )
}

export default Menu;