import { API } from "../../Shared/API-URL";
import { ICategory } from "../../Interface/Category/ICategory";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { ISubCategoryDetails } from "../../Interface/Category/ISubCateogryDetails";

const Base_API = "/CategoryDetails";

export const GetCategoryByID = async (
  CatID: string,
): Promise<IResponse<ICategory | undefined>> => {
  const response = await API.get<IResponse<ICategory | undefined>>(
    `${Base_API}/${CatID}`,
  );
  return response.data;
};

export const GetSubCategoryDetails = async (): Promise<
  IResponse<ISubCategoryDetails[] | undefined>
> => {
  const response = await API.get<IResponse<ISubCategoryDetails[] | undefined>>(
    `${Base_API}/GetCategoriesWithSubCat`,
  );
  return response.data;
};
