import { IResponse } from "../../DTO/API-Response/IResponse";
import { API } from "../../Shared/API-URL";
import { LoginRequest } from "../../DTO/AuthDTO/LoginRequest";
import { AuthTokenResponse } from "../../Interface/Auth/AuthToken";
import { RegisterRequest } from "../../DTO/AuthDTO/RegisterRequest";
import { IUser } from "../../Interface/User-Interfaces/IUser";
import { UpdateUserRequest } from "../../DTO/UserDTO/UpdateUserRequest";
import { changePasswordDto } from "../../DTO/UserDTO/ChangePasswordDto";
import { checkPasswordDto } from "../../DTO/UserDTO/checkPasswordDto";
import { RefreshTokenRequest } from "../../DTO/AuthDTO/RefresjTokenRequest";
const Base_API = `/Auth`;
export const Login = async (
  Request: LoginRequest,
): Promise<IResponse<AuthTokenResponse>> => {
  const response = await API.post<IResponse<AuthTokenResponse>>(
    `${Base_API}/LoginUser`,
    Request,
  );
  return response.data;
};

export const Register = async (
  Request: RegisterRequest,
): Promise<IResponse<AuthTokenResponse>> => {
  const response = await API.post<IResponse<AuthTokenResponse>>(
    `${Base_API}/Register`,
    Request,
  );
  return response.data;
};
export const RefreshToken = async (
  Request: RefreshTokenRequest,
): Promise<IResponse<AuthTokenResponse>> => {
  const response = await API.post<IResponse<AuthTokenResponse>>(
    `${Base_API}/RefreshToken`,
    Request,
  );
  return response.data;
};
export const SignOut = async (userID: string): Promise<IResponse<boolean>> => {
  const response = await API.post<IResponse<boolean>>(
    `${Base_API}/SignOutUser/${userID}`,
  );
  return response.data;
};
export const UpdateUser = async (
  UpdateRequest: UpdateUserRequest,
): Promise<IResponse<IUser>> => {
  const response = await API.put<IResponse<IUser>>(
    `${Base_API}/UpdateUser`,
    UpdateRequest,
  );
  return response.data;
};
export const ChangePassword = async (
  ChangePassRequest: changePasswordDto,
): Promise<IResponse<IUser>> => {
  const response = await API.put<IResponse<IUser>>(
    `${Base_API}/ChangeUserPassword`,
    ChangePassRequest,
  );
  return response.data;
};
export const CheckPassword = async (
  request: checkPasswordDto,
): Promise<IResponse<boolean>> => {
  const response = await API.get<IResponse<boolean>>(
    `${Base_API}/CheckPassword`,
    { params: request },
  );
  return response.data;
};
export const DeleteAccount_serv = async (
  userID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/DeleteUser/${userID}`,
  );
  return response.data;
};
