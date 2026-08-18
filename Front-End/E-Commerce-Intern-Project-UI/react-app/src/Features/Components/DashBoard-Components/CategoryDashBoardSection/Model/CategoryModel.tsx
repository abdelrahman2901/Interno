import React, { useEffect, useState } from "react";
import "../../Shared/css/Model.css";
import { CategoryRequest } from "../../../../../Core/DTO/CategoryDTO/CategoryRequestDto";
import { CategoryUpdateRequest } from "../../../../../Core/DTO/CategoryDTO/CategoryUpdateRequest";
import { ICategory } from "../../../../../Core/Interface/Category/ICategory";
import { ISubCategoryDetails } from "../../../../../Core/Interface/Category/ISubCateogryDetails";
import {
  AddCategory_serv,
  UpdateCategory_serv,
} from "../../../../../Core/Services/CategoryServices/CategoryServiceCmd";
import { GetCategoryByID } from "../../../../../Core/Services/CategoryServices/CategoryServiceQuery";

type props = {
  onClose: () => void;
  categoires: ISubCategoryDetails[];
  categoryProps?: string;
  onAddnewCategory: () => void;
};

export default function AddCategoryModel({
  categoires,
  onClose,
  onAddnewCategory,
  categoryProps,
}: props) {
  const [category, setCategory] = useState<
    ISubCategoryDetails | ICategory | undefined
  >(undefined);

  const loadCategory = async () => {
    try {
      const response = await GetCategoryByID(categoryProps!);
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
      setCategory({
        categoryID: "",
        categoryImageUrl: "",
        categoryName: "",
        parentCategoryID: "",
      });
    }
  }, [categoryProps]);

  function SetParentCat(e: React.ChangeEvent<HTMLSelectElement>) {
    const value = e.currentTarget.value;

    setCategory((prev) => ({
      ...prev!,
      parentCategoryID: value,
    }));
  }

  function OnSubmitCat(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);

    const AddNewCategory = async (request: CategoryRequest) => {
      try {
        const response = await AddCategory_serv(request);
        console.log(response);
        onAddnewCategory();
      } catch (err) {
        if (err) console.error(err);
      }
    };
    const UpdateCatRequest = async (request: CategoryUpdateRequest) => {
      try {
        const response = await UpdateCategory_serv(request);
        console.log(response);
        onAddnewCategory();
      } catch (err) {
        if (err) console.error(err);
      }
    };
    console.log(category);

    if (category?.categoryID !== "") {
      const Updaterequest: CategoryUpdateRequest = {
        categoryID: category?.categoryID!,
        categoryName: form.get("categoryName") as string,
        categoryImage: form.get("categoryImage") as File,
        parentCategoryID:
          form.get("ParentID") !== "" ? (form.get("ParentID") as string) : null,
      };
      console.log("Update REuqetst : ", Updaterequest);

      UpdateCatRequest(Updaterequest);
    } else {
      const Newrequest: CategoryRequest = {
        categoryName: form.get("categoryName") as string,
        categoryImage: form.get("categoryImage") as File,
        parentCategoryID:
          form.get("ParentID") !== "" ? (form.get("ParentID") as string) : null,
      };
      console.log("add Reuqest : ", Newrequest);

      AddNewCategory(Newrequest);
    }
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
                //     ? new File([], "https://localhost:7164/CategoryImages/" +
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
