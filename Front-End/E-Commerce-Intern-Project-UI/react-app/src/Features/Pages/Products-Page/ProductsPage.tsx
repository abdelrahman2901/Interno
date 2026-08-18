import { useSearchParams } from "react-router-dom";
import ProductsSideBarFilter from "../../Components/ProductPage-Components/ProductsSideBarFilter/ProductsSideBarFilter";
import ProductsSection from "../../Components/ProductSection/ProductsSection";
import "./Product-Page.css";
import { useEffect, useState } from "react";
import { FilterPageModel } from "../../Components/ProductPage-Components/Model/FilterPageModel";
import { useFilterStore } from "../../../Core/Global-State-Management/Zusstand/FilterStore/FilterStore";

export default function ProductsPage() {
  const [params] = useSearchParams();

  const { filter, setFilter } = useFilterStore();
  useEffect(() => {
    const paramFilter: FilterPageModel = {
      Category: params.get("Category")!,
      SubCategory: params.get("SubCategory")!,
      Price: Number(params.get("Price")),
      Size: params.get("Size")!,
      Color: params.get("Color")!,
      Sort: "",
    };
    setFilter(paramFilter);
  }, []);

  return (
    <>
      <div className="products-container">
        <ProductsSideBarFilter />
        <main className="products-main">
          <div className="products-toolbar">
            <div className="toolbar-actions">
              <select
                className="sort-select"
                onChange={(e) => {
                  const sort = e.currentTarget.value;
                  setFilter({ Sort: sort });
                }}
                value={filter.Sort}
              >
                <option value="All">Featured</option>
                <option value="new">Newest</option>
                <option value="priceL">Price: Low to High</option>
                <option value="priceH">Price: High to Low</option>
                <option value="nameA">Name: A-Z</option>
                <option value="nameZ">Name: Z-A</option>
              </select>
            </div>
          </div>
          <ProductsSection filterProps={null} />
        </main>
      </div>
    </>
  );
}
