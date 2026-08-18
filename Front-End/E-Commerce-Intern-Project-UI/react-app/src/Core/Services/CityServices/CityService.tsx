import { IResponse } from "../../DTO/API-Response/IResponse";
import { API } from "../../Shared/API-URL";
import { CityRequest } from "../../DTO/CityDTO/CityRequest";
import { CityModel } from "../../DTO/CityDTO/CityModel";

const Base_API = `/Cities`;

export const addCity = async (
  request: CityRequest,
): Promise<IResponse<boolean>> => {
  const response = await API.post<IResponse<boolean>>(Base_API, request);
  return response.data;
};
export const deleteCity = async (
  cityID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(`${Base_API}/${cityID}`);
  return response.data;
};
export const updateCity_serv = async (
  City: CityModel,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(Base_API, City);
  return response.data;
};

export const getCities = async (): Promise<IResponse<CityModel[]>> => {
  const response = await API.get<IResponse<CityModel[]>>(Base_API);
  return response.data;
};
export const getCityByID = async (
  ciyID: string,
): Promise<IResponse<CityModel>> => {
  const response = await API.get<IResponse<CityModel>>(`${Base_API}/${ciyID}`);
  return response.data;
};
