import axios from "axios";
import {
  InternoMicroserviceClient,
  TokenModelDTO,
} from "../API/Interno-Microservice-Client/Interno-Microservice-Client";
import { config } from "../config";
import { getApiErrorMessage } from "./getApiErrorMessage";

export {
  config,
  getBannerImageUrl,
  getCategoryImageUrl,
  getProductImageUrl,
} from "../config";

export const API = axios.create({
  baseURL: config.gatewayUrl,
});

API.interceptors.request.use(
  (requestConfig) => {
    const token = localStorage.getItem("Token");

    if (token) {
      requestConfig.headers.Authorization = `Bearer ${token}`;
    }

    return requestConfig;
  },
  (error) => Promise.reject(error),
);

const refreshClient = new InternoMicroserviceClient(config.gatewayUrl, API);

API.interceptors.response.use(
  (response) => response,
  async (error) => {
    const isRedirected = localStorage.getItem("isRedirected");
    if (error.response)
      if (error.response.status === 401 && !isRedirected) {
        const token = localStorage.getItem("Token");
        const refreshToken = localStorage.getItem("RefreshToken");
        if (token && refreshToken) {
          try {
            const response = await refreshClient.refreshToken(
              new TokenModelDTO({
                token: token,
                refreshToken: refreshToken,
              }),
            );
            if (response.isSuccess) {
              localStorage.setItem("Token", response.data?.token!);
              localStorage.setItem(
                "RefreshToken",
                response.data?.refreshToken!,
              );

              window.location.href = "/Home";
            }
          } catch (err) {
            console.error(getApiErrorMessage(err));
          }
        } else {
          localStorage.clear();
          localStorage.setItem("isRedirected", "true");
          window.location.href = "/Home/Login";
        }
      }

    return Promise.reject(error);
  },
);
