import axios from "axios";

const apiClient = axios.create({
    baseURL: "https://localhost:7028/api/",
});

// ����������� ��� ���������� ������
apiClient.interceptors.request.use(
    (config) => {
        const token = sessionStorage.getItem("accessToken");
        if (token) {
            console.log(token);
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
                // ������ �� �������� /refresh
                const refreshToken = sessionStorage.getItem("refreshToken");
                console.log("Refresh Token:", refreshToken);

                debugger;

                const refreshResponse = await axios.post(
                    "https://localhost:7028/api/auth/refresh",
                    { refreshToken }
                );

                const newAccessToken = refreshResponse.data; // ���������, ��� ������ ���������� ���������� ���
                console.log("New Access Token:", newAccessToken);
                debugger;

                // ����� Access Token � sessionStorage
                sessionStorage.setItem("accessToken", newAccessToken);

                // ���������� ��������� Authorization ��� ��������� �������
                error.config.headers.Authorization = `Bearer ${newAccessToken}`;

                // ���������� ��������� ������� � ����� �������
                return apiClient.request(error.config);
            } catch (refreshError) {
                console.error("������ ���������� ������:", refreshError.response?.data || refreshError.message);

                // ��������������� �� �����
                sessionStorage.clear();
                window.location.href = "/login";
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error); // ���� ������ �� 401
    }
);


export default apiClient;
