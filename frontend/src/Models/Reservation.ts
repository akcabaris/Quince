import { PostGet } from "./Post";

export type CustomerReservationGet = {
    reservationId: number;
    customerId: string;
    status: string;
    reservationDate: string;
    reservationNote: string;
    postDto: PostGet;
}
