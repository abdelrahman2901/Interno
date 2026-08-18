import { Link } from 'react-router-dom';
import { ISubCategoryDetails } from '../../../Core/Interface/Category/ISubCateogryDetails';

interface CategoryDropdownProps {
  categories: ISubCategoryDetails[];
  isDropdownOpen: boolean;
  onToggleDropdown: () => void;
}

export default function CategoryDropdown({ categories, isDropdownOpen, onToggleDropdown }: CategoryDropdownProps) {
  return (
    <div className="dropdown">
      <span
        className="nav-link dropdown-trigger"
        onClick={onToggleDropdown}
      >
        Categories <span className="arrow">▾</span>
      </span>
      <div className={`dropdown-menu categories-menu ${isDropdownOpen ? "show" : ""}`}>
        <div className="mega-menu">
          {categories.length > 0 &&
            categories.map((category: ISubCategoryDetails) => (
              <div className="menu-column" key={category.categoryID}>
                <div className="column-title">
                  {category.categoryName}
                </div>
                {category.subCategories.map((subCat: any) => (
                  <Link
                    className="nav-link"
                    to={`/Home/Products?Category=${category.categoryName}&SubCategory=${subCat.categoryName}&Price=0&Size=All&Color=All`}
                    key={subCat.categoryID}
                  >
                    <div className="dropdown-item">
                      {subCat.categoryName}
                    </div>
                  </Link>
                ))}
              </div>
            ))}
        </div>

        <div className="menu-footer">
          {categories.map((cat: ISubCategoryDetails) => (
            <Link
              className="nav-link"
              to={`/Home/Products?Category=${cat.categoryName}&SubCategory=All&Price=0&Size=All&Color=All`}
              key={cat.categoryID}
            >
              <span className="footer-link">
                View All {cat.categoryName} →
              </span>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
}
