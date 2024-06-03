import React, { useEffect, useState } from 'react';
import { WorkerPosts, postReservations } from '../../Models/Post';
import { handleError } from '../../Helpers/ErrorHandler';
import { DeletePostAPI, WorkerPostsGetAPI } from '../../Service/PostService';
import { FaAnglesDown, FaAnglesUp } from 'react-icons/fa6';
import { UpdateReservationStatusAPI } from '../../Service/ReservationService';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import { toast } from 'react-toastify';
import AddPostModal from '../../Components/Post/AddPostModal';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import Spinner from '../../Components/Spinners/Spinner';

type Props = {};

const WorkerPostsPage = (props: Props) => {
  const [postValues, setPostValues] = useState<WorkerPosts[] | null>(null);
  const [openIndex, setOpenIndex] = useState<number | null>(null);
  const [addPostModalIsOpen, setAddPostModalIsOpen] = useState<boolean>(false);
  const [deleteModalIsOpen, setDeleteModalIsOpen] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>();

  const handleGetUserPosts = async () => {
    setLoading(true);
    try {
      const userPosts = await WorkerPostsGetAPI();
      if (userPosts && userPosts.length >= 1) {
        setPostValues(userPosts);
      }
    } catch (error) {
      handleError(error);
    }
    setLoading(false);
  };


  const handleUpdateReservationStatus = async (reservationId: number, status: string) => {
    if (reservationId != null && (status == "Approved" || status == "Denied")) {
      try {
        const updateStatus = await UpdateReservationStatusAPI(reservationId, status);
        if (updateStatus) {
          handleGetUserPosts();
        }
      } catch (error) {
        handleError(error);
      }
    } else {
      toast.warn("Something went wrong !!")
    }
  }

  useEffect(() => {
    handleGetUserPosts();
  }, []);

  const handleToggle = (index: number) => {
    setOpenIndex(openIndex === index ? null : index);
  };

  const handleCloseModal = () => {
    setAddPostModalIsOpen(false);
    handleGetUserPosts();
  };

  const handleDeletePost = async (postId: number) => {

    try {
      if (postId != null) {
        const response = await DeletePostAPI(postId);
        if (response) {
          setDeleteModalIsOpen(false);
          handleGetUserPosts();
          toast.warn("Deleted");
        }
      }
    } catch (error) {
      handleError(error);
    }
  }


  return (
    
    loading ? (<Spinner />) : (
      <section id="posts" className="">
      <div className='flex justify-center'> {/* Updated this line */}
        <button onClick={() => { setAddPostModalIsOpen(true) }} className='border  bg-gradient-to-r from-green-700 to-green-600 hover:from-green-600 hover:to-green-500 text-white font-bold py-2 px-4 rounded transition duration-300'><AddIcon/> Create Post</button>
        {addPostModalIsOpen && <AddPostModal onClose={handleCloseModal} handleGetUserPosts={handleGetUserPosts} />}
      </div>
      {postValues && postValues.length > 0 ? (
        <div className="flex flex-col overflow-hidden bg-white items-center justify-center">

          <div className="item-center lg:grid lg:grid-cols-2">
            {postValues.map((postVal, index) => (
              <div
                key={index}
                className="rounded-lg pb-2 px-2 md:mx-3 my-6 bg-gradient-to-r cursor-pointer border-spacing-2 shadow-blue-100/50 relative shadow-xl border max-w-180 min-h-48"
                onClick={() => handleToggle(index)}
              >
                <div className="flex justify-between mt-4 w-full bg-slate-50 items-center rounded-md shadow-md opacity-80 relative">
                  <div className="flex items-center"></div>
                </div>

                <div className="rounded-sm text-center">
                  <div className='flex flex-row justify-between'>
                    <h1 className="text-md font-mono text-darkBlue"><span className='text-red-700'>Title: </span> {postVal.title}</h1>
                    <button onClick={() => { setDeleteModalIsOpen(true) }} className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600">
                      <DeleteIcon />Delete
                    </button>
                    {deleteModalIsOpen ? (
                      <div className="fixed inset-0 z-50 flex items-center justify-center bg-gray-800 bg-opacity-10">
                        <div className="bg-white p-4 rounded-lg shadow-lg md:w-1/3 m-2">
                          <h2 className="text-lg font-semibold mb-4">Are you sure you want to delete the post named "{postVal.title}"?</h2>
                          <div className="flex justify-end gap-4">
                            <button onClick={() => { setDeleteModalIsOpen(false) }} className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400">Cancel</button>
                            <button onClick={() => { handleDeletePost(postVal.postId) }} className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600">Delete</button>
                          </div>
                        </div>
                      </div>
                    ) : (
                      <></>
                    )}
                  </div>
                  <div className="my-4 text-gray-900 font-light text-xs"><span className='text-red-700'>Description: </span>{postVal.description}</div>
                </div>
                <div className="max-h-16 mt-2 text-center text-sm">
                  <p className='text-red-700'>Location</p>
                  <p className="text-gray-700">
                    {postVal.city} - {postVal.county}
                  </p>
                </div>
                <div className="flex mt-1 w-full">
                  <div className="text-right font-thin text-sm flex w-full">
                    <p className="text-green-700 text-center w-2/5"><span className='text-red-700'>Price: </span>{postVal.price}{postVal.priceCurrency} - {postVal.priceWorkUnit}</p>
                  </div>
                </div>
                {openIndex === index && (
                  <div className="px-2 py-4 mt-4 border rounded-md transition-all bg-gray-50 border-spacing-2 duration-300 ease-in-out">
                    <div className="text-gray-900 font-light text-xs space-y-2">
                      <h1 className='text-red-700 mb-2'>Reservations</h1>


                      {postVal.reservList && postVal.reservList.length > 0 ? (
                        postVal.reservList.map((reservation: postReservations, resIndex) => (
                          <div className='border border-gray-400 rounded-lg p-1 shadow-md space-y-3 bg-slate-100 '>
                            <div key={resIndex} className="flex relative items-center justify-between text-sm space-x-2">
                              <p className='text-blue-800 overflow-hidden text-wrap relative items-center justify-normal flex-row '>
                                <img className='w-14 rounded-lg' src={reservation.customerPictureLink === "http://localhost:5279/resources/" ? "/img/profile.png" : reservation.customerPictureLink} alt="" />
                                <span className="truncate">{reservation.customerName}</span>
                              </p>

                              <p className='justify-end'><CalendarMonthIcon />{reservation.reservationDate.slice(0, 10)}</p>
                            </div>
                            <p className='text-sm font-serif overflow-clip text-wrap'>
                              <span className='text-green-800'>Reservation Note: </span>{reservation.reservationNote}
                            </p>
                            <p><span className='text-green-800'>Status:</span>{reservation.status}</p>
                            {reservation.status == "Waiting" ? (
                              <div className='flex flex-row justify-normal space-x-3 text-sm font-serif'>

                                <button onClick={() => { handleUpdateReservationStatus(reservation.reservationId, "Approved") }}
                                  className='justify-end shadow-md bg-gradient-to-r from-green-600 to-green-500 hover:from-green-800 hover:to-green-600 text-white text-sm font-mono p-1 rounded transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 duration-150'>
                                  Approve
                                </button>
                                <button onClick={() => { handleUpdateReservationStatus(reservation.reservationId, "Denied") }}
                                  className='justify-end shadow-md bg-gradient-to-r from-red-600 to-red-500  hover:from-red-800 hover:to-red-700 text-white text-sm font-mono p-1 rounded transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 duration-150'>
                                  Deny
                                </button>
                              </div>
                            ) : (<></>)}
                          </div>
                        ))
                      ) : (
                        <p>No reservation available.</p>
                      )}
                    </div>
                  </div>
                )}

                <div>
                  {openIndex === index ? (
                    <>
                      <FaAnglesUp className="mx-auto text-darkBlue" />
                      <p className='text-center mt-2 text-darkBlue'>Hide Reservations</p>
                    </>
                  ) : (
                    <>
                      <p className='text-center mt-2 text-darkBlue'>See Reservations
                        {postVal.countOfWaitingReservation > 0 && (
                          <>
                            <span className="absolute ml-2 justify-center items-center w-6 h-6 bg-cyan-800 text-white rounded-full animate-ping"></span>
                            <span className="absolute ml-2 justify-center items-center w-6 h-6 bg-green-600 text-white rounded-full">
                              {postVal.countOfWaitingReservation}
                            </span>
                          </>
                        )}
                      </p>

                      <FaAnglesDown className="mx-auto text-darkBlue" />
                    </>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      ) : (
        <div className="p-8 text-center text-gray-500">
          You don't have any post.
        </div>
      )}

    </section>
    )
  );
};

export default WorkerPostsPage;