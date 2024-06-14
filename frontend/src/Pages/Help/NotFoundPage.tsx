import React from 'react'
import { Link } from 'react-router-dom'
import DoNotDisturbAltIcon from '@mui/icons-material/DoNotDisturbAlt';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';

type Props = {}

const NotFoundPage = (props: Props) => {
  return (
<div className="bg-gray-800 text-white min-h-screen flex flex-col items-center justify-center">
    <DoNotDisturbAltIcon  fontSize='large'/>
  <h1 className="text-4xl font-bold mb-4">404 - Page Not Found</h1>
  <p className="text-lg mb-6">
    The page you are looking for might have been removed, had its name
    changed, or is temporarily unavailable.
  </p>
  <svg className="animate-bounce w-6 h-6 ..."> <ArrowDownwardIcon /></svg>
  <Link to="/" className="text-blue-400 hover:underline">Go back to the home page</Link>
</div>

  )
}

export default NotFoundPage