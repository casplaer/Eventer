import axios from "axios";

const apiClient = axios.create({
    baseURL: "https://localhost:7028/api/",
});

// Интерсептор для добавления токена
apiClient.interceptors.request.use(
    (config) => {
        const token = sessionStorage.getItem("accessToken");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Интерсептор ответа для обработки истечения токена
apiClient.interceptors.response.use(
    (response) => response, // Если запрос успешен, возвращается ответ
    async (error) => {
        if (error.response?.status === 401 && !error.config._retry) {
            // Флаг, чтобы не зациклить запросы
            error.config._retry = true;

            try {
                const refreshToken = sessionStorage.getItem("refreshToken");

                const refreshResponse = await axios.post(
                    "https://localhost:7028/api/auth/refresh",
                    { refreshToken }
                );

                const newAccessToken = refreshResponse.data;

                // Новый Access Token в sessionStorage
                sessionStorage.setItem("accessToken", newAccessToken);

                // Обновление заголовка Authorization для исходного запроса
                error.config.headers.Authorization = `Bearer ${newAccessToken}`;

                // Повторение исходного запроса с новым токеном
                return apiClient.request(error.config);
            } catch (refreshError) {
                console.error("Ошибка обновления токена:", refreshError.response?.data || refreshError.message);

                sessionStorage.clear();
                window.location.href = "/login";
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error); 
    }
);


export default apiClient;
