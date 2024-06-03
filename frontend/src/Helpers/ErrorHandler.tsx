import axios from "axios";
import { toast } from "react-toastify";

export const handleError = (error: any) => {
    if(axios.isAxiosError(error)){
        var err = error.response;
        if(Array.isArray(err?.data.errors)) {
            for(let val of err?.data.errors){
                toast.warning(val.description);
            }
        } else if(typeof err?.data.errors === "object") {
            for(let e in err?.data.errors){
                toast.warning(err.data.errors[e][0]);
            }
        } else if(err?.data){
            toast.warning(err.data);
        } else if(err?.status == 401){
            toast.warning("Please Login");
            window.history.pushState({}, "LoginPage", "/login");
        } else if(err){
            toast.warning(err?.data);
        } else if(error.code == 'ERR_NETWORK') {
            toast.warning("Network Error Occured. Please try again later. Error Type: " + error.message);
        }
        else {
            toast.warning("An error occurred, please try again later.");
        }
    } else {
        toast.warning("Something went wrong.")
    }
};