import React from 'react'
import { ReviewGET } from '../../Models/Review'
import StarIcon from '@mui/icons-material/Star';

type Props = {
    review: ReviewGET;
}

const ReviewCard = ({ review }: Props) => {
    return (
        <div className="p-6 max-w-lg mx-auto bg-white rounded-xl shadow-md flex items-center my-2 space-x-4">
            <div>
                <div className="text-lg font-medium text-start">
                    {
                        Array.from({ length: 5 }, (_, i) => (
                            <StarIcon key={i} className={`${i < review.reviewScore ? "text-yellow-300" : "text-gray-300"}`} />
                        ))
                    }
                </div>
                <p className="mt-2 text-gray-500 text-start text-base break-all">
                    {review.reviewText}
                </p>
            </div>
        </div>
    )
}

export default ReviewCard
