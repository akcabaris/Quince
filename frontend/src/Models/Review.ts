export type ReviewPOST = {
    reviewText: string;
    reviewScore: number;
    reservationId: number;
}

export type ReviewGET = {
    reviewId: number;
    reviewText: string;
    reviewScore: number;
    reservationId: number;
    reviewToUserId: string;
    reviewWriterUserId: string;
}