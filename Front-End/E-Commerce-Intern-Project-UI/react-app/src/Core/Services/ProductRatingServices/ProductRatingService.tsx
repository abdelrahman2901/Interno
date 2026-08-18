import { IResponse } from "../../DTO/API-Response/IResponse";
import { ProductRatesDetails } from "../../DTO/ProductRatesDTO/ProductRatesDetails";
import { ProductRatesRequets } from "../../DTO/ProductRatesDTO/ProductRatesRequest";
import { API } from "../../Shared/API-URL";

const Base_API = "/ProductRates";

export const GetProductRatesForProduct = async (
  ProductID: string,
): Promise<IResponse<ProductRatesDetails[]>> => {
  const response = await API.get<IResponse<ProductRatesDetails[]>>(
    `${Base_API}/${ProductID}`,
  );

  return response.data;
};
export const GetAllProductRates = async (): Promise<
  IResponse<ProductRatesDetails[]>
> => {
  const response = await API.get<IResponse<ProductRatesDetails[]>>(Base_API);
  return response.data;
};
export const GetUserRating = async (
  userID: string,
): Promise<IResponse<ProductRatesDetails[]>> => {
  const response = await API.get<IResponse<ProductRatesDetails[]>>(
    `${Base_API}/GetUserRating/${userID}`,
  );
  return response.data;
};
export const CreateProductRateList = async (
  request: ProductRatesRequets[],
): Promise<IResponse<boolean>> => {
  const response = await API.post<IResponse<boolean>>(Base_API, request);
  return response.data;
};
export const DeleteProductRate = async (
  RateID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(`${Base_API}/${RateID}`);
  return response.data;
};
