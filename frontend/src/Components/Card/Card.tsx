import React from 'react';


interface Props {
  name: string;
  imgUrl: string;

}

const Card: React.FC<Props> = ({ name, imgUrl }) => {
  return (
    <div>
      <div className="text-center pb-1 overflow-clip text-lg text-wrap">{name}</div>
      <div className='border   rounded-md'><img src={imgUrl} alt="" className='border rounded' /></div>
    </div>
  );
}

export default Card;
