import React, { SyntheticEvent, useRef, useState } from 'react';
import { Link } from 'react-router-dom';
import MenuIcon from '@mui/icons-material/Menu';
import useOutsideClick from './useOutsideClick';


type DropdownItem = {
  Icon: React.ElementType;
  text: string;
  to: string;
};

type Props = {
  items: DropdownItem[];
  children?: React.ReactNode;
  isLoggedIn: boolean;
  logout: (e: SyntheticEvent) => void;
};
const Dropdown = ({ items, logout, isLoggedIn }: Props) => {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  useOutsideClick(dropdownRef, () => {
    if (isOpen) {
      setIsOpen(false);
    }
  });

  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="relative md:hidden" ref={dropdownRef}>
      <div className="dropdown-main flex ml-3" onClick={toggleDropdown}>
        <MenuIcon />
      </div>
      {isOpen && (
        <ul className={`absolute top-full z-10 space-y-1 bg-white right-1 shadow-md border border-gray-200`}>
          {items.map((item, index) => (
            <Link
              key={index}
              to={item.to}
              onClick={toggleDropdown}
              className="px-4 py-4 h-10 w-auto text-gray-800 hover:bg-gray-200 flex items-center"
            >
              <item.Icon />
              <span className='ml-2'>{item.text}</span>
            </Link>
          ))}

          {
            isLoggedIn ? (<div className='relative flex items-center'>
              <Link onClick={logout}
                to='/'
                className="px-4 py-2 h-10 w-auto text-gray-800 hover:bg-gray-200 flex items-center"
              >
                🔒 Logout
              </Link>
            </div>) : (<></>)
          }

        </ul>
      )}
    </div>
  );

};

export default Dropdown;