import React, { useEffect, useState } from 'react'
import { ReviewGET } from '../../Models/Review'
import { GetAccountReviewsAPI } from '../../Service/ReviewService';
import Spinner from '../../Components/Spinners/Spinner';
import ReviewCard from '../../Components/Reviews/ReviewCard';

type Props = {}

const ReviewsPage = (props: Props) => {
  const [reviews, setReviews] = useState<ReviewGET[] | null>();
  const [loader, setLoader] = useState<boolean>(false);

  const handleGetReviews = async () => {
    setLoader(true);

    const response = await GetAccountReviewsAPI();
    if (response && response.data.length > 0) {
      setReviews(response.data);
    } else if (response?.data.length == 0) {
      setReviews(null);
    }

    setLoader(false);
  }


  useEffect(() => {
    handleGetReviews();
  }, [])

  return (
    loader ? (
      <Spinner />
    ) : (
      <div className="mb-4">
              <h1 className='text-center text-xl font-bold text-blue-600 py-4'>Reviews</h1>
        {reviews != null ? (
          reviews.map((review) => (
            <ReviewCard key={review.reviewId} review={review} />
          ))
        ) : (
          <h1 className='text-center'>You don't have any Review.</h1>
        )}
      </div>
    )
  );
  
}

export default ReviewsPage