import axios, { Axios, AxiosResponse } from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { ConversationGet, Message, MessagePost } from "../Models/Message";

const api = "http://localhost:5279/api/";

export const ConversationsGetAPI = async () => {
    try {
        const data = await axios.get<ConversationGet[]>(api +"Conversation");
        return data;
    }catch(error){
        handleError(error);
    }
};


export const MessagePostAPI = async (conversationId: number, content:string) => {
    try {
        const data = await axios.post<MessagePost>(api+"Message/"+conversationId, {
            content:content,
        })
        return data;
    } catch (error){
        handleError(error);
    }
};

export const GetMessagesAPI = async (conversationId: number) => {
    try {
        const data = await axios.get<Message[]>(api+"Conversation/Messages?id="+ conversationId);
        return data;
    } catch(error) {
        handleError(error);
    }
}


export const CreateConversationAPI = async (userId: string) => {
    try {
        const response = await axios.post<number>(`${api}Conversation/${userId}`);
        return response;
    } catch(error) {
        handleError(error);
    }
}

export const DeleteConversationAPI = async (conversationId: number) => {
    try {
        const response = await axios.put<AxiosResponse>(api+"Conversation/"+conversationId);
        return response.data;
    } catch (error) {
        handleError(error);
    }
}

export const DeleteMessagesAPI = async (conversationId: number) => {
    try {
        const response = await axios.put<AxiosResponse>(api+"Message/"+conversationId);
        return response.data;
    }
    catch (error){
        handleError(error);
    }
}