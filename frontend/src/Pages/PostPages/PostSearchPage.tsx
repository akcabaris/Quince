import React, { useEffect, useState } from 'react';
import { PostGet } from '../../Models/Post';
import { postGetAPI, postLengthGetAPI } from '../../Service/PostService';
import { handleError } from '../../Helpers/ErrorHandler';
import cities from '../../Arguments/cities.json';
import categories from '../../Arguments/category.json';
import Pagination from '../../Components/Pagination/Pagination';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import ListPost from '../../Components/Post/ListPost';
import { useAuth } from '../../Context/useAuth';
import { useLocation } from 'react-router-dom';
import Spinner from '../../Components/Spinners/Spinner';



const PostSearchPage = () => {

  const [pageCity, setPageCity] = useState("");
  const [pageCounty, setPageCounty] = useState("");
  const [pageCategory, setPageCategory] = useState("");
  const [city, setCity] = useState<string>("");
  const [county, setCounty] = useState<string>("");
  const [category, setCategory] = useState<string>("");
  const [countyList, setCountyList] = useState<string[] | null>();
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [totalPost, setTotalPost] = useState<number>(0);
  const [postValues, setPostValues] = useState<PostGet[] | null>(null);
  const [isSearched, setIsSearched] = useState<boolean>(false);
  const { isLoggedIn } = useAuth();
  const [loading, setLoading] = useState<boolean>();


  const [isButtonDisabled, setIsButtonDisabled] = useState(false);


  const navigate = (currentPage: number) => {
    setCurrentPage(currentPage);
  };

  const location = useLocation();
  useEffect(() => {
    if (location.state && location.state.category) {
      setCategory(location.state.category);
    }
  }, [location.state]);


  const handleSearch = async () => {
    setLoading(true);
    setIsSearched(true);
    try {
      setPageCity(city);
      setPageCounty(county);
      setPageCategory(category);
      const totalPost = await postLengthGetAPI(city, county, category);
      if (totalPost && (totalPost.data > 0)) {
        setTotalPost(totalPost.data);
        const response = await postGetAPI(city, county, category, currentPage);
        if (response && response.data.length > 0) {
          const postData = Array.isArray(response.data) ? response.data : [];
          setPostValues(postData);
        }
      }
      else if (totalPost && (totalPost.data == 0)) {
        setPostValues(null);
        setTotalPost(totalPost.data);
      } else {
        setPostValues(null);

      }
    } catch (error) {
      handleError(error);
      setPostValues(null);
    }
    finally { setLoading(false); }
    setIsButtonDisabled(true);
    setTimeout(() => setIsButtonDisabled(false), 1500);
  }

  const handlePageChange = async () => {
    setLoading(true);
    if (currentPage <= (Math.ceil(totalPost / 20)) ?? currentPage >= 1) {
      try {
        setCity("");
        setCounty("");
        setCategory("");
        const response = await postGetAPI(pageCity, pageCounty, pageCategory, currentPage);
        if (response && response.data) {
          setPostValues(Array.isArray(response.data) ? response.data : []);
          window.scrollTo({ top: 0, behavior: 'smooth' });
        }
        else if (response?.data.length === 0) {
          handleError("No data found.");
          setPostValues(null);
        }
      } catch (error) {
        handleError(error);
        setPostValues(null);
      }
    }
    setLoading(false);
  }
  useEffect(() => {
    handlePageChange();
  }, [currentPage])


  type CountiesType = {
    [key: string]: string[];
  };

  const counties: CountiesType = require('../../Arguments/counties.json');


  const handleListingCounty = async () => {
    if (city != null) {
      setCountyList(counties[city]);
      setCounty("");
    }
  }

  useEffect(() => {
    setCounty("");
    setIsSearched(false);
    handleListingCounty();
  }, [city])

  useEffect(() => {
    setIsSearched(false);
  }, [category, county])



  return (
    <div className=' relative w-full  items-center overflow-auto max-h-full flex flex-col font-serif mt-10 min-h-[500px] xl:min-h-[720px]'>

      <div className=' lg:min-w-52 max-w-180 lg:pt-4  xl:pt-24 xl:mr-3'>
        {!isLoggedIn() ? (<p className='text-center text-red-600 px-2 pt-1 pb-3'>To make a reservation and message with Service Provider Users, you have to log in.</p>) : (<></>)}
        <p className='text-center px-2 pt-1 pb-3'>Get quotes for free and compare. 100.000 professionals are ready for you.</p>
        <div className="flex flex-col items-center mt 10">
          <select value={city} onChange={(e) => setCity(e.target.value)} className="border border-gray-300 rounded-md py-2 px-4 mb-2 w-2/4">
            <option value="">Select a city</option>
            {cities.map((city, index) => (<option key={index} value={city}>{city}</option>))}
          </select>
          {countyList != null && (
            <select value={county} onChange={(e) => setCounty(e.target.value)} className="border border-gray-300 rounded-md py-2 px-4 mb-2 w-2/4">
              <option value="">Select a County</option>
              {countyList.map((county, index) => (<option key={index} value={county}>{county}</option>))}
            </select>
          )}

          <select value={category} onChange={(e) => setCategory(e.target.value)} className="border border-gray-300 rounded-md py-2 px-4 mb-2 w-2/4">
            <option value="">All Categories</option>
            {categories.map((category, index) => (<option key={index} value={category}>{category}</option>))}
          </select>

          <button onClick={handleSearch} className="bg-gradient-to-r from-green-900 to-green-700 disabled:hover:from-green-900 disabled:hover:to-green-700 hover:from-sky-800 hover:to-sky-600 text-white font-bold py-2 px-4 rounded w-1/4"
            disabled={isButtonDisabled}
          >
            Search
          </button>
        </div>
      </div>
      <div className="mx-10 relative items-center mb-8 rounded-lg flex flex-col">
        {
          loading && isSearched ? <Spinner /> : (
            <>
              {postValues && postValues.length > 0 ? (
                <>
                  <ListPost postValues={postValues} totalPost={totalPost} />
                  <Pagination currentPage={currentPage} totalPages={Math.ceil(totalPost / 20)} navigate={navigate} />
                </>
              ) : (
                <>
                  <>{isSearched && !loading && totalPost == 0 ? (
                    <div className="flex flex-col items-center justify-center space-y-4 mt-4">
                      <ArrowUpwardIcon />
                      <p className="text-lg lg:text-2xl text-center text-gray-700"><span>No results found.  Search Again </span></p>

                    </div>
                  ) : (
                    <>
                    </>
                  )}


                  </>
                </>
              )}</>
          )
        }

      </div>

    </div>
  );
}
export default PostSearchPage;