import axios, { AxiosResponse, AxiosResponseHeaders, ResponseType } from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { AddPost, PostGet, WorkerPosts } from "../Models/Post";



const api = "http://localhost:5279/api/Post";

// get the posts (post doesn't mean the http method in here)
export const postGetAPI = async (city: string, county: string, category: string, page: number) => {
    try {
        const data = await axios.get<PostGet[]>(api + "/getPost" + `?City=${city}&County=${county}&Category=${category}&PageNumber=${page}`);
        return data;
    } catch (error) {
        handleError(error);
    }
};
export const postLengthGetAPI = async (city: string, county: string, category: string) => {
    try {
        const size = await axios.get<number>(api + "/getPostLength" + `?City=${city}&County=${county}&Category=${category}`);
        return size;
    } catch (error) {
        handleError(error);
    }
};
export const postGetWithId = async (postId: number) => {
    try {
        const response = await axios.get<PostGet>(api+"")
    } catch(error) {
        handleError(error)
    }
}

export const WorkerPostsGetAPI = async () => {
    try {
        const posts = await axios.get<WorkerPosts[]>(api + "/GetUserPosts");
        return posts.data;
    } catch (error) {
        handleError(error);
    }
};

export const DeletePostAPI = async (postId: number) => {
    try {
        const response = await axios.delete<AxiosResponse>(api + "/"+postId);
        return response;
    } catch (error) {
        handleError(error);
    }
};


export const CreatePostAPI = async (AddPost: AddPost, city: string, county: string) => {
    try {
        const response = await axios.post<AxiosResponse>(api, {
            category: AddPost.category,
            title: AddPost.title,
            description: AddPost.description,
            price: AddPost.price,
            priceCurrency: AddPost.priceCurrency,
            priceWorkUnit: AddPost.priceWorkUnit,
            city: city,
            county: county
        });
        return response;
    } catch (error) {
        handleError(error);
    }
}

