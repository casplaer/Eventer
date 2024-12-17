import axios from "axios";

const apiClient = axios.create({
    baseURL: "https://localhost:7028/api/",
});

// ����������� ��� ���������� ������
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

// ����������� ������ ��� ��������� ��������� ������
apiClient.interceptors.response.use(
    (response) => response, // ���� ������ �������, ������������ �����
    async (error) => {
        if (error.response?.status === 401 && !error.config._retry) {
            // ����, ����� �� ��������� �������
            error.config._retry = true;

            try {
                const refreshToken = sessionStorage.getItem("refreshToken");

                const refreshResponse = await axios.post(
                    "https://localhost:7028/api/auth/refresh",
                    { refreshToken }
                );

                const newAccessToken = refreshResponse.data;

                // ����� Access Token � sessionStorage
                sessionStorage.setItem("accessToken", newAccessToken);

                // ���������� ��������� Authorization ��� ��������� �������
                error.config.headers.Authorization = `Bearer ${newAccessToken}`;

                // ���������� ��������� ������� � ����� �������
                return apiClient.request(error.config);
            } catch (refreshError) {
                console.error("������ ���������� ������:", refreshError.response?.data || refreshError.message);

                sessionStorage.clear();
                window.location.href = "/login";
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error); 
    }
);


export default apiClient;
