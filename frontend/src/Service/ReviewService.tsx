import axios, { Axios, AxiosResponse } from "axios";
import { ReviewGET, ReviewPOST } from "../Models/Review";
import { handleError } from "../Helpers/ErrorHandler";

const api = "http://localhost:5279/api";

export const CreateReviewAPI = async (reviewData: ReviewPOST) => {
    try {
        const response = await axios.post<AxiosResponse>(api+"/Review",(reviewData))
        return response;
    } catch(error){
        handleError(error);
    }

}

export const GetReviewsAPI = async (workerId: string) => {
    try {
        const response = await axios.get<ReviewGET[]>(api+"/Review/GetReviews?workerId="+workerId);
        return response;
    } catch(error){
        handleError(error);
    }
}