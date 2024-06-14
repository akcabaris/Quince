import React, { useEffect, useState } from 'react'
import { CreateReservationAPI } from '../../Service/ReservationService';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router';
import { handleError } from '../../Helpers/ErrorHandler';

type Props = {
    onClose: () => void;
    postId: number;
    workerName: string;
}

const MakeReservationModal = ({onClose, workerName, postId}: Props) => {

    const navigate = useNavigate();
    const [reservationNote, setReservationNote] = useState<string>("");
    const [selectedDate, setSelectedDate] = useState<Date | null>(null);
    const [dateError, setDateError] = useState<string>("");
    const [reservationNoteError, setReservationNoteError] = useState<string>("");
    const currentDate = new Date();
    const yesterday = new Date();
    yesterday.setDate(currentDate.getDate() - 1);



    const todayForInput = currentDate.toISOString().split('T')[0];
    currentDate.setFullYear(currentDate.getFullYear() + 5)
    const maxDateForInput = currentDate.toISOString().split('T')[0];

    const handleReservation = async () => {
        if(selectedDate == null){
          setDateError("Please choose the Date of reservation")
          return ;
        }
        if(selectedDate < yesterday || selectedDate.getFullYear()  > (currentDate.getFullYear() + 5)){
          setDateError("You can make a reservation only within the next 5 year.");
          return ;
        }
        if (reservationNote == null || reservationNote.length < 1) {
          setReservationNoteError("You have to write a note");
          return;
        }
        if(reservationNote.length > 200){
          setReservationNoteError("The Reservation Note Length can be a maximum of 200 Characters");
          return ;
        }
        try {
          
          if (postId != null && selectedDate != null) {
            setReservationNote("");
            const response = await CreateReservationAPI(postId, selectedDate, reservationNote);
            if (response) {
              if (response.status == 201) {
                onClose();
                await toast.success("Reservation Is successful");
              } else if (response.status == 200) {
                onClose();
                toast.warn(response.data.toString())
              }else if (response.status == 204) {
                onClose();
                toast.warn("You have to update your profile information before making a reservation.");
                navigate("/customer-profile");
              }
            }
            else if (selectedDate == null) {
              setDateError("Choose Date please");
            }
          }
        } catch (error) {
          handleError(error);
        }
      };
    
    



    useEffect(() => {
        if (reservationNote.length >= 1) { setReservationNoteError(""); }
      }, [reservationNote])
    
      useEffect(() => {
        if(selectedDate != null && selectedDate > yesterday && (selectedDate.getFullYear() < (currentDate.getFullYear() + 5))){
          setDateError("");
        }
    
      },[selectedDate]);


  return (
    (
        <div className="fixed inset-0 flex items-center justify-center z-50 px-2">
          <div className="absolute inset-0 bg-black opacity-50"></div>
          <div className="bg-white p-8 rounded shadow-lg z-10">
            <h2 className="text-xl mb-4">You're making a reservation to {workerName}'s Post</h2>
            <h3>Please select the reservation's date</h3>

            <input
              type="date"
              min={todayForInput}
              max={maxDateForInput}
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
            {dateError.length >0 ? <p className="text-red-500">{dateError}</p> : (<></>)}
            <div className="flex justify-end">
              <button onClick={handleReservation} className='mt-2 p-2 bg-blue-500 text-white rounded mr-2'>
                Confirm Reservation
              </button>
              <button onClick={onClose} className='mt-2 p-2 bg-gray-500 text-white rounded'>
                Close
              </button>
            </div>
          </div>
        </div>
      )
  )
}

export default MakeReservationModal