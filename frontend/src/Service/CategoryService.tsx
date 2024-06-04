import axios from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { Category } from "../Models/Category";

const api = "http://localhost:5279/api/Category";

export const GetCategoriesAPI = async () => {
    try{
        const categories = await axios.get<Category[]>(api);
        if(categories != null){
        return categories;}
        else{ return null}
    } catch(error) {
        handleError(error);
    }
}