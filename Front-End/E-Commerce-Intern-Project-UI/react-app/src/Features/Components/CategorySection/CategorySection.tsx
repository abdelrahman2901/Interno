import { useEffect, useState } from "react";
import "./CategorySection.css";
import { Link } from "react-router-dom";
import {
  InternoMicroserviceClient,
  SubCategoryResponse,
} from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";
import { getCategoryImage } from "../../../Core/Shared/getCategoryImage";

export default function CategorySection() {
  const [Categories, SetCategories] = useState<SubCategoryResponse[]>([]);
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  useEffect(() => {
    const loadCats = async () => {
      try {
        const response = await Client.getCategoriesWithSubCat();

        if (response.statusCode! > 200) {
          console.log(getApiErrorMessage(response));
        }
        if (response.data) {
          SetCategories(response.data!);
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    };
    loadCats();
  }, []);

  return (
    <>
      <section className="categories-section">
        <div className="main-section-header">
          <div className="main-section-label">Browse By</div>
          <h2 className="main-section-title">Shop By Category</h2>
          <div className="main-title-underline"></div>
        </div>
        <div className="categories-grid">
          {Categories.map((cat, index) => (
            <div className="category-card" key={cat.categoryID}>
              <Link
                to={`/Home/Products?Category=${cat.categoryName}&SubCategory=All&Price=0&Size=All&Color=All`}
              >
                <img
                  src={getCategoryImage(cat, index)}
                  alt={cat.categoryName}
                />
              </Link>
              <div className="category-overlay">
                <div className="category-name">{cat.categoryName}</div>
                <div className="category-count">
                  {cat.subCategories?.length! > 0
                    ? `${cat.subCategories?.length!}+ styles`
                    : "No Styles Was Added Recently"}
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>
    </>
  );
}
