export type PostGet = {
    postId: number;
    workerId: string;
    workerName: string;
    pictureLink: string;
    category: string;
    title: string;
    description: string;
    price: number;
    priceCurrency: string,
    priceWorkUnit: string,
    city: string;
    county: string;
    isPostActive: boolean;
    userScore: number | null;
}
export type AddPost = {
    category: string;
    title: string;
    description: string;
    price: number;
    priceCurrency: string,
    priceWorkUnit: string,
}

export type WorkerPosts = {
    postId: number;
    workerId: string;
    category: string;
    title: string;
    description: string;
    price: number;
    priceCurrency: string,
    priceWorkUnit: string,
    city: string;
    county: string;
    countOfWaitingReservation: number;
    isPostActive: boolean;
    reservList: postReservations[];
}

export type postReservations = {
    reservationId: number;
    customerId: string;
    postId: number;
    status: string;
    reservationDate: string;
    reservationNote: string;
    customerPictureLink: string;
    postTitle: string;
    customerName: string;
}