import React, { useEffect, useState } from 'react';
import { WorkerGET, WorkerPUT } from '../../Models/Worker';
import { UpdateWorkerPictureAPI, WorkerDeleteProfilePicture, WorkerGetAPI, WorkerPutAPI } from '../../Service/WorkerService';
import { handleError } from '../../Helpers/ErrorHandler';
import AddAPhotoIcon from '@mui/icons-material/AddAPhoto';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import * as Yup from "yup";
import { yupResolver } from '@hookform/resolvers/yup';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import Spinner from '../../Components/Spinners/Spinner';

type Props = {};



const allowedFileExtensions = [".jpg", ".jpeg", ".png"];
const maxFileSize = 1 * 1024 * 1024; // 1 MB



const WorkerDetails = ({ }: Props) => {
    const [error, setError] = useState<string | null>(null);
    const [worker, setWorker] = useState<WorkerGET | null>(null);
    const [isButtonDisabled, setIsButtonDisabled] = useState(false);
    const [loading, setLoading] = useState<boolean>();


    const handleUpdateWorker = async (data: WorkerPUT) => {
        try {
            const response = await WorkerPutAPI(data);
            if(response && response.status == 200) {
                toast.success("Account information updated successfully!");
              }
              else {
                toast.warn("Something went wrong");
              }
        } catch (error) {
            handleError(error);
            toast.warn("Failed to update account information.")
        }
        setIsButtonDisabled(true);
        setTimeout(() => setIsButtonDisabled(false), 1500);
    };
    



    const handleFileChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            const fileExtension = file.name.split('.').pop()?.toLowerCase();
            if (!allowedFileExtensions.includes(`.${fileExtension}`)) {
                await setError("Only .jpg, .jpeg, .png files are allowed.");
                return;
            }
            if (file.size > maxFileSize) {
                await setError("File size should not exceed 1 MB.");
                return;
            }
            setError(null);
            const canvas = document.createElement("canvas");
            const ctx = canvas.getContext("2d");
            if (!ctx) {
                setError("Canvas context is null.");
                return;
            }
            const img = new Image();
            img.src = URL.createObjectURL(file);
            await img.decode();

            const squareSize = Math.min(img.width, img.height);
            canvas.width = squareSize;
            canvas.height = squareSize;

            const xOffset = (img.width - squareSize) / 2;
            const yOffset = (img.height - squareSize) / 2;
            ctx.drawImage(img, xOffset, yOffset, squareSize, squareSize, 0, 0, squareSize, squareSize);

            const croppedImageData = canvas.toDataURL("image/jpeg");
            const blob = await fetch(croppedImageData).then((res) => res.blob());
            const croppedFile = new File([blob], "cropped-image.jpg", { type: "image/jpeg" });

            try {
                await UpdateWorkerPictureAPI(croppedFile);
                toast.success("Picture Updated successfully.");
                setError(null);
                await handleGetWorker();
            } catch (error) {
                handleError(error);
                toast.warn("Failed to update picture.");
            }
        }
    };

    const handleGetWorker = async () => {
        setLoading(true);
        try {
            const response = await WorkerGetAPI();
            if (response && response.data != null) {
                setWorker(response.data);
            }
        } catch (error) {
            handleError(error);
        }
        setLoading(false);
    };
    const handleDeletePhoto = async () => {
        try {
            await WorkerDeleteProfilePicture();
            toast.success("Picture deleted successfully!");
            handleGetWorker();
        } catch (error) {
            handleError(error);
        }
    };
    const validation = Yup.object().shape({
        fullName: Yup.string().required("Full Name is required").default(worker?.fullName)
            .min(3, "Too short name")
            .max(50, "Max length can be 50 characters"),
        phoneNumber: Yup.number().typeError("Format must be like 5XXXXXXXXX")
        .required("Phone number is required").default(worker?.phoneNumber)
        .test('is-starting-with-5', 'Phone number must start with 5', (value) => {
            // Telefon numarasının ilk karakterini al ve kontrol et
            const firstDigit = value.toString()[0];
            return firstDigit === '5';
        }),
        occupation: Yup.string().required("Occupation is required").default(worker?.occupation)
            .min(4)
            .max(30),
        description: Yup.string().required("Description can't be empty").default(worker?.description)
            .min(10, "At least 20 characters")
            .max(2000, "Max 2000 characters"),
    });
    const { register, handleSubmit, formState: { errors } } = useForm<WorkerPUT>({
        resolver: yupResolver(validation)
    });

    useEffect(() => {
        handleGetWorker();
    }, []);

    return (
        loading ? <Spinner /> : (
            worker && (
                <div className='w-full flex md:flex-row relative p-2'>
                    <div className='flex flex-grow flex-col justify-center items-center p-2 space-y-6'>
                        <div className='flex items-center justify-center'>
                            <label htmlFor="fileInput"
                                title='Click For Change the Profile Picture'
                                className="rounded-full flex flex-col items-center justify-center  transition duration-300 ease-in-out hover:opacity-70">
                                <img src={worker?.pictureLink === "http://localhost:5279/resources/" ? "/img/profile.png" : worker?.pictureLink}
                                    className='rounded-full w-64 shadow-lg ' alt="" />
                                <AddAPhotoIcon className='hover:text-gray-600' fontSize='large' />
                            </label>
                            <input id="fileInput" type="file" onChange={handleFileChange} accept={allowedFileExtensions.join(", ")} style={{ display: "none" }} />
                        </div>
                        {error != null ? (<p className='text-red-700'>{error}</p>) : (<></>)}
                        {
                            worker.pictureLink == "http://localhost:5279/resources/" ? (
                                <></>
                            ) : (
                                <div className=''>
                                    <button>
                                        <DeleteForeverIcon fontSize='large' onClick={handleDeletePhoto} />
                                    </button>
                                </div>
                            )
                        }
    
    
                        <form className='' onSubmit={handleSubmit(handleUpdateWorker)}>
                            <div>
                                <label htmlFor="fullName" className="block mb-2 text-sm font-medium text-gray-900">
                                    Full Name
                                </label>
                                <input
                                defaultValue={worker.fullName}
                                    type="text"
                                    id="fullName"
                                    minLength={3}
                                    maxLength={50}
                                    className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                                    placeholder={worker.fullName}
                                    {...register("fullName")}
                                />
                                {errors.fullName ? (
                                    <p className='text-red-500'>{errors.fullName.message}</p>
                                ) : null}
                            </div>
                            <div>
                                <label htmlFor="phoneNumber" className="block mb-2 text-sm font-medium text-gray-900">
                                    Phone Number
                                </label>
                                <input
                                                                defaultValue={worker.phoneNumber}
                                    id="phoneNumber"
                                    minLength={10}
                                    maxLength={10}
                                    inputMode='numeric'
                                    className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                                    placeholder={worker.phoneNumber.toString()}
                                    {...register("phoneNumber")}
                                />
                                {errors.phoneNumber ? (
                                    <p className='text-red-500'>{errors.phoneNumber.message}</p>
                                ) : null}
                            </div>
    
                            <div>
                                <label htmlFor="occupation" className="block mb-2 text-sm font-medium text-gray-900">
                                    Occupation
                                </label>
                                <input
                                                                defaultValue={worker.occupation}
                                    type="text"
                                    id="occupation"
                                    minLength={4}
                                    maxLength={30}
                                    className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                                    placeholder={worker.occupation}
                                    {...register("occupation")}
                                />
                                {errors.occupation ? (
                                    <p className='text-red-500'>{errors.occupation.message}</p>
                                ) : null}
                            </div>
                            <div className='md:min-w-96'>
                                <label htmlFor="description" className="block mb-2 text-sm font-medium text-gray-900">
                                    About Me
                                </label>
                                <textarea
                                                                defaultValue={worker.description}
                                    id="description"
                                    rows={5}
                                    minLength={10}
                                    maxLength={2000}
                                    className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                                    placeholder={worker.description}
                                    {...register("description")}
                                />
                                {errors.description ? (
                                    <p className='text-red-500'>{errors.description.message}</p>
                                ) : null}
                            </div>
    
                            <button
                                type="submit"
                                disabled={isButtonDisabled}
                                className='w-full mt-2 text-white bg-cyan-800 hover:bg-cyan-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center'
                            >
                                Update
                            </button>
                        </form>
                    </div >
                </div >
            )
        )
    );
};

export default WorkerDetails;
