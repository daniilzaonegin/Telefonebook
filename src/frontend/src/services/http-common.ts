import { useUserDataStore } from "@/store/usersStore";
import axios, {
  AxiosInstance
} from "axios";
import { useRouter } from "vue-router";

const apiClient: AxiosInstance = axios.create({
  baseURL: process.env.VUE_APP_API_SERVER,
  headers: {
    "Content-Type": "application/json",
  },
});

let authTokenInterceptorId = 0;
let nonAuthInterceptorId = 0;

export function registerAuthTokenInterceptor(token: string) {
  authTokenInterceptorId = apiClient.interceptors.request.use(
    function (config: any) {
      config.headers.Authorization = "Bearer " + token;
      return config;
    },
    function (error: any) {
      if (error.response.status === 401) {
        const usersStore = useUserDataStore();
        usersStore.logOut();
      }
      return Promise.reject(error);
    }
  );
  nonAuthInterceptorId = apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response.status === 401) {
        const usersStore = useUserDataStore();
        const router = useRouter();
        usersStore.logOut();
        router.push("/login");
      }
      return error;
    }
  );
}

export function deregisterAuthTokenInterceptor() {
  apiClient.interceptors.request.eject(authTokenInterceptorId);
  apiClient.interceptors.response.eject(nonAuthInterceptorId);
}

export default apiClient;
