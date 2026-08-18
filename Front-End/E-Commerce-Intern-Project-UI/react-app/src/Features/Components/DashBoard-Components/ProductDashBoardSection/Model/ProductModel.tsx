import React, { useEffect, useState } from "react";
import "../../Shared/css/Model.css";
import { ProductRequest } from "../../../../../Core/DTO/ProductDTO/ProductRequestDto";
import { ProductUpdateRequest } from "../../../../../Core/DTO/ProductDTO/ProductUpdateRequest";
import { ICategory } from "../../../../../Core/Interface/Category/ICategory";
import { ISubCategoryDetails } from "../../../../../Core/Interface/Category/ISubCateogryDetails";
import { IColors } from "../../../../../Core/Interface/Colors/IColors";
import { IProductDetails } from "../../../../../Core/Interface/Products/IProductDetails";
import { ISize } from "../../../../../Core/Interface/Size/ISize";
import { GetSubCategoryDetails } from "../../../../../Core/Services/CategoryServices/CategoryServiceQuery";
import { GetColors_Serv } from "../../../../../Core/Services/ColorsServices/ColorService_Query";
import {
  AddProduct_Serv,
  UpdateProduct_Serv,
} from "../../../../../Core/Services/ProductServices/ProductServiceCmd";
import { GetSizes_Serv } from "../../../../../Core/Services/SizeServices/SizeService_Query";
import { GetProductByID } from "../../../../../Core/Services/ProductServices/ProductServiceQuery";

