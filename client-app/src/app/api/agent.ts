import axios, { AxiosResponse } from "axios";

axios.defaults.baseURL = 'https://localhost:7175/';

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    post: <T>(url: string, body: any) => axios.post<T>(url, body).then(responseBody),
}

const Application = {
    login: (email: string, password: string) =>
        requests.post<{ status: string; token: string }>('Registration/login', { email, password }),
}

const agent = {
    Application
}

export default agent;
