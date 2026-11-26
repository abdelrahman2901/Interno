import { useEffect, useState } from "react";
import { CategoryResponse, ColorResponse, InternoMicroserviceClient, SizeResponse, SubCategoryResponse } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../Core/Shared/API-URL";
import { FilterPageModel } from "../Model/FilterPageModel";
import { useFilterStore } from "../../../../Core/Global-State-Management/Zusstand/FilterStore/FilterStore";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";
export default function ProductsSideBarFilter() {
  const [colors, setColors] = useState<ColorResponse[]>([]);
  const [sizes, setSizes] = useState<SizeResponse[]>([]);
  const [_categories, setPrivCategories] = useState<SubCategoryResponse[]>([]);
  const [categories, setCategories] = useState<SubCategoryResponse[]>([]);
  const [updatedFIlter, setUpdatedFIlter] = useState<FilterPageModel>(
    new FilterPageModel(),
  );
  const [selectedSubCategories, setSelectedSubCategories] = useState<
    CategoryResponse[]
  >([]);
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const { setFilter, resetFilter, filter } = useFilterStore();

  const loadCategories = async () => {
    try {
      const response = await Client.getCategoriesWithSubCat();
      if (response.isSuccess && response.data) {
        setCategories(response.data);
        setPrivCategories(response.data);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadSizes = async () => {
    try {
      const response = await Client.getAllSizes();
      if (response.isSuccess && response.data) {
        setSizes(response.data);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadColors = async () => {
    try {
      const response = await Client.getAllColors();
      if (response.isSuccess && response.data) {
        setColors(response.data);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
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
    if (updatedFIlter?.CategoryID !== undefined && categories.length !== 0) {
      setSelectedSubCategories(
        categories.find((r) => r.categoryID === updatedFIlter?.CategoryID)
          ?.subCategories ?? [],
      );
    }
  }, [updatedFIlter?.SubCategoryID, categories.length]);

  // function extractSizeChar(sizeName: string) {
  //   if (sizeName.includes(" ")) {
  //     return sizeName.slice(0, 2).trim() + sizeName.split(" ")[1].slice(0, 1);
  //   }
  //   return sizeName.slice(0, 1);
  // }
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
                checked={updatedFIlter?.CategoryID === undefined}
              />
              <span>All Categories</span>
            </label>
            {categories.map((category) => (
              <label className="filter-checkbox" key={category.categoryID}>
                <input
                  type="radio"
                  value={category.categoryID}
                  onChange={(e) => {
                    const categoryID = e.currentTarget.value;

                    setUpdatedFIlter((prev) => ({
                      ...prev!,
                      CategoryID: categoryID,
                    }));

                    setSelectedSubCategories(category.subCategories ?? []);
                  }}
                  onClick={(e) => {
                    const isChecked = e.currentTarget.checked;
                    if (isChecked) {
                      setUpdatedFIlter((prev) => ({
                        ...prev!,
                        CategoryID: undefined,
                        SubCategory: "All",
                      }));
                      setSelectedSubCategories([]);
                    }
                  }}
                  checked={updatedFIlter?.CategoryID === category.categoryID}
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
                        const subCatID = e.currentTarget.value;

                        setUpdatedFIlter((prev) => ({
                          ...prev!,
                          SubCategoryID: subCatID,
                        }));
                      }}
                      onClick={(e) => {
                        const isChecked = e.currentTarget.checked;
                        if (isChecked) {
                          setUpdatedFIlter((prev) => ({
                            ...prev!,
                            SubCategoryID: undefined,
                          }));
                        }
                      }}
                      value={subCat.categoryID}
                      checked={
                        updatedFIlter?.SubCategoryID === subCat.categoryID
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
                className={`size-btn ${size.sizeName === updatedFIlter?.SizeID ? "active" : ""}`}
                onClick={(e) => {
                  const sizeID = e.currentTarget.value;

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    SizeID: sizeID !== updatedFIlter.SizeID ? sizeID : undefined,
                  }));
                }}
                value={size.sizeID}
                key={size.sizeID}
              >
                {(size.sizeName ?? "")}
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
                className={`color-btn ${color.colorID === updatedFIlter?.ColorID ? "active" : ""}`}
                onClick={(e) => {
                  const colorID = e.currentTarget.value;

                  setUpdatedFIlter((prev) => ({
                    ...prev!,
                    ColorID: colorID !== updatedFIlter.ColorID ? colorID : undefined,
                  }));
                }}
                value={color.colorID}
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
