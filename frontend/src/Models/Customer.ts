export type CustomerGET = {
    customerId: number;
    pictureLink: string;
    fullName:string;
    phoneNumber: number;
    gender: string;
}

export type CustomerPUT = {
    fullName:string;
    phoneNumber: number;
    gender: string;
}