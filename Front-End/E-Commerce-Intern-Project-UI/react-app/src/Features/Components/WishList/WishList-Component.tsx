import { useEffect, useState } from "react";
import "./WishList.css";
import { WishListDetailsModel } from "../../../Core/DTO/WishList/WishListDetailsModel";
import {
  ClearUserWishList,
  DeleteWishList,
  GetAllUserWishList,
} from "../../../Core/Services/WishList-Services/WishListService";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider";
import {
  AddCartItem,
  AddCartItemList,
} from "../../../Core/Services/Cart-Item-Services/CartItemService";
import { CartItemRequest } from "../../../Core/DTO/CartItemDTO/CartItemRequest";
import { CartDetails } from "../../../Core/DTO/CartDTO/CartDetails";
import { GetUserCartItemsDetails } from "../../../Core/Services/CartServices/CartService";
import { Link } from "react-router-dom";
import { useCartStore } from "../../../Core/Global-State-Management/Zusstand/CartStore/CartUpdateStore";

export default function WishListComponent() {
  const Base_Url = "https://localhost:7164";
  const { user } = useAuth();
  const [WishListItems, SetWishListItems] = useState<WishListDetailsModel[]>(
    [],
  );
  const [Cart, setCart] = useState<CartDetails>(new CartDetails());
  const { triggerCartUpdate } = useCartStore();
  const loadCartItems = async () => {
    try {
      const response = await GetUserCartItemsDetails(user?.userID!);
      if (response.isSuccess) {
        console.log(response.data);

        setCart(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  const loadWishListItems = async () => {
    try {
      const response = await GetAllUserWishList(user?.userID!);
      if (response.isSuccess) {
        console.log(response);

        SetWishListItems(response.data!);
      }
      if (response.statusCode === 404) {
        SetWishListItems([]);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  async function ClearWishList() {
    if (WishListItems.length > 0) {
      try {
        const response = await ClearUserWishList(user?.userID!);
        if (response.isSuccess) {
          loadWishListItems();
        }
      } catch (err) {
        if (err) console.error(err);
      }
    }
  }
  async function addItem(productID: string) {
    try {
      const response = await AddCartItem({
        userID: user?.userID!,
        productID: productID,
      });
      if (response.isSuccess) {
        SetWishListItems(
          WishListItems.filter((r) => r.product.productID !== productID),
        );
        triggerCartUpdate();
        RemoveFromWishList(
          WishListItems.find((r) => r.product.productID === productID)
            ?.wishlistID!,
        );
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  function loadWishListItemsWithFilter() {
    SetWishListItems(
      WishListItems.filter(
        (r) =>
          !Cart.cartItems.find(
            (ct) => ct.product.productID === r.product.productID,
          ),
      ),
    );
  }

  useEffect(() => {
    if (user) {
      loadWishListItems();
    }
    if (Cart.cartItems.length === 0) {
      loadCartItems();
    }
  }, []);

  useEffect(() => {
    if (Cart.cartItems.length > 0) {
      loadWishListItemsWithFilter();
    }
  }, [Cart.cartItems]);

  async function RemoveFromWishList(WishListID: string) {
    try {
      const respones = await DeleteWishList(WishListID);
      if (respones.isSuccess) {
        console.log(respones);

        loadWishListItems();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  async function addAllToCart() {
    if (WishListItems.length > 0) {
      const request: CartItemRequest[] = [];
      const userID = user?.userID;
      for (let item of WishListItems) {
        request.push({ userID: userID!, productID: item.product.productID });
      }

      try {
        const response = await AddCartItemList(request);
        if (response.isSuccess) {
          SetWishListItems([]);
          triggerCartUpdate();
          ClearWishList();
        }
      } catch (err) {
        if (err) console.error(err);
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
                onClick={() => RemoveFromWishList(item.wishlistID)}
                title="Remove from wishlist"
              >
                <span className="icon">❌</span>
              </button>
              <div className="wishlist-badge">
                <span
                  className={`stock-status ${item.product.stock > 0 ? "in" : "out-of"}-stock`}
                >
                  {item.product.stock > 0 ? "In" : "Out"} Stock
                </span>
              </div>
              <div className="wishlist-image">
                <img
                  src={`${Base_Url}/ProductsImages/${item.product.parentCategoryName}/${item.product.categoryName}/${item.product.productImageUrl}`}
                  alt="Linen Wrap Dress"
                />
              </div>
              <div className="wishlist-details">
                <h3 className="wishlist-name">{item.product.productName}</h3>
                <p className="wishlist-category">
                  {item.product.parentCategoryName}'s{" "}
                  {item.product.categoryName}
                </p>
                <div className="wishlist-price">
                  <span className="current-price">
                    $
                    {item.product.salePrice !== 0
                      ? item.product.salePrice
                      : item.product.price}
                  </span>
                  {item.product.salePrice !== 0 && (
                    <span className="original-price">
                      ${item.product.price}
                    </span>
                  )}
                </div>
                <div className="wishlist-actions">
                  <button
                    className="btn-add-to-cart"
                    onClick={() => addItem(item.product.productID)}
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