type props = {
  onClose: () => void;
  onAddUpdateProduct: () => void;
  ProductProps: string | null;
};
export default function ProductModel({
  onClose,
  onAddUpdateProduct,
  ProductProps,
}: props) {
  const [categories, setCategories] = useState<ISubCategoryDetails[]>([]);
  const [subcategories, setSubCategories] = useState<ICategory[]>([]);
  const [product, setProduct] = useState<IProductDetails>();
  const [colors, setColors] = useState<IColors[]>([]);
  const [sizes, setSizes] = useState<ISize[]>([]);
  const [isSubCatEmpty, setIsSubCatEmpty] = useState<boolean>(true);

  const loadSizes = async () => {
    try {
      const response = await GetSizes_Serv();
      setSizes(response.data!);
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadColors = async () => {
    try {
      const response = await GetColors_Serv();
      setColors(response.data!);
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadCats = async () => {
    try {
      const response = await GetSubCategoryDetails();
      console.log(response.data);

      setCategories(response.data!);
    } catch (err) {
      if (err) console.error(err);
    }
  };

  useEffect(() => {
    loadColors();
    loadSizes();
  }, []);

  useEffect(() => {
    loadCats();
  }, []);

  const loadProduct = async () => {
    try {
      const response = await GetProductByID(ProductProps!);
      if (response.isSuccess) {
        console.log("response ; ", response.data);

        setProduct(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (ProductProps) {
      loadProduct();
    }
  }, [ProductProps]);

  useEffect(() => {
    if (product) {
      const subCats: ICategory[] = categories.find(
        (i) => i.categoryID === product?.parentcategoryID,
      )?.subCategories!;

      if (!subCats) {
        setSubCategories([]);
        return;
      }
      setIsSubCatEmpty(false);
      setSubCategories(subCats);
      console.log("sub of the update product", subCats);
    }
  }, [product, categories]);

  function SelectCat(e: React.ChangeEvent<HTMLSelectElement>) {
    const subCats: ICategory[] = categories.find(
      (i) => i.categoryID === e.target.value,
    )?.subCategories!;

    setProduct((prev) => ({ ...prev!, parentcategoryID: e.target.value }));

    if (!subCats) {
      setSubCategories([]);
      return;
    }
    setIsSubCatEmpty(false);
    setSubCategories(subCats);
  }

  function OnSubmitProduct(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);

    const Request: ProductRequest = {
      price: Number(form.get("productPrice") as string),
      productImage: form.get("productImage") as File,
      stock: Number(form.get("Stock") as string),
      sizeID: form.get("ProductSizeID") as string,
      colorID: form.get("productColorID") as string,
      active: form.get("productActive") === "on" ? true : false,
      salePrice: Number(form.get("SalePrice") as string),
      categoryID: form.get("productCategoryID") as string,
      productName: form.get("productName") as string,
    };
    console.log("product ; ", product?.productID);

    const UpdateReq: ProductUpdateRequest = {
      ...Request,
      productID: product?.productID!,
    };
    console.log(Request);

    const addProdcut = async () => {
      try {
        const response = await AddProduct_Serv(Request);
        console.log(response);

        onAddUpdateProduct();
      } catch (err) {
        if (err) console.error(err);
      }
    };
    const UpdateProdcut = async () => {
      try {
        const response = await UpdateProduct_Serv(UpdateReq);
        onAddUpdateProduct();
        console.log(response);

        onAddUpdateProduct();
      } catch (err) {
        if (err) console.error(err);
      }
    };
    if (product?.productID !== undefined) {
      UpdateProdcut();
    } else {
      addProdcut();
    }
  }
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-large">
          <div className="Custom-modal-header">
            <h3>Add Product</h3>
            <button className="close-btn" onClick={onClose}>
              &times;
            </button>
          </div>
          <form onSubmit={OnSubmitProduct}>
            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Product Name *</label>
                <input
                  defaultValue={
                    product?.productName ? product?.productName : ""
                  }
                  type="text"
                  name="productName"
                  required
                  placeholder="e.g., Linen Wrap Dress"
                />
              </div>

              <div className="Custom-form-group">
                <label>Category *</label>
                <select
                  required
                  onChange={SelectCat}
                  value={
                    product?.parentcategoryID ? product?.parentcategoryID : ""
                  }
                >
                  <option value="">Select Category</option>
                  {categories.map((cat) => (
                    <option key={cat.categoryID} value={cat.categoryID}>
                      {cat.categoryName}
                    </option>
                  ))}
                </select>
              </div>
              {!isSubCatEmpty && (
                <div className="Custom-form-group">
                  <label>Sub Category *</label>
                  <select
                    name="productCategoryID"
                    required
                    onChange={(e) =>
                      setProduct((prev) => ({
                        ...prev!,
                        categoryID: e.target.value,
                      }))
                    }
                    value={product?.categoryID ? product.categoryID : ""}
                  >
                    <option value="">Select SubCategory</option>
                    {subcategories.map((sub) => (
                      <option key={sub.categoryID} value={sub.categoryID}>
                        {sub.categoryName}
                      </option>
                    ))}
                  </select>
                </div>
              )}
            </div>

            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Color</label>
                <select
                  onChange={(e) =>
                    setProduct((prev) => ({
                      ...prev!,
                      colorID: e.target.value,
                    }))
                  }
                  value={product?.colorID ? product.colorID : ""}
                  name="productColorID"
                  required
                >
                  <option value="">Select Product Color</option>
                  {colors.map((color) => (
                    <option key={color.colorID} value={color.colorID}>
                      {color.colorName}
                    </option>
                  ))}
                </select>
              </div>
              <div className="Custom-form-group">
                <label>Size</label>
                <select
                  onChange={(e) =>
                    setProduct((prev) => ({
                      ...prev!,
                      sizeID: e.target.value,
                    }))
                  }
                  value={product?.sizeID ? product.sizeID : ""}
                  name="ProductSizeID"
                >
                  <option value="">Select Product Size</option>
                  {sizes.map((size) => (
                    <option key={size.sizeID} value={size.sizeID}>
                      {size.sizeName}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Price ($) *</label>
                <input
                  defaultValue={product?.price ? product.price : ""}
                  type="number"
                  name="productPrice"
                  required
                  placeholder="99.00"
                  step="0.01"
                />
              </div>
              <div className="Custom-form-group">
                <label>Sale Price ($)</label>
                <input
                  defaultValue={product?.salePrice ? product.salePrice : ""}
                  type="number"
                  name="SalePrice"
                  placeholder="120.00"
                  step="0.01"
                />
              </div>
              <div className="Custom-form-group">
                <label>Stock Quantity *</label>
                <input
                  defaultValue={product?.stock ? product.stock : ""}
                  type="number"
                  name="Stock"
                  required
                  placeholder="100"
                />
              </div>
            </div>
            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Image URL *</label>
                <input
                  type="file"
                  name="productImage"
                  required={product ? false : true}
                />
              </div>
            </div>

            <div className="Custom-form-group">
              <label>
                <input
                  type="checkbox"
                  name="productActive"
                  onChange={(e) => {
                    setProduct((prev) => ({
                      ...prev!,
                      active: e.target.checked,
                    }));
                  }}
                  defaultChecked={product?.active}
                />
                Active (visible on store)
              </label>
            </div>

            <div className="Custom-form-actions">
              <button
                type="button"
                className="btn-secondary"
                id="cancelProductBtn"
                onClick={onClose}
              >
                Cancel
              </button>
              <button type="submit" className="btn-primary">
                Save Product
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
