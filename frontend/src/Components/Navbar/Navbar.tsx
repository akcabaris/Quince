import React, { useEffect, useState } from 'react'
import { useAuth } from '../../Context/useAuth'
import { Link } from 'react-router-dom';
import Dropdown from '../Dropdown/Dropdown';
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';
import CampaignIcon from '@mui/icons-material/Campaign';
import SearchIcon from '@mui/icons-material/Search';
import EventNoteIcon from '@mui/icons-material/EventNote';
import HowToRegIcon from '@mui/icons-material/HowToReg';
import LoginIcon from '@mui/icons-material/Login';
import HelpIcon from '@mui/icons-material/Help';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import SettingsIcon from '@mui/icons-material/Settings';
import NavbarItem from './NavbarItem';
import StarIcon from '@mui/icons-material/Star';
import Menu from '../Menu/Menu';

type Props = {}

const DropdownItemsCustomer = [
  { Icon: AccountBoxIcon, text: 'Account', to: '/customer-profile' },
  { Icon: QuestionAnswerIcon, text: 'Messages', to: '/messages' },
  { Icon: EventNoteIcon, text: 'Reservations', to: '/my-reservations' },
];


const DropdownItemsWorker = [
  { Icon: AccountBoxIcon, text: 'Account', to: '/worker-profile' },
  { Icon: StarIcon, text: 'Reviews', to: '/reviews' },
  { Icon: QuestionAnswerIcon, text: 'Messages', to: '/messages' },
  { Icon: CampaignIcon, text: 'My Posts', to: '/my-posts' },
];


const NotLoggedInItems = [
  { Icon: LoginIcon, text: 'Login', to: '/login' },
  { Icon: HowToRegIcon, text: 'Register', to: '/register' },
]

const Navbar = (props: Props) => {
  const { isLoggedIn, user, logout } = useAuth();


  return (
    <nav className='bg-white sticky top-0 z-10 container mx-auto p-6 md:text-sm lg:text-md max-w-full'>
      <div className='flex place-items-center justify-between'>

        <div className='flex items-center xl:space-x-20 font-bold'>
          <Link to="/" className="flex items-center text-black no-underline mr-6" replace={true}>
            <img src="/favicon.ico" alt="Quince" width="48" height="48" className="mr-2" />
            <span>Quince</span>
          </Link>
          <NavbarItem Icon={SearchIcon} text='Search' to='/post-search' />
        </div>

        <>
          {isLoggedIn() ? (
            <>
              <div className=' lg:flex'>

              </div>
              {
                user?.accountType == "Worker" ?
                  (<Dropdown items={DropdownItemsWorker} logout={logout} isLoggedIn={isLoggedIn()} />) :
                  (<Dropdown items={DropdownItemsCustomer} logout={logout} isLoggedIn={isLoggedIn()} />)
              }

            </>
          ) : (<>
            <div className='hidden lg:flex'>
              {
                NotLoggedInItems.map((item, index) => (
                  <NavbarItem
                    key={index}
                    Icon={item.Icon}
                    text={item.text}
                    to={item.to}
                  />
                ))
              }

            </div>
            <div className='flex lg:hidden'>
              <Dropdown items={NotLoggedInItems} logout={logout} isLoggedIn={isLoggedIn()} />
            </div>
          </>
          )}
        </>

      </div>
    </nav>

  );

}

export default Navbar