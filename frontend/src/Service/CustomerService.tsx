import axios, { Axios, AxiosResponse } from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { CustomerGET, CustomerPUT } from "../Models/Customer";
const api = "http://localhost:5279/api/";


export const CustomerGetAPI = async () => {
    try {
        const response = await axios.get<CustomerGET>(api +"Customer");
        return response;
    }catch(error){
        handleError(error);
    }
};

export const CustomerPutAPI = async (customerData: CustomerPUT) => {
    try {
        const response = await axios.put<AxiosResponse>(api + 'Customer', customerData);
        return response; 
    } catch (error) {
        handleError(error);
    }
};

export const CustomerDeleteProfilePicture = async () => {
    try {
        await axios.delete(api+ 'Customer/DeletePic');
    }
    catch (error) {
        handleError(error);
    }
}

export const UpdateCustomerPictureAPI = async (imageFile: File): Promise<void> => {
    const formData = new FormData();
    formData.append("ImageFile", imageFile);

    try {
        const response = await axios.put(api + "Customer/pic", formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response.data;
    } catch (error) {
        handleError(error);
        throw error;
    }
};