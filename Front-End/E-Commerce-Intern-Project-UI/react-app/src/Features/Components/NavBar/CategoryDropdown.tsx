import { Link } from "react-router-dom";
import { SubCategoryResponse } from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";

interface CategoryDropdownProps {
  categories: SubCategoryResponse[];
  isDropdownOpen: boolean;
  onToggleDropdown: () => void;
  onNavigate?: () => void;
}

export default function CategoryDropdown({
  categories,
  isDropdownOpen,
  onToggleDropdown,
  onNavigate,
}: CategoryDropdownProps) {
  return (
    <div className="dropdown">
      <span className="nav-link dropdown-trigger" onClick={onToggleDropdown}>
        Categories <span className="arrow">▾</span>
      </span>
      <div
        className={`Nav-dropdown-menu categories-menu ${isDropdownOpen ? "show" : ""}`}
      >
        <div className="mega-menu">
          {categories.length > 0 &&
            categories.map((category: SubCategoryResponse) => (
              <div className="menu-column" key={category.categoryID}>
                <div className="column-title">{category.categoryName}</div>
                {category.subCategories?.map((subCat) => (
                  <Link
                    className="nav-dropdown-link"
                    to={`/Home/Products?Category=${category.categoryName}&SubCategory=${subCat.categoryName}&Price=0&Size=All&Color=All`}
                    key={subCat.categoryID}
                    onClick={onNavigate}
                  >
                    <span className="nav-dropdown-item">
                      {subCat.categoryName}
                    </span>
                  </Link>
                ))}
              </div>
            ))}
        </div>

        <div className="menu-footer">
          {categories.map((cat: SubCategoryResponse) => (
            <Link
              className="nav-dropdown-link"
              to={`/Home/Products?Category=${cat.categoryName}&SubCategory=All&Price=0&Size=All&Color=All`}
              key={cat.categoryID}
              onClick={onNavigate}
            >
              <span className="footer-link">View All {cat.categoryName} →</span>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
}
