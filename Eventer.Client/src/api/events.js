import axios from "axios";

const api = axios.create({
    baseURL: "https://localhost:7028/api", 
});

export const getEvents = async (filterParams) => {
    const response = await api.get("/Events", { params: filterParams });
    return response.data.events;
};
