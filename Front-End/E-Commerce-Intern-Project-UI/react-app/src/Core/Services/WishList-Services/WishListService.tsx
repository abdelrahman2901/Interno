import { IResponse } from "../../DTO/API-Response/IResponse";
import { API } from "../../Shared/API-URL";
import { WishListModel } from "../../DTO/WishList/WishListModel";
import { WishListRequest } from "../../DTO/WishList/WishListRequest";
import { WishListDetailsModel } from "../../DTO/WishList/WishListDetailsModel";

const Base_API = `/WishList`;

export const GetAllUserWishList = async (
  userID: string,
): Promise<IResponse<WishListDetailsModel[]>> => {
  const response = await API.get<IResponse<WishListDetailsModel[]>>(
    `${Base_API}/GetAllUserWishList/${userID}`,
  );
  return response.data;
};

export const CreateWishList = async (
  request: WishListRequest,
): Promise<IResponse<WishListModel>> => {
  const response = await API.post<IResponse<WishListModel>>(
    `${Base_API}/CreateWishList`,
    request,
  );
  return response.data;
};

export const UpdateWishList = async (
  request: WishListModel,
): Promise<IResponse<WishListModel>> => {
  const response = await API.put<IResponse<WishListModel>>(
    `${Base_API}/UpdateWishList`,
    request,
  );
  return response.data;
};

export const DeleteWishList = async (
  WishListID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/DeleteWishList/${WishListID}`,
  );
  return response.data;
};
export const ClearUserWishList = async (
  UserID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/ClearUserWishList/${UserID}`,
  );
  return response.data;
};
