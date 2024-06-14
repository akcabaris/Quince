import React, { useState } from 'react';
import { PostGet } from '../../Models/Post';
import { FaAnglesDown, FaBullseye } from "react-icons/fa6";
import { FaAnglesUp } from "react-icons/fa6";
import { useAuth } from '../../Context/useAuth';
import { handleError } from '../../Helpers/ErrorHandler';
import { CreateConversationAPI } from '../../Service/MessageService';
import MessageIcon from '@mui/icons-material/Message';
import { useNavigate } from 'react-router';
import StarIcon from '@mui/icons-material/Star';
import StarHalfIcon from '@mui/icons-material/StarHalf';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import InspectWorkerModal from '../Modal/InspectWorkerModal';
import MakeReservationModal from '../Modal/MakeReservationModal';

interface Props {
  postValue: PostGet;
  isOpen: boolean;
  onToggle: () => void;
}



const SquarishPost = ({ postValue, isOpen, onToggle }: Props) => {
  const { user } = useAuth();
  const [showReservationModal, setShowReservationModal] = useState(false);
  const [showWorkerPublicModal, setShowWorkerPublicModal] = useState(false);

  const navigate = useNavigate();

  const handleCreateConversation = async () => {
    try {
      const conversationId = await CreateConversationAPI(postValue.workerId);
      navigate("/messages");
    } catch (error) {
      handleError(error)
    }
  }

  const closeModal = () => {
    setShowReservationModal(false);
  };


  const renderStars = (score: number | null) => {
    const stars = [];
    if(score == null ){
      for (let i = 1; i <= 5; i++){
        stars.push(<StarBorderIcon key={i} fontSize='large' className="text-gray-300" />)
      };
    } else{
    for (let i = 1; i <= 5; i++) {
      if (i <= score) {
        stars.push(<StarIcon key={i} fontSize='large' className="text-yellow-300" />);
      }
      else if ((i - score) <= 0.75 && (i - score) > 0.25) {
        stars.push(<StarHalfIcon key={i} fontSize='large' className="text-yellow-300" />);
      }
      else if (0.25 >= (i - score) && (i - score) > 0) {
        stars.push(<StarIcon key={i} fontSize='large' className="text-yellow-300" />);
      }
      else {
        stars.push(<StarBorderIcon key={i} fontSize='large' className="text-gray-300" />);
      }
    }}
    return stars;
  };

  return (
    <>
    {
      showWorkerPublicModal ? <InspectWorkerModal key={postValue.workerId} onClose={() => setShowWorkerPublicModal(false)} workerId={postValue.workerId} /> : <></>
     
    }
    <div className="rounded-lg pb-2 px-2 md:mx-3 my-6 border-spacing-2 shadow-blue-100/50 relative shadow-xl border max-w-180 min-h-48">
      <div className="flex justify-between mt-4 w-full bg-slate-50 items-center rounded-md shadow-md opacity-80 relative">
        <div className="flex items-center">
          <img className='w-12 items-center rounded-lg' src={postValue.pictureLink === "http://localhost:5279/resources/" ? "/img/profile.png" : postValue.pictureLink} alt="" />
          <p className='px-1 items-center font-serif text-md text-darkBlue hover:opacity-85' onClick={() => setShowWorkerPublicModal(true)} title='Click for inspecting to User?'>{postValue.workerName} </p>
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

      <div className="text-lg font-medium flex flex-row mb-2 text-start">
        {renderStars(postValue.userScore)} {postValue.userScore != null ? (<p className='font-sans'>{postValue.userScore.toFixed(2)}</p>) : (<></>)}
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
      <div  className='w-full'>
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
              <button onClick={() => {setShowReservationModal(true)}} className='justify-end my-4 bg-gradient-to-r hover:from-green-900 hover:to-green-700 from-sky-800 to-sky-600 text-white font-bold py-2 px-4 rounded'>
                Make a Reservation
              </button>
            </div>
            {showReservationModal && <MakeReservationModal onClose={closeModal} postId={postValue.postId} workerName={postValue.workerName} />}

          </>
        )
      }
      </div>
    </div>
    </>
  );
}

export default SquarishPost;
