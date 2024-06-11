import axios, { Axios, AxiosResponse } from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { WorkerGET, WorkerPUT, WorkerPublicGET } from "../Models/Worker";

const api = "http://localhost:5279/api/";

export const WorkerGetAPI = async () => {
    try {
        const response = await axios.get<WorkerGET>(api +"Worker");
        return response;
    }catch(error){
        handleError(error);
    }
};

export const WorkerGetPublicAPI = async (workerId: string) => {
    try {
        const response = await axios.get<WorkerPublicGET>(api +"Worker/GetPublic?workerId="+ workerId);
        return response;
    } catch (error) {
        console.error('Error fetching worker public data:', error);
        throw error;
    }
}


export const WorkerPutAPI = async (workerData: WorkerPUT) => {
    try {
        const response = await axios.put<AxiosResponse>(api + 'Worker', workerData);
        return response;
    } catch (error) {
        handleError(error);
    }
};


export const WorkerDeleteProfilePicture = async () => {
    try {
        await axios.delete(api+ 'Worker/DeletePic');
    }
    catch (error) {
        handleError(error);
    }
}

export const UpdateWorkerPictureAPI = async (imageFile: File): Promise<void> => {
    const formData = new FormData();
    formData.append("ImageFile", imageFile);

    try {
        const response = await axios.put(api + "Worker/pic", formData, {
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

