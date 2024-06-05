import React, { useEffect, useState } from 'react'
import { CustomerReservationGet as CustomerReservationGet } from '../../Models/Reservation'
import { handleError } from '../../Helpers/ErrorHandler';
import { DeleteReservationAPI, GetCustomersReservationAPI, UpdateReservationStatusAPI } from '../../Service/ReservationService';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import Spinner from '../../Components/Spinners/Spinner';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import { toast } from 'react-toastify';

type Props = {}

const CustomerReservationsPage = (props: Props) => {
  const [reservationList, setReservationList] = useState<CustomerReservationGet[] | null>();
  const [loader, setLoader] = useState<boolean>();
  const [isOpen, setIsOpen] = useState<boolean>(false); // 1. State'i tanımla

  const hangleGetCustomerReservations = async () => {
    setLoader(true);
    try {
      const reservationVals = await GetCustomersReservationAPI();
      if (reservationVals && reservationVals.length > 0) {
        setReservationList(reservationVals);

      } else {
        setReservationList(null);
      }
    } catch (error) {
      handleError(error)
    }
    setLoader(false);
  }
  const handleDeleteReservation = async (reservationId: number) => {
    if (reservationId != null) {
      await DeleteReservationAPI(reservationId);
    }
    hangleGetCustomerReservations();
  }

  const handleUpdateReservationStatus = async (reservationId: number, status: string) => {
    if (reservationId != null && (status == "Done")) {
      try {
        const response = await UpdateReservationStatusAPI(reservationId, status);
        if (response && response.status == 200) {
          toast.success("It's marked as Done");
        }
      } catch (error) {
        handleError(error);
      }
    } else {
      toast.warn("Something went wrong !!")
    }
  }

  useEffect(() => {
    hangleGetCustomerReservations();
  }, [])

  const handlePostToggle = () => { // 2. Post'un açılıp kapanmasını sağla
    setIsOpen(!isOpen);
  }

  return (
    loader ? <Spinner /> : (
      <div className='min-h-screen'>
        <h1 className='text-center text-xl font-bold text-blue-600 py-4'>Reservations</h1>

        {reservationList != null && reservationList.length > 0 ? (
          reservationList.map((reservationVal, resIndex) => (
            <div className="item-center justify-center flex w-full " key={resIndex}>
              {/*Reservation*/}
              <div className='border border-gray-400 rounded-lg p-1 w-full md:max-w-180 text-center shadow-md space-y-3 bg-slate-100 '>
                <p className='text-gray-800 overflow-hidden text-wrap border border-gray-400 rounded-md shadow-md relative items-center justify-normal flex-row '>
                  <span className="">
                    <span className='text-green-800'>Reservation Note</span><br />{reservationVal.reservationNote}
                  </span>
                </p>
                <p className='justify-end'><span className='text-red-700'>Status:</span> {reservationVal.status} - <CalendarMonthIcon />{reservationVal.reservationDate.slice(0, 10)}</p>
                <div className='flex flex-row justify-center space-x-3 text-sm font-serif'>
                  {reservationVal.status == "Approved" ? (
                    <button onClick={() => { handleUpdateReservationStatus(reservationVal.reservationId, "Done") }}
                      className='justify-end shadow-md bg-gradient-to-r from-green-600 to-green-500 hover:from-green-800 hover:to-green-600 text-white text-sm font-mono p-1 rounded transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 duration-150'>
                      Mark as Done
                    </button>
                  ) : (
                    <></>
                  )}
                  {reservationVal.status == "Waiting" || "Denied" ? (
                    <button
                      onClick={() => { handleDeleteReservation(reservationVal.reservationId) }}
                      className='justify-end shadow-md bg-gradient-to-r from-yellow-500 to-yellow-400  hover:from-red-800 hover:to-red-700 text-white text-sm font-mono p-1 rounded transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 duration-150'>
                      Cancel
                    </button>) : (<></>)}
                </div>
                <div>
                  <span onClick={handlePostToggle} className="cursor-pointer">Post <KeyboardArrowDownIcon /></span> {/* Post'a tıklandığında handlePostToggle fonksiyonunu çağır */}
                  {isOpen && ( // Eğer isOpen true ise, post gösterilsin
                    <div className="rounded-lg pb-2 px-2 md:mx-3 my-2 cursor-pointer border-spacing-2 text-xs shadow-blue-100/50 relative shadow-xl border max-w-180 min-h-48">
                      <div className="flex justify-between mt-4 w-full items-center rounded-md shadow-md opacity-80 relative">
                        <div className="flex items-center">
                          <img className='w-12 items-center rounded-lg' src={reservationVal.postDto.pictureLink === "http://localhost:5279/resources/" ? "/img/profile.png" : reservationVal.postDto.pictureLink} alt="" />
                          <p className='px-1 items-center font-serif text-darkBlue'>{reservationVal.postDto.workerName} </p>
                        </div>
                      </div>
                      <div>
                        <p className='max-h-6 '>⭐️⭐️⭐️⭐️</p>
                      </div>
                      <div className='rounded-sm text-center'>
                        <h1 className="font-mono">{reservationVal.postDto.title}</h1>
                      </div>
                      <div className="px-6 py-4 border rounded-md transition-all border-spacing-2 duration-300 ease-in-out">
                        <div className="text-gray-900 font-light">{reservationVal.postDto.description}</div>
                      </div>
                      <div className='max-h-16 mt-2 text-center'>
                        <p className="text-gray-700"> {reservationVal.postDto.city} - {reservationVal.postDto.county}</p>
                      </div>
                      <div className="flex mt-1 w-full">
                        <div className="text-right font-thin flex w-full">
                          <p className="text-gray-700 text-center w-3/5">{reservationVal.postDto.category}</p>
                          <p className="text-green-700 font-bold text-center w-2/5">{reservationVal.postDto.price} - {reservationVal.postDto.priceCurrency} / {reservationVal.postDto.priceWorkUnit}</p>
                        </div>
                      </div>
                    </div>
                  )}
                </div>
              </div>
            </div>
          ))
        ) : <h1 className='text-center'>You don't have any Reservation.</h1>}
      </div>
    )
  );
}

export default CustomerReservationsPage
