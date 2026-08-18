import axios from "axios";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { API } from "../../Shared/API-URL";
import { AreaModel } from "../../DTO/AreaDTO/AreaModel";
import { AreaRequest } from "../../DTO/AreaDTO/AreaRequest";

const Base_API = `/Areas`;

export const addArea = async (
  request: AreaRequest,
): Promise<IResponse<AreaModel>> => {
  const response = await API.post<IResponse<AreaModel>>(Base_API, request);
  return response.data;
};
export const deleteArea = async (
  areaID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(`${Base_API}/${areaID}`);
  return response.data;
};

export const updateArea = async (
  UpdateRequest: AreaModel,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(Base_API, UpdateRequest);
  return response.data;
};

export const getAreas = async (): Promise<IResponse<AreaModel[]>> => {
  const response = await API.get<IResponse<AreaModel[]>>(Base_API);
  return response.data;
};
export const getAreaByID = async (
  areaID: string,
): Promise<IResponse<AreaModel>> => {
  const response = await API.get<IResponse<AreaModel>>(`${Base_API}/${areaID}`);
  return response.data;
};
