import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { handleError } from '../../Helpers/ErrorHandler';
import categories from '../../Arguments/category.json';
import { BsSearch } from "react-icons/bs";
import { QualityTileData, TrendCategoryData } from '../../Data';
import Tiles from '../../Components/Card/Card';
import TileWithDescription from '../../Components/Card/CardWithDescription';


type Props = {};

const HomePage = (props: Props) => {
  const [category, setCategory] = useState<string>("");
  const categoryNavigate = useNavigate();

  const handleClick = () => {
    try {
      handleSend(category, categoryNavigate);
    } catch (error) {
      handleError(error);
    }
  }

  const handleSend = (categoryF: string, categoryNavigate: any) => {
    categoryNavigate('/post-search', {
      state: {
        category: categoryF,
      }
    });
  };

  const handleTileClick = (topCategory: string) => {
    setCategory(topCategory);
    handleSend(topCategory, categoryNavigate);
  }

  return (
    <div className="mx-auto w-4/5 min-w-fit md:min-w-180 font-serif">
      <div>
        <h1 className="text-center text-2xl font-serif text-gray-800 mb-8 px-1 pt-5 pb-1">
        The easiest way to obtain the service you desire is to simply search for what you need.
        </h1>
      </div>
      <div className="flex rounded-md overflow-hidden border max-w-180 items-center mt-3 w-1/2 mx-auto">
      
        <select
          value={category}
          onChange={(e) => setCategory(e.target.value)}
          className="w-full border-cyan-700 rounded-md px-4"
        >
          <option value="">All Categories</option>
          {categories.map((category, index) => (
            <option key={index} value={category}>
              {category}
            </option>
          ))}
        </select>
        <button
          onClick={handleClick}
          className="bg-indigo-600 text-white px-6 text-lg font-semibold py-4 rounded-r-md"
        >
          <BsSearch />
        </button>
      </div>
      <h1 className="text-center text-2xl font-bold text-gray-800 mb-8 mt-20">
        Trend Services
      </h1>
      <div className="flex flex-col lg:grid lg:grid-cols-3 gap-4 w-4/5 mx-auto">
      {TrendCategoryData != null && TrendCategoryData.map((category, index) => (
      <div key={index} className="p-2 border rounded-md cursor-pointer text-xl hover:bg-gray-100 transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 duration-300"
       onClick={() => handleTileClick(category.id)}>
          <Tiles name={category.id} imgUrl={category.url} />
        </div>
      ))}
    </div>
    <div className="flex flex-col lg:grid lg:grid-cols-4 gap-4 pt-32 w-full text-xl ">
    {QualityTileData != null && QualityTileData.map((tile, index) => (
      <div key={index} className="p-4 cursor-pointer">
          <TileWithDescription name={tile.id} imgUrl={tile.url} description={tile.caption} />
        </div>
      ))}
    </div>
    </div>
  );
}

export default HomePage;