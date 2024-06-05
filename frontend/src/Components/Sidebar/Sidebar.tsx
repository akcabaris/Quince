import React, { useState } from "react";
import { Link } from "react-router-dom";
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';
import CampaignIcon from '@mui/icons-material/Campaign';
import EventNoteIcon from '@mui/icons-material/EventNote';
import HelpIcon from '@mui/icons-material/Help';
import StarIcon from '@mui/icons-material/Star';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import SettingsIcon from '@mui/icons-material/Settings';
import HowToRegIcon from '@mui/icons-material/HowToReg';
import LoginIcon from '@mui/icons-material/Login';
import { useAuth } from "../../Context/useAuth";

type SideBarItem = {
  Icon: React.ElementType;
  text: string;
  to: string;
};

type Props = {};

const CustomerItems: SideBarItem[] = [
  { Icon: AccountBoxIcon, text: 'Account', to: '/customer-profile' },
  { Icon: QuestionAnswerIcon, text: 'Messages', to: '/messages' },
  { Icon: EventNoteIcon, text: 'Reservations', to: '/my-reservations' },
];

const WorkerItems: SideBarItem[] = [
  { Icon: AccountBoxIcon, text: 'Account', to: '/worker-profile' },
  { Icon: StarIcon, text: 'Reviews', to: '/reviews' },
  { Icon: QuestionAnswerIcon, text: 'Messages', to: '/messages' },
  { Icon: CampaignIcon, text: 'My Posts', to: '/my-posts' },
];

const NotLoggedInItems: SideBarItem[] = [
  { Icon: HelpIcon, text: 'Help', to: '/help' },
];

const Sidebar = (props: Props) => {
  const { isLoggedIn, user, logout } = useAuth();
  let items: SideBarItem[];
  if (isLoggedIn()) {
    items = user?.accountType === "Worker" ? WorkerItems : CustomerItems;
  } else {
    items = NotLoggedInItems;
  }

  return (
    <div className='hidden md:flex l-0 flex-col bg-gray-200 min-w-60 p-4 fixed left-0 h-full'>
      {items.map((item, index) => (
        <Link to={item.to} replace={true} key={index} className=" border-b border-gray-400 px-4 py-6 h-10 w-auto text-gray-800 hover:bg-gray-400 flex items-center ">
          <item.Icon />
          <span className='ml-2'>{item.text}</span>
        </Link>
      ))}
      {
        isLoggedIn() ? (<div className='relative flex items-center border-b border-gray-400 hover:bg-gray-400'>
          <Link onClick={logout} replace={true}
            to='/'
            className="px-4 py-2 h-10 w-auto text-gray-800 flex items-center "
          >
            🔒 Logout
          </Link>
        </div>) : (<></>)
      }
    </div>
  );
};

export default Sidebar;
