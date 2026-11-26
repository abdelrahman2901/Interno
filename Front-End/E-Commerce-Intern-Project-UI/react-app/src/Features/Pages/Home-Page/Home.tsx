import Header from "../../Components/Header/Header";
import CategorySection from "../../Components/CategorySection/CategorySection";
import ProductsSection from "../../Components/ProductSection/ProductsSection";
import FooterSection from "../../Components/FooterSection/Footer";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { CategoryfilterType } from "../../../Core/Types/CategoryFIlterTypes";
import { useFilterStore } from "../../../Core/Global-State-Management/Zusstand/FilterStore/FilterStore";
import {
  CategoryResponse,
  InternoMicroserviceClient,
} from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { config } from "../../../Core/config";
import { API } from "../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";

export default function Home() {
  const [CurrentFilter, SetCurrentFilter] = useState<string | undefined>();
  const filter = (filterName: string | undefined) => {
    SetCurrentFilter(filterName);
  };
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [categories, setCategories] = useState<CategoryResponse[]>([]);
  const { setFilter } = useFilterStore();
  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      const response = await Client.getCategoriesWithSubCat();
      if (response.isSuccess) {
        setCategories(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

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
            onClick={() => filter(undefined)}
          >
            All
          </button>
          {categories.map((category) => (
            <button
              key={category.categoryID}
              className={`filter-tab ${CurrentFilter === category.categoryName ? "active" : " "}`}
              data-filter={category.categoryName}
              onClick={() => {
                filter(category.categoryName!);
                setFilter({ CategoryID: category.categoryID });
              }}
            >
              {category.categoryName}
            </button>
          ))}
        </div>
        <ProductsSection />
      </section>
      <FooterSection />
    </>
  );
}
