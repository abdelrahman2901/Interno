import { API } from "../../Shared/API-URL";
import { ProductRequest } from "../../DTO/ProductDTO/ProductRequestDto";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { IProduct } from "../../Interface/Products/IProduct";
import { ProductUpdateRequest } from "../../DTO/ProductDTO/ProductUpdateRequest";

const Base_API = "/Product";
export const AddProduct_Serv = async (
  product: ProductRequest,
): Promise<IResponse<IProduct>> => {
  const form = new FormData();
  form.append("Active", product.active ? "true" : "false");
  form.append("SalePrice", product.salePrice.toString());
  form.append("Price", product.price.toString());
  form.append("CategoryID", product.categoryID);
  form.append("SizeID", product.sizeID);
  form.append("ColorID", product.colorID);
  form.append("Stock", product.stock.toString());
  form.append("ProductImage", product.productImage);
  form.append("ProductName", product.productName);

  const respone = await API.post<IResponse<IProduct>>(Base_API, form);
  return respone.data;
};

export const UpdateProduct_Serv = async (
  product: ProductUpdateRequest,
): Promise<IResponse<IProduct>> => {
  const form = new FormData();
  form.append("ProductID", product.productID);
  form.append("SizeID", product.sizeID);
  form.append("ColorID", product.colorID);
  form.append("Active", product.active ? "true" : "false");
  form.append("SalePrice", product.salePrice.toString());
  form.append("Price", product.price.toString());
  form.append("CategoryID", product.categoryID);
  form.append("Stock", product.stock.toString());
  form.append("ProductImage", product.productImage);
  form.append("ProductName", product.productName);

  const respnse = await API.put<IResponse<IProduct>>(Base_API, form);
  return respnse.data;
};

export const DeleteProduct_Serv = async (
  productID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.delete<IResponse<boolean>>(
    `${Base_API}/${productID}`,
  );
  return response.data;
};
