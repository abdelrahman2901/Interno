import { IResponse } from "../../DTO/API-Response/IResponse";
import { ShippingCostDetails } from "../../DTO/ShippingCostDTO/ShippingCostDetails";
import { ShippingCostModel } from "../../DTO/ShippingCostDTO/ShippingCostModel";
import { ShippingCostRequest } from "../../DTO/ShippingCostDTO/ShippingCostRequest";
import { API } from "../../Shared/API-URL";

const Base_API = "/ShippingCosts";
export const GetAllShippingCostDetails = async (): Promise<
  IResponse<ShippingCostDetails[]>
> => {
  const response = await API.get<IResponse<ShippingCostDetails[]>>(Base_API);
  return response.data;
};
export const GetShippingCostDetailsByAreaID = async (
  areaID: string,
): Promise<IResponse<ShippingCostDetails>> => {
  const response = await API.get<IResponse<ShippingCostDetails>>(
    `${Base_API}/GetShippingCostDetailsByAreaID/${areaID}`,
  );
  return response.data;
};

export const CreateNewShippingCost = async (
  request: ShippingCostRequest,
): Promise<IResponse<boolean>> => {
  const response = await API.post<IResponse<boolean>>(Base_API, request);
  return response.data;
};

export const UpdateShippingCost = async (
  request: ShippingCostModel,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/UpdateShippingCost`,
    request,
  );
  return response.data;
};

export const DeleteShippingCost = async (
  ShippingCostID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/${ShippingCostID}`,
  );
  return response.data;
};
