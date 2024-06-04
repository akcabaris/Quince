import React, { useEffect, useState } from 'react'
import { CustomerGET, CustomerPUT } from '../../Models/Customer';
import { CustomerDeleteProfilePicture, CustomerGetAPI, CustomerPutAPI, UpdateCustomerPictureAPI } from '../../Service/CustomerService';
import { toast } from 'react-toastify';
import { handleError } from '../../Helpers/ErrorHandler';
import { useForm } from 'react-hook-form';
import AddAPhotoIcon from '@mui/icons-material/AddAPhoto';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import * as Yup from "yup";
import { yupResolver } from '@hookform/resolvers/yup';
import Spinner from '../../Components/Spinners/Spinner';


type Props = {}


const allowedFileExtensions = [".jpg", ".jpeg", ".png"];
const maxFileSize = 1 * 1024 * 1024; // 1 MB

const CustomerDetails = (props: Props) => {
  const [error, setError] = useState<string | null>(null);
  const [customer, setCustomer] = useState<CustomerGET | null>(null);
  const [isButtonDisabled, setIsButtonDisabled] = useState(false);
  const [loading, setLoading] = useState<boolean>();

  const handleUpdateCustomer = async (data: CustomerPUT) => {
    try {
      const response = await CustomerPutAPI(data);
      if(response && response.status == 200) {
        toast.success("Account information updated successfully!");
      }
      else {
        toast.warn("Failed to update account information.")
      }
    } catch (error) {
      handleError(error);
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
        await UpdateCustomerPictureAPI(croppedFile);
        toast.success("Picture Updated successfully.");
        setError(null);
        await handleGetCustomer();
      } catch (error) {
        handleError(error);
        toast.warn("Failed to update picture.");
      }
    }
  };
  const handleGetCustomer = async () => {
    setLoading(true);
    try {
      const response = await CustomerGetAPI();
      if (response && response.data != null) {
        setCustomer(response.data);
      }
    } catch (error) {
      handleError(error);
    }
    setLoading(false);
  };

  const handleDeletePhoto = async () => {
    try {
      await CustomerDeleteProfilePicture();
      toast.success("Picture deleted successfully!");
      handleGetCustomer();
    } catch (error) {
      handleError(error);
    }
  };




  const validation = Yup.object().shape({
    fullName: Yup.string().default(customer?.fullName).required("Full Name is required")
      .min(3, "Too short name")
      .max(50, "Max length can be 50 characters"),
    phoneNumber: Yup.number().typeError("Format must be like 5XXXXXXXXX")
      .required("Phone number is required").default(customer?.phoneNumber)
      .test('is-starting-with-5', 'Phone number must start with 5', (value) => {
        const firstDigit = value.toString()[0];
        return firstDigit === '5';
      }),
    gender: Yup.string().required("Gender is required").default(customer?.gender)
      .min(4)
      .max(30),
  });
  const { register, handleSubmit, formState: { errors } } = useForm<CustomerPUT>({
    resolver: yupResolver(validation)
  });

  useEffect(() => {
    handleGetCustomer();
  }, []);


  return (
    loading ? <Spinner /> : (
      customer && (
        <div className='w-full flex md:flex-row relative p-2'>
          <div className='flex flex-grow flex-col justify-center items-center p-2 space-y-6'>
            <div className='flex items-center justify-center'>
              <label htmlFor="fileInput"
                title='Click For Change the Profile Picture'
                className="rounded-full flex flex-col items-center justify-center  transition duration-300 ease-in-out hover:opacity-70">
                <img src={customer?.pictureLink === "http://localhost:5279/resources/" ? "/img/profile.png" : customer?.pictureLink}
                  className='rounded-full w-64 shadow-lg ' alt="" />
                <AddAPhotoIcon className='hover:text-gray-600' fontSize='large' />
              </label>
              <input id="fileInput" type="file" onChange={handleFileChange} accept={allowedFileExtensions.join(", ")} style={{ display: "none" }} />
            </div>
            {error != null ? (<p className='text-red-700'>{error}</p>) : (<></>)}
            {
              customer.pictureLink == "http://localhost:5279/resources/" ? (
                <></>
              ) : (
                <div className=''>
                  <button>
                    <DeleteForeverIcon fontSize='large' onClick={handleDeletePhoto} />
                  </button>
                </div>
              )
            }


            <form className='' onSubmit={handleSubmit(handleUpdateCustomer)}>
              <div>
                <label htmlFor="fullName" className="block mb-2 text-sm font-medium text-gray-900">
                  Full Name
                </label>
                <input
                  defaultValue={customer.fullName}
                  type="text"
                  id="fullName"
                  minLength={3}
                  maxLength={50}
                  className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  placeholder={customer.fullName}
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
                  defaultValue={customer.phoneNumber}
                  id="phoneNumber"
                  minLength={10}
                  maxLength={10}
                  inputMode='numeric'
                  className="bg-gray-50 border border-slate-500 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  placeholder={customer.phoneNumber.toString()}
                  {...register("phoneNumber")}
                />
                {errors.phoneNumber ? (
                  <p className='text-red-500'>{errors.phoneNumber.message}</p>
                ) : null}
              </div>

              <div className="flex flex-col space-y-2">
  <label htmlFor="gender" className="block text-sm font-medium text-gray-900">
    Gender
  </label>
  <div className="flex space-x-4">
    <label htmlFor="male" className="flex items-center">
      <input
        type="radio"
        id="male"
        value="Male"
        {...register("gender")}
        defaultChecked={customer.gender === "Male"}
        className="focus:ring-primary-600 h-4 w-4 text-primary-600 border-gray-300 rounded"
      />
      <span className="ml-2 text-sm text-gray-900">Male</span>
    </label>
    <label htmlFor="female" className="flex items-center">
      <input
        type="radio"
        id="female"
        value="Female"
        {...register("gender")}
        defaultChecked={customer.gender === "Female"}
        className="focus:ring-primary-600 h-4 w-4 text-primary-600 border-gray-300 rounded"
      />
      <span className="ml-2 text-sm text-gray-900">Female</span>
    </label>
    <label htmlFor="unspecified" className="flex items-center">
      <input
        type="radio"
        id="unspecified"
        value="Unspecified"
        {...register("gender")}
        defaultChecked={customer.gender === "Unspecified"}
        className="focus:ring-primary-600 h-4 w-4 text-primary-600 border-gray-300 rounded"
      />
      <span className="ml-2 text-sm text-gray-900">Unspecified</span>
    </label>
  </div>
  {errors.gender && <p className="text-red-500 text-sm">{errors.gender.message}</p>}
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
}

export default CustomerDetails;