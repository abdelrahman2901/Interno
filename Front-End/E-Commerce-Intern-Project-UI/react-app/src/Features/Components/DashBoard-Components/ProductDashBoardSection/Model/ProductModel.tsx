import React, { useEffect, useState } from "react";
import "../../Shared/css/Model.css";
import { CategoryResponse, ColorResponse, FileParameter, InternoMicroserviceClient, ProductDetails_vw, SizeResponse, SubCategoryResponse } from "../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API } from "../../../../../Core/Shared/API-URL";
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
  const Client = new InternoMicroserviceClient(undefined, API);
  const [categories, setCategories] = useState<SubCategoryResponse[]>([]);
  const [subcategories, setSubCategories] = useState<CategoryResponse[]>([]);
  const [product, setProduct] = useState<ProductDetails_vw>();
  const [colors, setColors] = useState<ColorResponse[]>([]);
  const [sizes, setSizes] = useState<SizeResponse[]>([]);
  const [isSubCatEmpty, setIsSubCatEmpty] = useState<boolean>(true);
  const [colorHex, setColorHex] = useState("#000000");
  const [hasPickedColor, setHasPickedColor] = useState(false);
  const [isFormSubmitted, setIsFormSubmitted] = useState(false);
  const [errors, setErrors] = useState({
    productName: "",
    parentCategory: "",
    categoryID: "",
    color: "",
    sizeID: "",
    price: "",
    salePrice: "",
    stock: "",
    productImage: "",
  });

  function toColorInputValue(hex?: string | null) {
    if (!hex) return "#000000";
    let value = hex.trim();
    if (!value.startsWith("#")) value = `#${value}`;
    if (value.length === 4) {
      value = `#${value[1]}${value[1]}${value[2]}${value[2]}${value[3]}${value[3]}`;
    }
    return /^#[0-9a-fA-F]{6}$/.test(value) ? value : "#000000";
  }

  const loadSizes = async () => {
    try {
      const response = await Client.getAllSizes();
      setSizes(response.data!);
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadColors = async () => {
    try {
      const response = await Client.getAllColors();
      setColors(response.data!);
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadCats = async () => {
    try {
      const response = await Client.getCategoriesWithSubCat();
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
      const response = await Client.productDetailsVW();
      if (response.isSuccess) {
        const found = response.data?.find((p) => p.productID === ProductProps);
        console.log("response ; ", found);
        setProduct(found);
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
    if (!product?.colorID) return;
    const match = colors.find((color) => color.colorID === product.colorID);
    if (match?.colorHexCode) {
      setColorHex(toColorInputValue(match.colorHexCode));
      setHasPickedColor(true);
    }
  }, [product?.colorID, colors]);

  useEffect(() => {
    if (product) {
      const subCats: CategoryResponse[] =
        categories.find((i) => i.categoryID === product?.parentcategoryID)
          ?.subCategories ?? [];

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
    const subCats: CategoryResponse[] =
      categories.find((i) => i.categoryID === e.target.value)?.subCategories ??
      [];

    setProduct(
      (prev) =>
        new ProductDetails_vw({
          ...prev,
          parentcategoryID: e.target.value,
          categoryID: "",
        }),
    );

    if (!subCats) {
      setSubCategories([]);
      return;
    }
    setIsSubCatEmpty(false);
    setSubCategories(subCats);
  }

  function isProductValid(
    productName: string,
    categoryID: string,
    colorID: string | null,
    sizeID: string,
    price: number,
    salePriceValue: string,
    salePrice: number,
    stockValue: string,
    stock: number,
    productImage: FileParameter | undefined,
  ) {
    const hasColor = !!colorID || hasPickedColor;
    const nextErrors = {
      productName: productName ? "" : "Product Name Cant Be Empty",
      parentCategory: product?.parentcategoryID
        ? ""
        : "Category Cant Be Empty",
      categoryID: categoryID ? "" : "Sub Category Cant Be Empty",
      color: hasColor ? "" : "Color Cant Be Empty",
      sizeID: sizeID ? "" : "Size Cant Be Empty",
      price: price > 0 ? "" : "Price Must Be Greater Than 0",
      salePrice:
        salePriceValue !== "" && !Number.isNaN(salePrice) && salePrice >= 0
          ? ""
          : "Sale Price Cant Be Empty",
      stock:
        stockValue !== "" && !Number.isNaN(stock) && stock >= 0
          ? ""
          : "Stock Cant Be Empty",
      productImage:
        product?.productID || productImage
          ? ""
          : "Product Image Cant Be Empty",
    };
    setErrors(nextErrors);
    return !Object.values(nextErrors).some((message) => message);
  }

  function OnSubmitProduct(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setIsFormSubmitted(true);
    const form = new FormData(e.currentTarget);

    const imageFile = form.get("productImage") as File;
    const productImage: FileParameter | undefined =
      imageFile && imageFile.size > 0
        ? { data: imageFile, fileName: imageFile.name }
        : undefined;
    const productName = (form.get("productName") as string)?.trim() ?? "";
    const categoryID = form.get("productCategoryID") as string;
    const price = Number(form.get("productPrice") as string);
    const salePriceValue = form.get("SalePrice") as string;
    const salePrice = Number(salePriceValue);
    const sizeID = form.get("ProductSizeID") as string;
    const selectedColorID = form.get("productColorID") as string;
    const colorID = selectedColorID || null;
    const colorHexCode = selectedColorID ? null : colorHex;
    const stockValue = form.get("Stock") as string;
    const stock = Number(stockValue);
    const active = form.get("productActive") === "on";

    if (
      !isProductValid(
        productName,
        categoryID,
        colorID,
        sizeID,
        price,
        salePriceValue,
        salePrice,
        stockValue,
        stock,
        productImage,
      )
    ) {
      return;
    }

    const saveProduct = async () => {
      try {
        const response =
          product?.productID !== undefined
            ? await Client.productPUT(
                product.productID,
                productName,
                categoryID,
                price,
                sizeID,
                colorID,
                colorHexCode,
                salePrice,
                stock,
                active,
                productImage,
              )
            : await Client.productPOST(
                productName,
                categoryID,
                price,
                salePrice,
                sizeID,
                colorID,
                colorHexCode,
                stock,
                active,
                productImage,
              );
        console.log(response);
        onAddUpdateProduct();
      } catch (err) {
        if (err) console.error(err);
      }
    };
    saveProduct();
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
          <form onSubmit={OnSubmitProduct} noValidate>
            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Product Name *</label>
                <input
                  defaultValue={
                    product?.productName ? product?.productName : ""
                  }
                  type="text"
                  name="productName"
                  placeholder="e.g., Linen Wrap Dress"
                  onChange={() =>
                    setErrors((prev) => ({ ...prev, productName: "" }))
                  }
                />
                <span className="error">
                  {isFormSubmitted ? errors.productName : ""}
                </span>
              </div>

              <div className="Custom-form-group">
                <label>Category *</label>
                <select
                  onChange={(e) => {
                    SelectCat(e);
                    setErrors((prev) => ({
                      ...prev,
                      parentCategory: "",
                      categoryID: "",
                    }));
                  }}
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
                <span className="error">
                  {isFormSubmitted ? errors.parentCategory : ""}
                </span>
              </div>
              <div className="Custom-form-group">
                <label>Sub Category *</label>
                <select
                  name="productCategoryID"
                  onChange={(e) => {
                    setProduct(
                      (prev) =>
                        new ProductDetails_vw({
                          ...prev,
                          categoryID: e.target.value,
                        }),
                    );
                    setErrors((prev) => ({ ...prev, categoryID: "" }));
                  }}
                  value={product?.categoryID ? product.categoryID : ""}
                >
                  <option value="">Select SubCategory</option>
                  {subcategories.map((sub) => (
                    <option key={sub.categoryID} value={sub.categoryID}>
                      {sub.categoryName}
                    </option>
                  ))}
                </select>
                <span className="error">
                  {isFormSubmitted ? errors.categoryID : ""}
                </span>
              </div>
            </div>

            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Color *</label>
                <div className="Custom-color-input-group">
                  <input
                    type="color"
                    name="productColorHex"
                    value={colorHex}
                    onChange={(e) => {
                      const selectedHex = e.target.value;
                      setColorHex(selectedHex);
                      setHasPickedColor(true);
                      setProduct(
                        (prev) =>
                          new ProductDetails_vw({
                            ...prev,
                            colorID: "",
                          }),
                      );
                      setErrors((prev) => ({ ...prev, color: "" }));
                    }}
                  />
                  <select
                    onChange={(e) => {
                      const selectedID = e.target.value;
                      const match = colors.find(
                        (color) => color.colorID === selectedID,
                      );
                      setProduct(
                        (prev) =>
                          new ProductDetails_vw({
                            ...prev,
                            colorID: selectedID,
                          }),
                      );
                      if (match?.colorHexCode) {
                        setColorHex(toColorInputValue(match.colorHexCode));
                        setHasPickedColor(true);
                      }
                      setErrors((prev) => ({ ...prev, color: "" }));
                    }}
                    value={product?.colorID ? product.colorID : ""}
                    name="productColorID"
                  >
                    <option value="">Select Product Color</option>
                    {colors.map((color) => (
                      <option key={color.colorID} value={color.colorID}>
                        {color.colorName ?? color.colorHexCode}
                      </option>
                    ))}
                  </select>
                </div>
                <span className="error">
                  {isFormSubmitted ? errors.color : ""}
                </span>
              </div>
              <div className="Custom-form-group">
                <label>Size *</label>
                <select
                  onChange={(e) => {
                    setProduct(
                      (prev) =>
                        new ProductDetails_vw({
                          ...prev,
                          sizeID: e.target.value,
                        }),
                    );
                    setErrors((prev) => ({ ...prev, sizeID: "" }));
                  }}
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
                <span className="error">
                  {isFormSubmitted ? errors.sizeID : ""}
                </span>
              </div>
            </div>

            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Price ($) *</label>
                <input
                  defaultValue={product?.price ? product.price : ""}
                  type="number"
                  name="productPrice"
                  placeholder="99.00"
                  step="0.01"
                  onChange={() =>
                    setErrors((prev) => ({ ...prev, price: "" }))
                  }
                />
                <span className="error">
                  {isFormSubmitted ? errors.price : ""}
                </span>
              </div>
              <div className="Custom-form-group">
                <label>Sale Price ($) *</label>
                <input
                  defaultValue={product?.salePrice ? product.salePrice : ""}
                  type="number"
                  name="SalePrice"
                  placeholder="120.00"
                  step="0.01"
                  onChange={() =>
                    setErrors((prev) => ({ ...prev, salePrice: "" }))
                  }
                />
                <span className="error">
                  {isFormSubmitted ? errors.salePrice : ""}
                </span>
              </div>
              <div className="Custom-form-group">
                <label>Stock Quantity *</label>
                <input
                  defaultValue={product?.stock ? product.stock : ""}
                  type="number"
                  name="Stock"
                  placeholder="100"
                  onChange={() =>
                    setErrors((prev) => ({ ...prev, stock: "" }))
                  }
                />
                <span className="error">
                  {isFormSubmitted ? errors.stock : ""}
                </span>
              </div>
            </div>
            <div className="Custom-form-row">
              <div className="Custom-form-group">
                <label>Image URL *</label>
                <input
                  type="file"
                  name="productImage"
                  onChange={() =>
                    setErrors((prev) => ({ ...prev, productImage: "" }))
                  }
                />
                <span className="error">
                  {isFormSubmitted ? errors.productImage : ""}
                </span>
              </div>
            </div>

            <div className="Custom-form-group">
              <label>
                <input
                  type="checkbox"
                  name="productActive"
                  onChange={(e) => {
                    setProduct(
                      (prev) =>
                        new ProductDetails_vw({
                          ...prev,
                          active: e.target.checked,
                        }),
                    );
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
