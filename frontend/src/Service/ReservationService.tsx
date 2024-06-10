import axios, { AxiosResponse } from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { error } from "console";
import { CustomerReservationGet } from "../Models/Reservation";
const api = "http://localhost:5279/api/";

export const CreateReservationAPI = async (postId: number, reservationDate: Date, reservationNote: string) => {
    try {
        const response = await axios.post<AxiosResponse>(`${api}Reservation/${postId}`, {
            reservationDate: reservationDate,
            reservationNote: reservationNote
        });
        return response;
    } catch (error) {
        handleError(error)
    }

}

export const DeleteReservationAPI = async (reservationId:number)  => {
    try {
        const response = await axios.delete<AxiosResponse>(api+"Reservation/"+reservationId);
        return response;
    } catch (error) {
        handleError(error)
    }
}

export const GetCustomersReservationAPI = async () => {
    try {
        const response = await axios.get<CustomerReservationGet[]>(api+"Reservation");
        return response.data;
    } catch (error) {
        handleError(error)
    }
}


export const UpdateReservationStatusAPI = async (reservationId: number, status: string) => {
    try {
        const response = await axios.put<AxiosResponse>(`${api}Reservation/${reservationId}`, { status });
        return response;
    } catch (error) {
        handleError(error);
    }
}