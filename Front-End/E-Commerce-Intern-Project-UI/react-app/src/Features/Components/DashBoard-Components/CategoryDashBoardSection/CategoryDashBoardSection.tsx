import { useEffect, useState } from "react";
import { ISubCategoryDetails } from "../../../../Core/Interface/Category/ISubCateogryDetails";
import { DeleteCategory_serv } from "../../../../Core/Services/CategoryServices/CategoryServiceCmd";
import { GetSubCategoryDetails } from "../../../../Core/Services/CategoryServices/CategoryServiceQuery";
import { DeleteModel } from "../Shared/Model/DeleteModel";
import AddCategoryModel from "./Model/CategoryModel";

export default function CategoryDashBoardSection() {
  const [Categoires, SetCategories] = useState<ISubCategoryDetails[]>([]);
  const [updateCat, SetUpdateCategory] = useState<string | null>(null);
  const [ExpendedCat, SetExpendedCat] = useState("");
  const [isAddUpdateCat, SetisAddUpdateCat] = useState(false);
  const [isDeletingCat, SetDeletingCat] = useState(false);
  const [CurrentCatToDelete, SetCurrentCatToDelete] = useState("");

  const loadCats = async () => {
    try {
      const response = await GetSubCategoryDetails();
      SetCategories(response.data!);
    } catch (err) {
      if (err) console.error(err);
    }
  };

  useEffect(() => {
    loadCats();
  }, []);

  function DeleteCategory(CatID: string) {
    console.log("deleting cat");

    const DeleteCat = async () => {
      try {
        const response = await DeleteCategory_serv(CatID);
        if (response.data) {
          SetCategories(Categoires.filter((c) => c.categoryID != CatID));
        }
      } catch (err) {
        if (err) console.error(err);
      }
    };
    DeleteCat();
  }

  function OnAddOrUpdateCategoryEvent() {
    ToggleAddCategoryBtn(false);
    loadCats();
  }

  function OnDeleteCategoryEvent(value: boolean) {
    console.log(value);

    SetDeletingCat(false);
    if (value) {
      DeleteCategory(CurrentCatToDelete);
    }
  }
  function AddCat() {
    SetUpdateCategory(null);
    ToggleAddCategoryBtn(true);
  }
  function ToggleAddCategoryBtn(value: boolean) {
    SetisAddUpdateCat(value);
  }
  function UpdateCategory(catID: string) {
    SetUpdateCategory(catID);
    ToggleAddCategoryBtn(true);
  }
  function expendCategory(CatID: string) {
    console.log(CatID);
    if (CatID == ExpendedCat) {
      SetExpendedCat("");
      return;
    }
    SetExpendedCat(CatID);
  }
  return (
    <>
      <div className="section-header">
        <h3>Manage Categories</h3>
        <button
          className="btn-primary"
          id="addCategoryBtn"
          onClick={() => AddCat()}
        >
          + Add Category
        </button>
      </div>

      <div className="categories-container">
        <div className="categories-list" id="categoriesList">
          {Categoires.length > 0 &&
            Categoires.map((cat) => (
              <div
                className={
                  "category-item " +
                  (ExpendedCat === cat.categoryID ? "expanded" : "")
                }
                data-id={cat.categoryID}
                key={cat.categoryID}
              >
                <div className="category-header">
                  <div className="category-info">
                    <div className="category-name-dash">{cat.categoryName}</div>
                    <div className="category-count-dash">
                      {cat.subCategories.length} subcategories
                    </div>
                  </div>
                  <div className="category-actions">
                    <button
                      className="action-btn edit"
                      onClick={() => UpdateCategory(cat.categoryID)}
                    >
                      ✏️ Edit
                    </button>
                    <button
                      className="action-btn delete"
                      onClick={() => {
                        SetDeletingCat(true);
                        SetCurrentCatToDelete(cat.categoryID);
                      }}
                    >
                      🗑️ Delete
                    </button>
                    {cat.subCategories.length > 0 && (
                      <span
                        className="expand-icon"
                        onClick={() => expendCategory(cat.categoryID)}
                      >
                        ▼
                      </span>
                    )}
                  </div>
                </div>
                {cat.subCategories.length > 0 && (
                  <div
                    className={
                      "subcategories " +
                      (ExpendedCat === cat.categoryID ? "show" : "")
                    }
                  >
                    {cat.subCategories.map((sub) => (
                      <div className="subcategory-item" key={sub.categoryID}>
                        <span className="subcategory-name">
                          {sub.categoryName}
                        </span>
                        <div className="category-actions">
                          <button
                            className="action-btn edit"
                            onClick={() => UpdateCategory(sub.categoryID)}
                          >
                            ✏️
                          </button>
                          <button
                            className="action-btn delete"
                            onClick={() => {
                              SetDeletingCat(true);
                              SetCurrentCatToDelete(sub.categoryID);
                            }}
                          >
                            🗑️
                          </button>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            ))}
          {Categoires.length === 0 && (
            <div className="empty-state">
              <div className="empty-state-icon">📁</div>
              <h4>No Categories Yet</h4>
              <p>Start by adding your first category</p>
            </div>
          )}
        </div>
      </div>

      {isAddUpdateCat && (
        <AddCategoryModel
          onClose={() => SetisAddUpdateCat(false)}
          categoires={Categoires}
          categoryProps={updateCat!}
          onAddnewCategory={OnAddOrUpdateCategoryEvent}
        />
      )}
      {isDeletingCat && <DeleteModel onDeleteConfirm={OnDeleteCategoryEvent} />}
    </>
  );
}
