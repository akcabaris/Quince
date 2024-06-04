import React, { useEffect, useState } from 'react';
import { PostGet } from '../../Models/Post';
import { FaAnglesDown } from "react-icons/fa6";
import { FaAnglesUp } from "react-icons/fa6";
import { useAuth } from '../../Context/useAuth';
import * as Yup from "yup";
import { yupResolver } from '@hookform/resolvers/yup';
import { CreateReservationAPI } from '../../Service/ReservationService';
import { handleError } from '../../Helpers/ErrorHandler';
import { useForm } from 'react-hook-form';
import { CreateConversationAPI, MessagePostAPI } from '../../Service/MessageService';
import MessageIcon from '@mui/icons-material/Message';
import SendIcon from '@mui/icons-material/Send';
import { toast } from 'react-toastify';
import { Navigate, useNavigate } from 'react-router';

interface Props {
  postValue: PostGet;
  isOpen: boolean;
  onToggle: () => void;
}

type createReservationInputs = {
  postId: number;
  reservationDate: Date;

};


const SquarishPost = ({ postValue, isOpen, onToggle }: Props) => {
  const { user } = useAuth();
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [reservationNote, setReservationNote] = useState<string>("");
  const [isDateNull, setIsDateNull] = useState<boolean>(false);
const [reservationNoteError, setReservationNoteError] = useState<string>("");

  const handleReservation = async () => {

    try {
      if (reservationNote == null || reservationNote.length < 1) {
        setReservationNoteError("You have to write a note");
        return;
      }
      if (postValue.postId != null && selectedDate != null) {
        setReservationNote("");
        const response = await CreateReservationAPI(postValue.postId, selectedDate, reservationNote);
        if (response) {
          if (response.status == 204) {
            setModalIsOpen(false);
            await toast.success(response.data.toString());
          } else if(response.status == 200) {
            setModalIsOpen(false);
            toast.warn(response.data.toString())
          }
        } 
         else if (selectedDate == null) {
          setIsDateNull(true);
          toast.error("Something went wrong")
        }
      }} catch (error) {
        handleError(error);
      }
      setReservationNote("");
    };

    const navigate = useNavigate();

    const handleCreateConversation = async () => {
      try {
        const conversationId = await CreateConversationAPI(postValue.workerId);
        navigate("/messages");
      } catch (error) {
        handleError(error)
      }
    }

    useEffect(() => {
      if (selectedDate != null)
        setIsDateNull(false);
    }, [selectedDate])

    useEffect(() => {
      if(reservationNote.length >= 1) { setReservationNoteError("");}
    }, [reservationNote])


    const openModal = () => {
      setModalIsOpen(true);
    };

    const closeModal = () => {
      setSelectedDate(null);
      setReservationNote("");
      setModalIsOpen(false);
    };


    const today = new Date().toISOString().split('T')[0];

    return (
      <div className="rounded-lg pb-2 px-2 md:mx-3 my-6 bg-gradi cursor-pointer border-spacing-2 shadow-blue-100/50 relative shadow-xl border max-w-180 min-h-48">
        <div className="flex justify-between mt-4 w-full bg-slate-50 items-center rounded-md shadow-md opacity-80 relative">
          <div className="flex items-center">
            <img className='w-12 items-center rounded-lg' src={postValue.pictureLink === "http://localhost:5279/resources/" ? "/img/profile.png" : postValue.pictureLink} alt="" />
            <p className='px-1 items-center font-serif text-md text-darkBlue'>{postValue.workerName} </p>
          </div>
          {user?.accountType == "Customer" || user?.accountType == "Worker" ? (

            <button onClick={handleCreateConversation}
              title='Send Message'
              className='right-0 my-4 bg-gradient-to-r hover:from-green-900 hover:to-green-700 from-sky-900 to-sky-600 text-sm text-white font-light py-2 px-4 rounded'>
              <MessageIcon />
            </button>
          ) : (
            <></>
          )}
        </div>

        <div>
          <p className='max-h-6 '>⭐️⭐️⭐️⭐️</p>
        </div>

        <div className='rounded-sm text-center'>
          <h1 className="text-md font-mono">{postValue.title}</h1>
        </div>
        {isOpen && (
          <div className="px-6 py-4 border rounded-md transition-all border-spacing-2 duration-300 ease-in-out">
            <div className="text-gray-900 font-light text-xs">{postValue.description}</div>
          </div>
        )}
        <div className='max-h-16 mt-2 text-center text-sm'>
          <p className="text-gray-700"> {postValue.city} - {postValue.county}</p>
        </div>
        <div className="flex mt-1 w-full">
          <div className="text-right font-thin text-sm flex w-full">
            <p className="text-gray-700 text-center w-3/5">{postValue.category}</p>
            <p className="text-green-700 font-bold text-center w-2/5">{postValue.price} - {postValue.priceCurrency} / {postValue.priceWorkUnit}</p>
          </div>
        </div>
        <div onClick={onToggle}>
          {
            isOpen ? (
              <FaAnglesUp className='mx-auto mt-2 text-darkBlue' />
            ) : (
              <FaAnglesDown className='mx-auto mt-2 text-darkBlue' />
            )
          }
        </div>
        {
          user?.accountType === "Customer" && (
            <>
              <div className=''>
                <button onClick={openModal} className='justify-end my-4 bg-gradient-to-r hover:from-green-900 hover:to-green-700 from-sky-800 to-sky-600 text-white font-bold py-2 px-4 rounded'>
                  Make a Reservation
                </button>
              </div>
              {modalIsOpen && (
                <div className="fixed inset-0 flex items-center justify-center z-50 px-2">
                  <div className="absolute inset-0 bg-black opacity-50"></div>
                  <div className="bg-white p-8 rounded shadow-lg z-10">
                    <h2 className="text-xl mb-4">You're making a reservation to {postValue.workerName}'s Post</h2>
                    <h3>Please select the reservation's date</h3>

                    <input
                      type="date"
                      min={today}
                      onChange={(e) => setSelectedDate(new Date(e.target.value))}
                      className="border p-2 rounded mb-4 w-full my-2"
                    />

                    <textarea
                      typeof='text'
                      maxLength={200}
                      onChange={(e) => setReservationNote(e.target.value)}
                      className="border p-2 rounded mb-4 w-full my-2"
                      placeholder="Send note about the reservation..."
                      aria-rowspan={2}

                    />
                    {reservationNoteError.length > 0 ? (<p className='text-red-600'>{reservationNoteError}</p>) : <></>}
                    {isDateNull ? <p className="text-red-500">You have to choose date</p> : (<></>)}
                    <div className="flex justify-end">
                      <button onClick={handleReservation} className='mt-2 p-2 bg-blue-500 text-white rounded mr-2'>
                        Confirm Reservation
                      </button>
                      <button onClick={closeModal} className='mt-2 p-2 bg-gray-500 text-white rounded'>
                        Close
                      </button>
                    </div>
                  </div>
                </div>
              )}

            </>
          )
        }
      </div>
    );
  }

  export default SquarishPost;
