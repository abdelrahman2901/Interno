import { IResponse } from "../../DTO/API-Response/IResponse";
import { CartItemModel } from "../../DTO/CartItemDTO/CartItemModel";
import { CartItemRequest } from "../../DTO/CartItemDTO/CartItemRequest";
import { updateItemQuantity } from "../../DTO/CartItemDTO/UpdateItemQuantity";
import { API } from "../../Shared/API-URL";

const Base_API = `/CartItem`;
export const AddCartItem = async (
  request: CartItemRequest,
): Promise<IResponse<boolean>> => {
  const response = await API.post<IResponse<boolean>>(
    `${Base_API}/AddCartItem`,
    request,
  );
  return response.data;
};
export const AddCartItemList = async (
  request: CartItemRequest[],
): Promise<IResponse<boolean>> => {
  const response = await API.post<IResponse<boolean>>(
    `${Base_API}/AddCartItemList`,
    request,
  );
  return response.data;
};

export const UpdateCartItem = async (
  request: updateItemQuantity,
): Promise<IResponse<CartItemModel>> => {
  const response = await API.put<IResponse<CartItemModel>>(
    `${Base_API}/UpdateCartItem`,
    request,
  );
  return response.data;
};

export const DeleteCartItem = async (
  itemID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/DeleteCartItem/${itemID}`,
  );
  return response.data;
};
