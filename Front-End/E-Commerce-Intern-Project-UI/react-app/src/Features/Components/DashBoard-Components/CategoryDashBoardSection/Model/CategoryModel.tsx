import React, { useEffect, useState } from "react";
import "../../Shared/css/Model.css";
import { CategoryResponse, FileParameter, InternoMicroserviceClient, SubCategoryResponse } from "../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API } from "../../../../../Core/Shared/API-URL";
type props = {
  onClose: () => void;
  categoires: SubCategoryResponse[];
  categoryProps?: string;
  onAddnewCategory: () => void;
};

export default function AddCategoryModel({
  categoires,
  onClose,
  onAddnewCategory,
  categoryProps,
}: props) {
  const Client = new InternoMicroserviceClient(undefined, API);
  const [category, setCategory] = useState<CategoryResponse | undefined>(
    undefined,
  );

  const loadCategory = async () => {
    try {
      const response = await Client.categoryDetails(categoryProps!);
      if (response.isSuccess) {
        setCategory(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (categoryProps) {
      loadCategory();
    } else {
      setCategory(
        new CategoryResponse({
          categoryID: "",
          categoryImageUrl: "",
          categoryName: "",
          parentCategoryID: "",
        }),
      );
    }
  }, [categoryProps]);

  function SetParentCat(e: React.ChangeEvent<HTMLSelectElement>) {
    const value = e.currentTarget.value;

    setCategory((prev) =>
      new CategoryResponse({
        ...prev,
        parentCategoryID: value,
      }),
    );
  }

  function OnSubmitCat(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);

    const imageFile = form.get("categoryImage") as File;
    const categoryImage: FileParameter | undefined =
      imageFile && imageFile.size > 0
        ? { data: imageFile, fileName: imageFile.name }
        : undefined;
    const categoryName = form.get("categoryName") as string;
    const parentCategoryID =
      form.get("ParentID") !== "" ? (form.get("ParentID") as string) : undefined;

    const saveCategory = async () => {
      try {
        const response =
          category?.categoryID !== ""
            ? await Client.updateCategory(
                category?.categoryID,
                categoryName,
                parentCategoryID,
                categoryImage,
              )
            : await Client.createNewCategory(
                categoryName,
                parentCategoryID,
                categoryImage,
              );
        console.log(response);
        onAddnewCategory();
      } catch (err) {
        if (err) console.error(err);
      }
    };
    saveCategory();
  }

  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content">
          <div className="Custom-modal-header">
            <h3>Add Category</h3>
            <button className="close-btn" onClick={onClose}>
              &times;
            </button>
          </div>
          <form onSubmit={OnSubmitCat}>
            <div className="Custom-form-group">
              <label>Category Name *</label>
              <input
                defaultValue={category?.categoryName ?? ""}
                type="text"
                name="categoryName"
                required
                placeholder="e.g., Women, Men, Kids"
              />
            </div>
            <div className="Custom-form-group">
              <label>Parent Category</label>
              <select
                name="ParentID"
                onChange={SetParentCat}
                value={category?.parentCategoryID ?? ""}
              >
                <option value="">None (Main Category)</option>
                {categoires.map((cat) => (
                  <option key={cat.categoryID} value={cat.categoryID}>
                    {cat.categoryName}
                  </option>
                ))}
              </select>
            </div>
            <div className="Custom-form-group">
              <label>Category Image *</label>
              <input
                type="file"
                name="categoryImage"
                placeholder="e.g., Women, Men, Kids"
                // value={
                //   category
                //     ? new File([], "http://localhost:5000/CategoryImages/" +
                //       category?.categoryImageUrl)
                //     : new File([],'')
                // }
              />
            </div>
            <div className="Custom-form-actions">
              <button type="button" className="btn-secondary" onClick={onClose}>
                Cancel
              </button>
              <button type="submit" className="btn-primary">
                Save Category
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
