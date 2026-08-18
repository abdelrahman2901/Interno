import Header from "../../Components/Header/Header";
import CategorySection from "../../Components/CategorySection/CategorySection";
import ProductsSection from "../../Components/ProductSection/ProductsSection";
import FooterSection from "../../Components/FooterSection/Footer";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { CategoryfilterType } from "../../../Core/Types/CategoryFIlterTypes";
import { useFilterStore } from "../../../Core/Global-State-Management/Zusstand/FilterStore/FilterStore";

export default function Home() {
  const [CurrentFilter, SetCurrentFilter] =
    useState<CategoryfilterType | null>();
  const filter = (filterName: CategoryfilterType) => {
    SetCurrentFilter(filterName);
  };
  const { resetFilter } = useFilterStore();
  useEffect(() => {
    resetFilter();
  }, []);

  return (
    <>
      <Header />
      <CategorySection />
      <section className="main-products-section">
        <div className="main-products-header">
          <div>
            <div className="main-section-label">Hand-Picked</div>
            <h2 className="main-section-title">Featured Products</h2>
            <div className="main-title-underline"></div>
          </div>
          <Link
            to={
              "/Home/Products?Category=All&SubCategory=All&Price=0&Size=All&Color=All"
            }
            className="view-all"
          >
            VIEW ALL PRODUCTS →
          </Link>
        </div>

        <div className="filter-tabs">
          <button
            className={`filter-tab ${CurrentFilter === "All" ? "active" : " "}`}
            data-filter="all"
            onClick={() => filter("All")}
          >
            All
          </button>
          <button
            className={`filter-tab   ${CurrentFilter === "Women" ? "active" : " "}`}
            data-filter="women"
            onClick={() => filter("Women")}
          >
            Women
          </button>
          <button
            className={`filter-tab ${CurrentFilter === "Men" ? "active" : " "}`}
            data-filter="men"
            onClick={() => filter("Men")}
          >
            Men
          </button>
          <button
            className={`filter-tab  ${CurrentFilter === "Kids" ? "active" : " "}`}
            data-filter="kids"
            onClick={() => filter("Kids")}
          >
            Kids
          </button>
          <button
            className={`filter-tab ${
              CurrentFilter === "Accessories" ? "active" : " "
            }`}
            data-filter="accessories"
            onClick={() => filter("Accessories")}
          >
            Accessories
          </button>
        </div>
        <ProductsSection filterProps={CurrentFilter!} />
      </section>
      <FooterSection />
    </>
  );
}
