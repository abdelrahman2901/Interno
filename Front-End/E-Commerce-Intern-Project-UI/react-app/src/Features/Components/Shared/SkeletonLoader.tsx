import "./SkeletonLoader.css";

export const ProductCardSkeleton = () => (
  <div className="skeleton-product-card">
    <div className="skeleton skeleton-image"></div>
    <div className="skeleton-content">
      <div className="skeleton skeleton-title"></div>
      <div className="skeleton skeleton-text"></div>
      <div className="skeleton skeleton-price"></div>
      <div className="skeleton skeleton-button"></div>
    </div>
  </div>
);

export const CategorySkeleton = () => (
  <div className="skeleton-category">
    <div className="skeleton skeleton-category-image"></div>
    <div className="skeleton skeleton-category-text"></div>
  </div>
);

export const TextSkeleton = ({ width = "100%", height = "20px" }) => (
  <div 
    className="skeleton skeleton-text" 
    style={{ width, height }}
  ></div>
);

export const ButtonSkeleton = ({ width = "120px", height = "40px" }) => (
  <div 
    className="skeleton skeleton-button" 
    style={{ width, height }}
  ></div>
);

export const CartItemSkeleton = () => (
  <div className="skeleton-cart-item">
    <div className="skeleton skeleton-cart-image"></div>
    <div className="skeleton-cart-details">
      <div className="skeleton skeleton-cart-title"></div>
      <div className="skeleton skeleton-cart-text"></div>
      <div className="skeleton skeleton-cart-price"></div>
    </div>
    <div className="skeleton skeleton-cart-quantity"></div>
  </div>
);
