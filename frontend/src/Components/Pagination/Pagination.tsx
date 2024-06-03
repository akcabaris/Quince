import React, { useState, useEffect } from 'react';

interface Props {
  currentPage: number;
  totalPages: number;
  navigate: (page: number) => void;
}

const Pagination = ({ currentPage, totalPages, navigate }: Props) => {
  const [canGoBack, setCanGoBack] = useState<boolean>(false);
  const [canGoForward, setCanGoForward] = useState<boolean>(false);

  const goBack = () => {
    if (canGoBack) {
      navigate(currentPage - 1);
    }
  }

  const goForward = () => {
    if (canGoForward) {
      navigate(currentPage + 1);
    }
  }

  const goLast = () => {
    navigate(totalPages);
  }

  const goInitial = () => {
    navigate(1);
  }

  useEffect(() => {
    setCanGoBack(currentPage > 1);
    setCanGoForward(currentPage < totalPages);
  }, [currentPage, totalPages]);

  return (<div className='relative'>
    <div className='flex items-center justify-center pt-2'>
      {totalPages > 0 && (
        <>
          <button className="bg-gray-300 rounded-full hover:bg-green-600 text-gray-800 font-bold py-2 px-4 ml-2" onClick={goBack} disabled={!canGoBack}>{"<"}</button>
  
          {currentPage !== 1 && (
            <button className="bg-gray-200 rounded-2xl hover:bg-green-600 text-gray-800 font-bold py-2 px-4 ml-2" onClick={goInitial} disabled={!canGoBack}>{1}</button>
          )}
  
          <button className="bg-green-400 rounded-full text-gray-800 font-bold py-2 px-4 ml-2" disabled={true}>{currentPage}</button>
  
          {currentPage !== totalPages && (
            <button className="bg-gray-200 rounded-2xl hover:bg-green-600 text-gray-800 font-bold py-2 px-4 ml-2" onClick={goLast} disabled={!canGoForward}>{totalPages}</button>
          )}
            
          <button className="bg-gray-300 rounded-full hover:bg-green-600 text-gray-800 font-bold py-2 px-4 ml-2" onClick={goForward} disabled={!canGoForward}>{">"}</button>
        </>
      )}
    </div>
    </div>
  )
  
}

export default Pagination;
