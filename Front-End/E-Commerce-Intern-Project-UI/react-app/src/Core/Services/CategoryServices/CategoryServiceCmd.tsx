import { API } from "../../Shared/API-URL";
import { CategoryRequest } from "../../DTO/CategoryDTO/CategoryRequestDto";
import { ICategory } from "../../Interface/Category/ICategory";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { CategoryUpdateRequest } from "../../DTO/CategoryDTO/CategoryUpdateRequest";

const Base_API = "/Category";

export const AddCategory_serv = async (
  category: CategoryRequest,
): Promise<IResponse<ICategory | undefined>> => {
  const formdata = new FormData();
  formdata.append("CategoryImage", category.categoryImage);
  if (category.parentCategoryID) {
    formdata.append("ParentCategoryID", category.parentCategoryID);
  }
  formdata.append("CategoryName", category.categoryName);

  const response = await API.post<IResponse<ICategory>>(Base_API, formdata, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
  return response.data;
};
export const UpdateCategory_serv = async (
  category: CategoryUpdateRequest,
): Promise<IResponse<ICategory | undefined>> => {
  const formdata = new FormData();
  if (category.categoryImage) {
    formdata.append("CategoryImage", category.categoryImage);
  }
  if (category.parentCategoryID) {
    formdata.append("ParentCategoryID", category.parentCategoryID);
  }
  formdata.append("CategoryName", category.categoryName);
  formdata.append("CategoryID", category.categoryID);

  const response = await API.put<IResponse<ICategory>>(
    `${Base_API}/UpdateCategory`,
    formdata,
    {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    },
  );
  return response.data;
};

export const DeleteCategory_serv = async (
  CatID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/DeleteCategory/${CatID}`,
  );
  return response.data;
};
