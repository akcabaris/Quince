import React from 'react';


interface Props {
  name: string;
  imgUrl: string;
  description: string;
}

const CardWithDescription: React.FC<Props> = ({ name, imgUrl, description }) => {
    return (
        <>
          <div className="max-w-xs mx-auto pb-1">
          <img src={imgUrl} alt="" className="w-10 h-10 object-cover mx-auto" />

          </div>
          <div className="text-center text-sm text-darkBlue pb-2">{name}</div>
          <div className="text text-base text-center">{description}</div>
        </>
      );
      
      
      
}

export default CardWithDescription;
