import { API } from "../../Shared/API-URL";
import { IProductDetails } from "../../Interface/Products/IProductDetails";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { FilterPageModel } from "../../../Features/Components/ProductPage-Components/Model/FilterPageModel";

const Base_API = "/ProductDetailsVW";
export const GetProducts = async (): Promise<IResponse<IProductDetails[]>> => {
  const response = await API.get<IResponse<IProductDetails[]>>(Base_API);
  return response.data;
};

export const GetProductByID = async (
  productID: string,
): Promise<IResponse<IProductDetails>> => {
  const response = await API.get<IResponse<IProductDetails>>(
    `${Base_API}/${productID}`,
  );
  return response.data;
};
export const FilterProducts = async (
  filter: FilterPageModel,
): Promise<IResponse<IProductDetails[]>> => {
  const response = await API.get<IResponse<IProductDetails[]>>(
    `${Base_API}/FilterProducts`,
    { params: filter },
  );
  return response.data;
};
