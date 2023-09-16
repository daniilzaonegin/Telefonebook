import { AuthInfo } from "@/models/authInfo";
import { LoginModel } from "@/models/login";
import contactsApiClient from "@/services/contacts-api-client";
import {
  deregisterAuthTokenInterceptor,
  registerAuthTokenInterceptor,
} from "@/services/http-common";
import axios, { AxiosError } from "axios";
import { defineStore } from "pinia";

export const useUserDataStore = defineStore("userData", {
  state: () => ({
    authInfo: {} as AuthInfo,
    error: "" as string,
  }),
  getters: {
    getAuthInfo: (state) => state.authInfo,
    errorMessage: (state) => state.error,
  },
  actions: {
    async loginUser(userData: LoginModel) {
      try {
        const authData = await contactsApiClient.authenticate(userData);
        this.authInfo.currentUser = userData.username;
        this.authInfo.token = authData.token;
        registerAuthTokenInterceptor(authData.token);
        localStorage.setItem("token", this.authInfo.token);
        localStorage.setItem("user", this.authInfo.currentUser);
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const axiosError = error as AxiosError;
          this.error = axiosError.message;
        } else {
          const typedError = error as Error;
          this.error = typedError.message;
        }
        throw error;
      }
    },
    autoLogin() {
      const token = localStorage.getItem("token");
      const user = localStorage.getItem("user");
      if (user && token) {
        registerAuthTokenInterceptor(token);
        this.authInfo = {currentUser: user, token: token };
      }
    },
    logOut() {
      this.authInfo = {} as AuthInfo;
      deregisterAuthTokenInterceptor();

      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },
  },
});
