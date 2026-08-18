import { useEffect, useState } from "react";
import { ColorDetails } from "../../../../Core/DTO/Colors/ColorDetails";
import { ISize } from "../../../../Core/Interface/Size/ISize";
import { ISubCategoryDetails } from "../../../../Core/Interface/Category/ISubCateogryDetails";
import { GetSizes_Serv } from "../../../../Core/Services/SizeServices/SizeService_Query";
import { GetColors_Serv } from "../../../../Core/Services/ColorsServices/ColorService_Query";
import { GetSubCategoryDetails } from "../../../../Core/Services/CategoryServices/CategoryServiceQuery";
import { FilterPageModel } from "../Model/FilterPageModel";
import { useFilterStore } from "../../../../Core/Global-State-Management/Zusstand/FilterStore/FilterStore";
import { ICategory } from "../../../../Core/Interface/Category/ICategory";

export default function ProductsSideBarFilter() {
  const [colors, setColors] = useState<ColorDetails[]>([]);
  const [sizes, setSizes] = useState<ISize[]>([]);
  const [_categories, setPrivCategories] = useState<ISubCategoryDetails[]>([]);
  const [categories, setCategories] = useState<ISubCategoryDetails[]>([]);
  const [updatedFIlter, setUpdatedFIlter] = useState<FilterPageModel>(
    new FilterPageModel(),
  );
  const [selectedSubCategories, setSelectedSubCategories] = useState<
    ICategory[]
  >([]);
  const { setFilter, resetFilter, filter } = useFilterStore();

  const loadCategories = async () => {
    try {
      const response = await GetSubCategoryDetails();
      if (response.isSuccess && response.data) {
        setCategories(response.data);
        setPrivCategories(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadSizes = async () => {
    try {
      const response = await GetSizes_Serv();
      if (response.isSuccess && response.data) {
        setSizes(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadColors = async () => {
    try {
      const response = await GetColors_Serv();
      if (response.isSuccess && response.data) {
        setColors(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (colors.length === 0) {
      loadColors();
    }
    if (sizes.length === 0) {
      loadSizes();
    }
    if (categories.length === 0) {
      loadCategories();
    }
  }, []);
  useEffect(() => {
    console.log(filter);

    setUpdatedFIlter(filter);
  }, [filter]);
  useEffect(() => {
    if (updatedFIlter?.Category !== "All" && categories.length !== 0) {
      setSelectedSubCategories(
        categories.find((r) => r.categoryName === updatedFIlter?.Category)
          ?.subCategories!,
      );
    }
  }, [updatedFIlter?.SubCategory, categories.length]);

  function extractSizeChar(sizeName: string) {
    if (sizeName.includes(" ")) {
      return sizeName.slice(0, 2).trim() + sizeName.split(" ")[1].slice(0, 1);
    }
    return sizeName.slice(0, 1);
  }
  return (
    <>
      <aside className="filters-sidebar">
        <div className="filters-header">
          <h3>Filters</h3>
          <button
            className="apply-filters "
            onClick={() => {
              console.log(updatedFIlter);

              setFilter(updatedFIlter!);
            }}
          >
            Apply
          </button>
          <button
            className="clear-filters"
            onClick={() => {
              setUpdatedFIlter(new FilterPageModel());
              resetFilter();
            }}
          >
            Clear All
          </button>
        </div>

        <div className="filter-section">
          <h4 className="filter-title">Category</h4>
          <div className="filter-options">
            <label className="filter-checkbox">
              <input
                type="radio"
                value="All"
                onChange={(e) => {
                  const category = e.currentTarget.value;

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    Category: category,
                  }));
                  setSelectedSubCategories([]);
                }}
                checked={updatedFIlter?.Category === "All"}
              />
              <span>All Categories</span>
            </label>
            {categories.map((category) => (
              <label className="filter-checkbox" key={category.categoryID}>
                <input
                  type="radio"
                  value={category.categoryName}
                  onChange={(e) => {
                    const categoryName = e.currentTarget.value;

                    setUpdatedFIlter((prev) => ({
                      ...prev!,
                      Category: categoryName,
                    }));

                    setSelectedSubCategories(category.subCategories);
                  }}
                  onClick={(e) => {
                    const isChecked = e.currentTarget.checked;
                    if (isChecked) {
                      setUpdatedFIlter((prev) => ({
                        ...prev!,
                        Category: "All",
                        SubCategory: "All",
                      }));
                      setSelectedSubCategories([]);
                    }
                  }}
                  checked={updatedFIlter?.Category === category.categoryName}
                />
                <span>{category.categoryName}</span>
              </label>
            ))}
          </div>
        </div>

        <div className="filter-section">
          <h4 className="filter-title">Subcategory</h4>
          <div className="filter-options">
            {(selectedSubCategories.length > 0 || selectedSubCategories) && (
              <div>
                {selectedSubCategories.map((subCat) => (
                  <label className="filter-checkbox" key={subCat.categoryID}>
                    <input
                      type="radio"
                      onChange={(e) => {
                        const subCat = e.currentTarget.value;

                        setUpdatedFIlter((prev) => ({
                          ...prev!,
                          SubCategory: subCat,
                        }));
                      }}
                      onClick={(e) => {
                        const isChecked = e.currentTarget.checked;
                        if (isChecked) {
                          setUpdatedFIlter((prev) => ({
                            ...prev!,
                            SubCategory: "All",
                          }));
                        }
                      }}
                      value={subCat.categoryName}
                      checked={
                        updatedFIlter?.SubCategory === subCat.categoryName
                      }
                    />
                    <span>{subCat.categoryName}</span>
                  </label>
                ))}
              </div>
            )}
          </div>
        </div>

        <div className="filter-section">
          <h4 className="filter-title">Price Range</h4>
          <div className="filter-options">
            <label className="filter-checkbox">
              <input
                type="checkbox"
                name="price-0"
                onChange={(e) => {
                  const price = e.currentTarget.value;
                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    Price: price === "150+" ? -1 : Number(price),
                  }));
                }}
                value="50"
                onClick={(e) => {
                  const isChecked = e.currentTarget.checked;
                  if (isChecked) {
                    setUpdatedFIlter((prev) => ({
                      ...prev!,
                      Price: 0,
                    }));
                  }
                }}
                checked={updatedFIlter?.Price === 50}
              />
              <span>Under $50</span>
            </label>
            <label className="filter-checkbox">
              <input
                type="checkbox"
                name="price-1"
                onChange={(e) => {
                  const price = e.currentTarget.value;

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    Price: price === "150+" ? -1 : Number(price),
                  }));
                }}
                onClick={(e) => {
                  const isChecked = e.currentTarget.checked;
                  if (isChecked) {
                    setUpdatedFIlter((prev) => ({
                      ...prev!,
                      Price: 0,
                    }));
                  }
                }}
                value="100"
                checked={updatedFIlter?.Price === 100}
              />
              <span> {"<"} $100</span>
            </label>
            <label className="filter-checkbox">
              <input
                type="checkbox"
                name="price-2"
                onChange={(e) => {
                  const price = e.currentTarget.value;
                  console.log("ffwefew");

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    Price: price === "150+" ? -1 : Number(price),
                  }));
                }}
                onClick={(e) => {
                  const isChecked = e.currentTarget.checked;
                  if (isChecked) {
                    console.log("updadadd");
                    setUpdatedFIlter((prev) => ({
                      ...prev!,
                      Price: 0,
                    }));
                  }
                }}
                value="150"
                checked={updatedFIlter?.Price === 150}
              />
              <span>{"<"} $150</span>
            </label>
            <label className="filter-checkbox">
              <input
                type="checkbox"
                name="price-2"
                onChange={(e) => {
                  const price = e.currentTarget.value;
                  console.log(price === "150+" ? -1 : Number(price));

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    Price: price === "150+" ? -1 : Number(price),
                  }));
                }}
                onClick={(e) => {
                  const isChecked = e.currentTarget.checked;
                  if (isChecked) {
                    setUpdatedFIlter((prev) => ({
                      ...prev!,
                      Price: 0,
                    }));
                    console.log(0);
                  }
                }}
                value="150+"
                checked={updatedFIlter?.Price === -1}
              />
              <span>$150+</span>
            </label>
          </div>
        </div>

        <div className="filter-section">
          <h4 className="filter-title">Size</h4>
          <div className="size-options">
            {sizes.map((size) => (
              <button
                className={`size-btn ${size.sizeName === updatedFIlter?.Size ? "active" : ""}`}
                onClick={(e) => {
                  const size = e.currentTarget.value;

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    Size: size !== updatedFIlter.Size ? size : "All",
                  }));
                }}
                value={size.sizeName}
                key={size.sizeID}
              >
                {extractSizeChar(size.sizeName)}
              </button>
            ))}
          </div>
        </div>

        <div className="filter-section">
          <h4 className="filter-title">Color</h4>
          <div className="color-options">
            {colors.map((color) => (
              <button
                key={color.colorID}
                className={`color-btn ${color.colorName === updatedFIlter?.Color ? "active" : ""}`}
                onClick={(e) => {
                  const color = e.currentTarget.value;

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    Color: color !== updatedFIlter.Color ? color : "All",
                  }));
                }}
                value={color.colorName}
                style={{ background: color.colorHexCode }}
                title={color.colorName}
              ></button>
            ))}
          </div>
        </div>
      </aside>
    </>
  );
}
