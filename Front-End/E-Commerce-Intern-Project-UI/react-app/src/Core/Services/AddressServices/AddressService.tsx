import { IResponse } from "../../DTO/API-Response/IResponse";
import { API } from "../../Shared/API-URL";
import { AddressRequest } from "../../DTO/AddressDTO/AddressRequest";
import { AddressModel } from "../../DTO/AddressDTO/AddressModel";
import { AddressDetails } from "../../DTO/AddressDTO/AddressDetails";

const Base_API = `/Address`;

export const AddAddress = async (
  request: AddressRequest,
): Promise<IResponse<AddressRequest>> => {
  const response = await API.post<IResponse<AddressRequest>>(
    `${Base_API}/CreateAddress`,
    request,
  );
  return response.data;
};

export const DeleteAddress_serv = async (
  addressID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/DeleteAddress/${addressID}`,
  );
  return response.data;
};

export const GetUserAddress = async (
  AddressID: string,
): Promise<IResponse<AddressModel>> => {
  const response = await API.get<IResponse<AddressModel>>(
    `${Base_API}/GetUserAddressByID/${AddressID}`,
  );
  return response.data;
};
export const GetUserAddresess = async (
  UserID: string,
): Promise<IResponse<AddressDetails[]>> => {
  const response = await API.get<IResponse<AddressDetails[]>>(
    `${Base_API}/GetUserAddress/${UserID}`,
  );
  return response.data;
};

export const updateAddress = async (
  address: AddressModel,
): Promise<IResponse<AddressRequest>> => {
  const response = await API.put<IResponse<AddressRequest>>(
    `${Base_API}/UpdateAddress`,
    address,
  );
  return response.data;
};
