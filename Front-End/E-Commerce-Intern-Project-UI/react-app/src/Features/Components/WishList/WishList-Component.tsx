import { useEffect, useState } from "react";
import "./WishList.css";
import {
  AddCartItemListRequest,
  CartDetailsResponse,
  InternoMicroserviceClient,
  WishListDetailsResponse,
} from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config, getProductImageUrl } from "../../../Core/Shared/API-URL";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider";
import { Link } from "react-router-dom";
import { useCartStore } from "../../../Core/Global-State-Management/Zusstand/CartStore/CartUpdateStore";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";
export default function WishListComponent() {
  const { user } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [WishListItems, SetWishListItems] = useState<WishListDetailsResponse[]>(
    [],
  );
  const [Cart, setCart] = useState<CartDetailsResponse>(
    new CartDetailsResponse(),
  );
  const { triggerCartUpdate } = useCartStore();
  const loadCartItems = async () => {
    try {
      const response = await Client.getUserCartItemsDetails();
      if (response.isSuccess) {
        console.log(response.data);

        setCart(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  const loadWishListItems = async () => {
    try {
      const response = await Client.getAllUserWishList(user?.userID!);
      if (response.isSuccess) {
        console.log(response);

        SetWishListItems(response.data!);
      }
      if (response.statusCode === 404) {
        SetWishListItems([]);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  async function ClearWishList() {
    if (WishListItems.length > 0) {
      try {
        const response = await Client.clearUserWishList(user?.userID!);
        if (response.isSuccess) {
          loadWishListItems();
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    }
  }
  async function addItem(productID: string) {
    try {
      const response = await Client.addCartItemList(
        new AddCartItemListRequest({
          productIDs: [productID],
        }),
      );
      if (response.isSuccess) {
        SetWishListItems(
          WishListItems.filter((r) => r.product?.productID !== productID),
        );
        triggerCartUpdate();
        RemoveFromWishList(
          WishListItems.find((r) => r.product?.productID === productID)
            ?.wishlistID!,
        );
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }
  function loadWishListItemsWithFilter() {
    SetWishListItems(
      WishListItems.filter(
        (r) =>
          !Cart.cartItems?.find(
            (ct) => ct.product?.productID === r.product?.productID,
          ),
      ),
    );
  }

  useEffect(() => {
    if (user) {
      loadWishListItems();
    }
    if ((Cart.cartItems?.length ?? 0) === 0) {
      loadCartItems();
    }
  }, []);

  useEffect(() => {
    if ((Cart.cartItems?.length ?? 0) > 0) {
      loadWishListItemsWithFilter();
    }
  }, [Cart.cartItems]);

  async function RemoveFromWishList(WishListID: string) {
    try {
      const respones = await Client.deleteWishList(WishListID);
      if (respones.isSuccess) {
        console.log(respones);

        loadWishListItems();
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }
  async function addAllToCart() {
    if (WishListItems.length > 0) {
      try {
        const response = await Client.addCartItemList(
          new AddCartItemListRequest({
            productIDs: WishListItems.map(
              (item) => item.product?.productID!,
            ).filter(Boolean),
          }),
        );
        if (response.isSuccess) {
          SetWishListItems([]);
          triggerCartUpdate();
          ClearWishList();
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    }
  }
  return (
    <>
      <div className="wishlist-header">
        <h1>My Wishlist</h1>
        <span className="item-count" id="wishlistCount">
          {WishListItems.length} Items
        </span>
      </div>
      {WishListItems.length === 0 && (
        <div className="empty-cart" id="emptyWishlist">
          <div className="empty-icon">♡</div>
          <h2>Your wishlist is empty</h2>
          <p>Save your favorite items here!</p>
          <a className="btn-primary">
            <Link
              className="nav-link"
              to={
                "/Home/Products?Category=All&SubCategory=All&Price=0&Size=All&Color=All"
              }
            >
              Start Shopping
            </Link>
          </a>
        </div>
      )}
      {WishListItems.length !== 0 && (
        <div className="wishlist-grid" id="wishlistGrid">
          {WishListItems.map((item) => (
            <div className="wishlist-item" key={item.wishlistID}>
              <button
                className="btn-remove-wishlist"
                onClick={() => RemoveFromWishList(item.wishlistID!)}
                title="Remove from wishlist"
              >
                <span className="icon">❌</span>
              </button>
              <div className="wishlist-badge">
                <span
                  className={`stock-status ${(item.product?.stock ?? 0) > 0 ? "in" : "out-of"}-stock`}
                >
                  {(item.product?.stock ?? 0) > 0 ? "In" : "Out"} Stock
                </span>
              </div>
              <div className="wishlist-image">
                <img
                  src={getProductImageUrl(
                    item.product?.productID ?? item.product?.productImageUrl,
                  )}
                  alt="Linen Wrap Dress"
                />
              </div>
              <div className="wishlist-details">
                <h3 className="wishlist-name">{item.product?.productName}</h3>
                <p className="wishlist-category">
                  {item.product?.parentCategoryName}'s{" "}
                  {item.product?.categoryName}
                </p>
                <div className="wishlist-price">
                  <span className="current-price">
                    $
                    {item.product?.salePrice !== 0
                      ? item.product?.salePrice
                      : item.product?.price}
                  </span>
                  {item.product?.salePrice !== 0 && (
                    <span className="original-price">
                      ${item.product?.price}
                    </span>
                  )}
                </div>
                <div className="wishlist-actions">
                  <button
                    className="btn-add-to-cart"
                    onClick={() => addItem(item.product?.productID!)}
                  >
                    Add to Cart
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      <div className="wishlist-bottom-actions">
        <button className="btn-clear-wishlist" onClick={ClearWishList}>
          Clear Wishlist
        </button>
        <button
          className="btn-add-all-cart"
          onClick={addAllToCart}
          // onclick="addAllToCart()"
        >
          Add All to Cart
        </button>
      </div>
    </>
  );
}
