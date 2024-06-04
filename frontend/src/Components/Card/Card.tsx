import React from 'react';
import SearchIcon from '@mui/icons-material/Search';

interface Props {
  name: string;
  imgUrl: string;
  countOfSearch: number;
  countOfPost: number;

}

const Card: React.FC<Props> = ({ name, imgUrl, countOfSearch, countOfPost }) => {
  return (
    <div>
      <div className="text-center pb-1 overflow-clip text-lg text-wrap">{name}</div>
      <div className='border   rounded-md'><img src={imgUrl} alt="" className='border rounded' /></div>
      <div className="text-center pb-1 flex overflow-clip justify-evenly text-sm text-gray-500 text-wrap">
        <p className='justify-start font-sans'><SearchIcon /> {countOfSearch}</p>
        <p className='justify-end font-sans'>{countOfPost} post</p>
      </div>
    </div>
  );
}

export default Card;
