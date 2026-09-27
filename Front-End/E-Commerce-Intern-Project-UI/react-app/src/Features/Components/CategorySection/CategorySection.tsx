import { useEffect, useState } from "react";
import "./CategorySection.css";
import { Link } from "react-router-dom";
import {
  CategoryResponse,
  InternoMicroserviceClient,
  SubCategoryResponse,
} from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API } from "../../../Core/Shared/API-URL";
export default function CategorySection() {
  const Base_Url = "http://localhost:5000";
  const [Categories, SetCategories] = useState<SubCategoryResponse[]>([]);
  const Client = new InternoMicroserviceClient(undefined, API);
  useEffect(() => {
    const loadCats = async () => {
      try {
        const response = await Client.getCategoriesWithSubCat();

        if (response.statusCode! > 200) {
          console.log(response.errorMessage);
        }
        if (response.data) {
          SetCategories(response.data!);
        }
      } catch (err) {
        console.error(err);
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
          {Categories.map((cat) => (
            <div className="category-card" key={cat.categoryID}>
              <Link
                to={`/Home/Products?Category=${cat.categoryName}&SubCategory=All&Price=0&Size=All&Color=All`}
              >
                <img
                  src={`${Base_Url}/CategoryImages/${cat.categoryImageUrl}`}
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
