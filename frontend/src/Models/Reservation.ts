import { PostGet } from "./Post";
import { ReviewGET } from "./Review";

export type CustomerReservationGet = {
    reservationId: number;
    customerId: string;
    status: string;
    reservationDate: string;
    reservationNote: string;
    postDto: PostGet;
    reviewDto: ReviewGET | null;
}
