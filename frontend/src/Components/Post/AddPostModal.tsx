import React, { useEffect, useRef, useState } from 'react';
import { CreatePostAPI } from '../../Service/PostService';
import { useForm } from 'react-hook-form';
import { AddPost } from '../../Models/Post';
import { handleError } from '../../Helpers/ErrorHandler';
import { toast } from 'react-toastify';
import categories from '../../Arguments/category.json';
import cities from '../../Arguments/cities.json';
import currencyList from '../../Arguments/currencyList.json';
import workUnits from '../../Arguments/workUnits.json';
import * as Yup from "yup";
import { yupResolver } from '@hookform/resolvers/yup';

type Props = {
  onClose: () => void; 
  handleGetUserPosts: () => void;
};

const validation = Yup.object().shape({
  category: Yup.string().required("Category is required"),
  title: Yup.string().required("Title is required"),
  description: Yup.string().required("Description is required"),
  price: Yup.number().required("Price is required"),
  priceCurrency: Yup.string().required("Currency is required"),
  priceWorkUnit: Yup.string().required("Work unit is required"),
});

const AddPostModal = ({ onClose, handleGetUserPosts }: Props) => {
  const [city, setCity] = useState<string>("");
  const [county, setCounty] = useState<string>("");
  const [isButtonDisabled, setIsButtonDisabled] = useState<boolean>(false);
  const [cityError, setCityError] = useState<string | null>();
  const [countyError, setCountyError] = useState<string | null>();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset
  } = useForm<AddPost>({ resolver: yupResolver(validation) });

  const handleCreatePost = async (formData: AddPost) => {
    setIsButtonDisabled(true);
    if (city.length < 1) { setCityError("Please select the city"); return; }
    if (county.length < 1) { setCountyError("Please select the county"); return; }
    try {
      const response = await CreatePostAPI(formData, city, county);
      if (response?.status === 204) {
        toast.success('Post created successfully');
      } else if (typeof response?.data === 'string' && response?.status === 200) {
        toast.warn(response.data);
      } else if (typeof response?.data === 'string' && response?.status === 400) {
        toast.error(response?.data);
      } else {
        toast.error("Bad Request");
      }
    } catch (error) {
      handleError(error);
    }
    formRef.current?.reset();
    onClose();
    handleGetUserPosts();
  };

  const handleCancel = () => {
    reset();
    onClose();
  };

  const [countyList, setCountyList] = useState<string[]>([]);
  const countyRef = useRef<HTMLSelectElement>(null);
  const counties: Record<string, string[]> = require('../../Arguments/counties.json');
  const formRef = useRef<HTMLFormElement>(null);



  const handleCityChange = async () => {
    if (city != null && city.length > 0) {
      setCountyList(counties[city]);
      setCounty("");

    }
  }

  useEffect(() => {
    setCounty("");
    handleCityChange();
  }, [city])


  return (
<div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <form
        onSubmit={handleSubmit(handleCreatePost)}
        className="flex flex-col bg-white p-8 rounded-lg justify-center text-center shadow-lg md:w-1/3 m-2 space-y-4"
      >
        <h1 className="text-xl font-semibold mb-4">Create Post</h1>

        <select id="category"
          required={true}
          className="border border-gray-300 rounded p-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          {...register('category')}
          defaultValue={"Select a Category"}
        >          <option value="">Select a Category</option>
          {categories.map((category, index) => (
            <option key={index} value={category}>{category}</option>
          ))}
        </select>
        {errors.category && <p className="text-red-500 text-sm mt-1">{errors.category.message}</p>}

        <select required={true} value={city} size={1} onChange={(e) => setCity(e.target.value)} className="border border-gray-300 rounded-md py-2 px-4 mb-2">
          <option value="">Select a city</option>
          {cities.map((city, index) => (<option key={index} value={city}>{city}</option>))}
        </select>
        {cityError && <p className="text-red-500 text-sm mt-1">{cityError}</p>}

        {countyList != null && (
          <select value={county} required={true} size={1} onChange={(e) => setCounty(e.target.value)} className=" border border-gray-300 rounded-md py-2 px-4 mb-2">
            <option value={""}>Select a County</option>
            {countyList.map((county, index) => (<option key={index} value={county}>{county}</option>))}
          </select>
        )}
        {countyError && <p className="text-red-500 text-sm mt-1">{countyError}</p>}

        <div className="flex flex-roww-full space-x-2 p-1 items-center text-sm  rounded p-2focus:outline-none focus:ring-2 focus:ring-blue-500">
          <span>Price:</span>
          <input id='price' required={true} type="number" min={0} max={2147483646} {...register('price')} placeholder='Price...'
            className='w-1/3 border border-gray-300 rounded p-2 focus:outline-none focus:ring-2 focus:ring-blue-500 text-end' />
            <span>-</span>
          <select id="" {...register('priceCurrency')} size={1} required={true} className='h-10 overflow-y-auto w-1/3 text-sm border border-gray-300 rounded p-2 focus:outline-none focus:ring-2 focus:ring-blue-500'>
            {currencyList.map((currency, index) => (<option key={index} value={currency}>{currency}</option>))}
          </select>
          <span>/</span>
          <select id=""  {...register('priceWorkUnit')} size={1} required={true} className='w-1/3 border border-gray-300 rounded p-2 focus:outline-none focus:ring-2 focus:ring-blue-500'>
          {workUnits.map((workUnit, index) => (<option key={index} value={workUnit}>{workUnit}</option>))}
          </select>
        </div>
        {errors.price && <p className="text-red-500 text-sm mt-1">{errors.price.message}</p>}
        {errors.priceCurrency && <p className="text-red-500 text-sm mt-1">{errors.priceCurrency.message}</p>}
        {errors.priceWorkUnit && <p className="text-red-500 text-sm mt-1">{errors.priceWorkUnit.message}</p>}

        <div className="flex flex-col items-start">
          <label htmlFor="title" className="mb-2 text-sm font-medium text-gray-900">Title:</label>
          <input id="title" minLength={50} maxLength={120} required={true} type="text" {...register('title')} className="border border-gray-300 rounded p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>
        {errors.title && <p className="text-red-500 text-sm mt-1">{errors.title.message}</p>}

        <textarea
          id="description"
          {...register('description')}
          placeholder="Write a description about this post..."
          rows={4}
          required={true}
          maxLength={2000}
          className="border border-gray-300 rounded p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        {errors.description && <p className="text-red-500 text-sm mt-1">{errors.description.message}</p>}
        <div className="flex justify-end gap-4">
          <button onClick={handleCancel} type="button" className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400">
            Cancel
          </button>
          <button type="submit" disabled={isButtonDisabled} className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600">
            Create
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddPostModal;
