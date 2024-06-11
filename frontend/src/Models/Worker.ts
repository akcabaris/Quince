import { ReviewGET } from "./Review";

export type WorkerGET = {
    WorkerId: string;
    fullName: string;
    pictureLink: string;
    phoneNumber: number;
    description: string;
    occupation: string;
}
export type WorkerPUT = {
    fullName: string;
    phoneNumber: number;
    occupation: string;
    description: string;
}
export type WorkerPublicGET = {
    WorkerId: string;
    fullName: string;
    pictureLink: string;
    phoneNumber: number;
    description: string | null ;
    occupation: string |null;
    reviewScore: number | null;
    reviewList: ReviewGET[];
}
