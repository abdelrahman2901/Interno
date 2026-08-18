import { IResponse } from "../../DTO/API-Response/IResponse";
import { IUser } from "../../Interface/User-Interfaces/IUser";
import { API } from "../../Shared/API-URL";

const Base_API = "/User";
export const GetCurrentUser = async (
  UserID: string,
): Promise<IResponse<IUser>> => {
  const response = await API.get<IResponse<IUser>>(
    `${Base_API}/GetUserDetailsByID/${UserID}`,
  );
  return response.data;
};
