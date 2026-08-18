import { CartModel } from "../../DTO/CartDTO/CartModel";
import { API } from "../../Shared/API-URL";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { CartDetails } from "../../DTO/CartDTO/CartDetails";
import { CartRequest } from "../../DTO/CartDTO/CartRequest";

const Base_API = `/Cart`;

export const GetUserCartItemsDetails = async (
  userID: string,
): Promise<IResponse<CartDetails>> => {
  const response = await API.get<IResponse<CartDetails>>(
    `${Base_API}/GetUserCartItemsDetails/${userID}`,
  );
  return response.data;
};

export const CreateCart = async (
  request: CartRequest,
): Promise<IResponse<CartModel>> => {
  const response = await API.post<IResponse<CartModel>>(
    `${Base_API}/CreateCart`,
    request,
  );
  return response.data;
};
