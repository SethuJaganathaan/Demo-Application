import axios, { AxiosResponse } from "axios";

axios.defaults.baseURL = 'https://localhost:7175/';
const responseBody = <T> (response: AxiosResponse<T>) => response.data;

const requests = {
    //get: <T> (url : string) => axios.get<T>(url).then(responseBody),
    post:<T> (url : string, body: {}) => axios.post<T>(url,body).then(responseBody),
}

const Application = {
    //list: () => requests.get<>('/'),
    login: (email: string, password: string) => requests.post<{ status: string, token: string }>('Registration/login', { email, password }),
}

const agent = {
    Application
} 

export default agent;