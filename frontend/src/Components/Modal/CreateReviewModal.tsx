import React, { useState } from 'react'
import { handleError } from '../../Helpers/ErrorHandler';
import { CreateReviewAPI } from '../../Service/ReviewService';
import { ReviewPOST } from '../../Models/Review';
import { toast } from 'react-toastify';
import * as Yup from "yup";
import { yupResolver } from '@hookform/resolvers/yup';
import { useForm } from 'react-hook-form';
import StarIcon from '@mui/icons-material/Star';

type Props = {
    onClose: () => void;
    reservationId: number;
    handleUpdate: () => void;
}

const validation = Yup.object().shape({
    reviewText: Yup.string().required("Review Text is required").min(10, "At least 10 Character.").max(240, "Max 200 Character."),
    reviewScore: Yup.number().required("Review Score is required").min(1, "").max(5, ""),
    reservationId: Yup.number().required()
});

const CreateReviewModal = ({ onClose, reservationId, handleUpdate }: Props) => {

    const { register, handleSubmit, formState: { errors }, reset, setValue, getValues } = useForm<ReviewPOST>({ resolver: yupResolver(validation) });
    setValue('reservationId', reservationId);

    const handleCreateReview = async (formData: ReviewPOST) => {
        try {
            const response = await CreateReviewAPI(formData);
            if (response && response.status == 200) {
                toast.success("Review successfully submitted! Thank you for your feedback.");
            } else if (response) {
                toast.warn(response.data.toString());
            }
        } catch (error) {
            handleError(error);
        }
        onClose();
        reset();
        handleUpdate();
    };

    const handleCancel = () => {
        reset();
        onClose();
    }

    const [starNum, setStarNum] = useState<number>(0);
    const handleStarNum = (whichStar: number) => {
        if (getValues('reviewScore') != null) {
            setStarNum(getValues('reviewScore'));
        } else {
            setStarNum(whichStar);
        }
    }

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white p-4 rounded-lg shadow-lg m-2 flex">
                <form onSubmit={handleSubmit(handleCreateReview)} className='items-center justify-center text-center'>
                    <h1 className="text-xl font-semibold mb-4">Send a review about this reservation.</h1>

                    <div className="text-lg font-medium flex flex-row mb-2 text-start">
                        {
                            Array.from({ length: 5 }, (_, i) => (
                                <span key={i}
                                    onMouseOver={() => { handleStarNum(i + 1) }}
                                    onMouseOut={() => { handleStarNum(0) }}
                                    onClick={() => { setValue('reviewScore', (i + 1)) }}
                                >
                                    <StarIcon fontSize='large' className={` ${i + 1 <= starNum ? "text-yellow-300" : "text-gray-300"}`} />
                                </span>
                            ))
                        }
                        {errors.reviewScore && <p className='text-red-500 text-sm mt-1'>{errors.reviewScore.message}</p>}
                    </div>
                    <textarea
                        id='reviewText'
                        {...register('reviewText')}
                        rows={4}
                        required={true}
                        maxLength={240}
                        className="border border-gray-300 rounded p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
                    >

                    </textarea>
                    <button onClick={() => { handleCancel() }} type='button' className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400">Cancel</button>
                    <button
                        type='submit'
                        className="px-4 py-2 mx-2 bg-red-500 text-white rounded hover:bg-red-600">Submit</button>
                </form>
            </div>
        </div>
    )
}

export default CreateReviewModal;
