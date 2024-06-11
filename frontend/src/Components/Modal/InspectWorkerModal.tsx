import React, { useEffect, useState } from 'react'
import { WorkerGET, WorkerPublicGET } from '../../Models/Worker';
import { WorkerGetAPI, WorkerGetPublicAPI as WorkerPublicGetAPI } from '../../Service/WorkerService';
import Spinner from '../Spinners/Spinner';
import { ReviewGET } from '../../Models/Review';
import ReviewCard from '../Reviews/ReviewCard';
import StarIcon from '@mui/icons-material/Star';
import StarHalfIcon from '@mui/icons-material/StarHalf';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import LocalPhoneIcon from '@mui/icons-material/LocalPhone';

type Props = {
  onClose: () => void;
  workerId: string;
}

const InspectWorkerModal = ({ onClose, workerId }: Props) => {
  const [worker, setWorker] = useState<WorkerPublicGET | null>(null);
  const [loader, setLoader] = useState<boolean>(false);

  const handleGetWorker = async () => {
    setLoader(true);
    if (workerId != null) {
      const response = await WorkerPublicGetAPI(workerId);
      if (response && response.status == 200) {
        setWorker(response.data);
      }
    }
    setLoader(false);
  }

  const handleCancel = () => {
    setWorker(null);
    onClose();
  }

  useEffect(() => {
    handleGetWorker();
  }, [])

  const renderStars = (score: number | null) => {
    const stars = [];
    if (score == null) {
      for (let i = 1; i <= 5; i++) {
        stars.push(<StarBorderIcon fontSize='large' className="text-gray-300" key={i} />)
      };
    } else {
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
      }
    }
    return stars;
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-white p-6 rounded-lg shadow-lg m-4 w-full max-w-md md:max-w-lg lg:max-w-xl">
        {loader ? (
          <Spinner />
        ) : (
          <div className="flex flex-col items-center w-full">
            <div className="flex flex-col items-center mb-4">
              <img
                className="w-24 h-24 rounded-full object-cover mb-2"
                src={worker?.pictureLink === "http://localhost:5279/resources/" ? "/img/profile.png" : worker?.pictureLink}
                alt=""
              />
              <h2 className="text-xl font-semibold">{worker?.fullName}</h2>
              <p className="text-gray-600">{worker?.phoneNumber}</p>
              <p className="text-gray-600 mb-4">{worker?.occupation}</p>
            </div>

            <div className="bg-gray-100 p-4 rounded-md mb-4 w-full text-center max-h-36 md:max-h-48 overflow-y-auto">
              <h3 className="text-lg font-medium text-gray-700 mb-2">About Me</h3>
              <p className="text-gray-600 break-words text-left text-sm md:text-base">{worker?.description}</p>
            </div>

            {worker && (<>
                            <h1 className='text-center text-xl font-bold text-blue-600 py-4'>Reviews</h1>
              <div className="flex items-center text-lg font-medium mb-4">
                
                {worker.reviewScore != null ? (
                  <>
                    <p className="font-sans mr-2">{worker.reviewScore.toFixed(2)}</p>
                    {renderStars(worker.reviewScore)}
                    <span className="ml-2 text-base">({worker.reviewList.length} Reviews)</span>
                  </>
                ) : (
                  <>
                    <p className='text-center'>{renderStars(worker.reviewScore)}<br />No Reviews</p>
                  </>
                )}
              </div>
              </>
            )}

            <div className="overflow-y-auto max-h-56 md:max-h-64 w-full mb-4">
              {worker?.reviewList.map((reviewVal: ReviewGET) => (
                <ReviewCard key={reviewVal.reviewId} review={reviewVal} />
              ))}
            </div>

            <button
              onClick={handleCancel}
              type="button"
              className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 transition duration-300"
            >
              Cancel
            </button>
          </div>
        )}
      </div>
    </div>
  )
}

export default InspectWorkerModal
