import { useEffect, useState } from "react";
import { Heart, Eye, Star } from "lucide-react";
import { ProductCardSkeleton } from "../Shared/SkeletonLoader";
import {
  AddCartItemListRequest,
  CartDetailsResponse,
  CreateWishListRequest,
  InternoMicroserviceClient,
  ProductDetails_vw,
  WishListDetailsResponse,
} from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config, getProductImageUrl } from "../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider";
import "./ProductSection.css";
import { CategoryfilterType } from "../../../Core/Types/CategoryFIlterTypes";
import { useFilterStore } from "../../../Core/Global-State-Management/Zusstand/FilterStore/FilterStore";
import { useCartStore } from "../../../Core/Global-State-Management/Zusstand/CartStore/CartUpdateStore";
type props = {
  filterProps: CategoryfilterType | null;
};

const PAGE_SIZE = 12;

export default function ProductsSection({ filterProps }: props) {
  const { user } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [Products, SetProducts] = useState<ProductDetails_vw[]>([]);
  const [_Products, SetPrivProducts] = useState<ProductDetails_vw[]>([]);
  const [CurrentFilter, SetCurrentFilter] = useState<string>();
  const [userWishListItems, setUserWishListItems] = useState<
    WishListDetailsResponse[]
  >([]);
  const { triggerCartUpdate } = useCartStore();
  const [CartItems, setCartItems] = useState<CartDetailsResponse>();
  const { filter } = useFilterStore();
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  const [loadError, setLoadError] = useState("");

  useEffect(() => {
    if (filterProps) {
      SetProducts(
        filterProps === "All"
          ? _Products
          : _Products
              .slice()
              .filter((p) => p.parentCategoryName === filterProps),
      );
      SetCurrentFilter(filterProps);
    }
  }, [filterProps]);

  const loadCartItems = async () => {
    try {
      const response = await Client.getUserCartItemsDetails();
      if (response.isSuccess) {
        setCartItems(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadFilter = async (page = pageNumber) => {
    setIsLoading(true);
    setLoadError("");
    try {
      const response = await Client.filterProducts(
        page,
        PAGE_SIZE,
        filter.Category,
        filter.SubCategory,
        filter.Price,
        filter.Size,
        filter.Color,
        filter.Sort,
      );
      if (response.isSuccess && response.data) {
        SetProducts(response.data.products ?? []);
        SetPrivProducts(response.data.products ?? []);
        setTotalPages(response.data.totalPages || 1);
        setHasNextPage(!!response.data.hasNextPage);
        setHasPreviousPage(!!response.data.hasPreviousPage);
        setPageNumber(response.data.currentPage || page);
      } else {
        SetProducts([]);
        setLoadError(response.errorMessage || "No Products Found");
      }
    } catch (err) {
      SetProducts([]);
      setLoadError(getApiErrorMessage(err));
    } finally {
      setIsLoading(false);
    }
  };

  const loadWishListItems = async () => {
    try {
      const response = await Client.getAllUserWishList(user?.userID!);
      if (response.isSuccess) {
        setUserWishListItems(response.data!);
      } else {
        setUserWishListItems([]);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  useEffect(() => {
    if (user) {
      loadWishListItems();
      loadCartItems();
    } else {
      setUserWishListItems([]);
      setCartItems(new CartDetailsResponse());
    }
  }, [user]);

  useEffect(() => {
    setPageNumber(1);
    loadFilter(1);
  }, [filter]);

  async function AddToWishList(productID: string) {
    try {
      const response = await Client.createWishList(
        new CreateWishListRequest({
          userID: user?.userID!,
          productID: productID,
        }),
      );
      if (response.isSuccess) {
        loadWishListItems();
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }
  function CalculateSalePerc(product: ProductDetails_vw) {
    return (
      ((product.salePrice ?? 0) / (product.price ?? 1)) * 100 -
      100 +
      100
    ).toFixed(2);
  }

  async function addItem(productID: string) {
    try {
      const response = await Client.addCartItemList(
        new AddCartItemListRequest({
          productIDs: [productID],
        }),
      );
      if (response.isSuccess) {
        loadCartItems();
        triggerCartUpdate();
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }

  const pageItems = Array.from({ length: totalPages }, (_, i) => i + 1);

  return (
    <>
      <div className="products-grid">
        {isLoading ? (
          // Show skeleton loaders while loading
          Array.from({ length: 8 }).map((_, index) => (
            <ProductCardSkeleton key={index} />
          ))
        ) : Products.length > 0 ? (
          Products.map((product) => (
            <div className="product-card" key={product.productID}>
              <div className="product-image-wrapper">
                <img
                  src={getProductImageUrl(
                    product.productID ?? product.productImageUrl,
                  )}
                  alt="Linen Wrap Dress"
                />
                <div className="product-badges">
                  {product.salePrice !== 0 && product.salePrice && (
                    <span className="Custom-badge Custom-sale">
                      {CalculateSalePerc(product)}% SALE
                    </span>
                  )}
                </div>
                <button
                  className={`wishlist-btn ${userWishListItems.length > 0 ? (userWishListItems.find((w) => w.product?.productID === product.productID) ? "active" : "") : ""}`}
                  onClick={() => AddToWishList(product.productID!)}
                >
                  <Heart size={20} />
                </button>
                <div className="quick-view-overlay">
                  <button className="quick-view-btn">
                    <Eye size={16} style={{ marginRight: "8px" }} />
                    Quick View
                  </button>
                </div>
              </div>
              <div className="product-info">
                <div className="product-category">
                  {product.sizeName?.slice(0, 1)} {product.parentCategoryName}{" "}
                  {product.categoryName}
                </div>
                <h3 className="product-name">{product.productName}</h3>
                <div className="product-rating">
                  {[...Array(Math.floor(product.rating ?? 0))].map((_, i) => (
                    <Star key={i} size={14} fill="#f39c12" color="#f39c12" />
                  ))}
                  {[...Array(Math.round(5 - (product.rating ?? 0)))].map(
                    (_, i) => (
                      <Star key={i} size={14} color="#f39c12" />
                    ),
                  )}
                  <span>({product.totalRating})</span>
                </div>
                <div className="product-price">
                  <span className="current-price">
                    $
                    {product.salePrice !== 0 && product.salePrice
                      ? product.salePrice
                      : product.price}
                  </span>
                  {product.salePrice !== 0 && product.salePrice && (
                    <span className="original-price">${product.price}</span>
                  )}
                </div>
                {CartItems?.cartItems?.find(
                  (r) => r.product?.productID === product.productID,
                ) === undefined && (
                  <button
                    className="add-to-cart-btn"
                    onClick={() => {
                      addItem(product.productID!);
                    }}
                  >
                    Add to Cart
                  </button>
                )}
              </div>
            </div>
          ))
        ) : (
          <div className="empty-state">
            <div className="empty-icon">
              <Eye size={48} opacity={0.5} />
            </div>
            <h3>{loadError || "No Products Found"}</h3>
          </div>
        )}
      </div>
      {totalPages > 1 && Products.length > 0 && (
        <div className="pagination">
          <button
            type="button"
            className="page-btn"
            disabled={!hasPreviousPage || pageNumber <= 1}
            onClick={() => {
              const next = Math.max(1, pageNumber - 1);
              setPageNumber(next);
              loadFilter(next);
            }}
          >
            Previous
          </button>
          <div className="page-numbers">
            {pageItems.map((page) => (
              <button
                type="button"
                key={page}
                className={`page-num ${page === pageNumber ? "active" : ""}`}
                onClick={() => {
                  setPageNumber(page);
                  loadFilter(page);
                }}
              >
                {page}
              </button>
            ))}
          </div>
          <button
            type="button"
            className="page-btn"
            disabled={!hasNextPage || pageNumber >= totalPages}
            onClick={() => {
              const next = pageNumber + 1;
              setPageNumber(next);
              loadFilter(next);
            }}
          >
            Next
          </button>
        </div>
      )}
    </>
  );
}
