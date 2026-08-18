import { useEffect, useState } from "react";
import "./Cart.css";
import { CartDetails } from "../../../Core/DTO/CartDTO/CartDetails";
import { Link } from "react-router-dom";
import { OrderCouponModel } from "../../../Core/DTO/OrdeCouponDTO/OrderCouponResponse.js";
import { useCartStore } from "../../../Core/Global-State-Management/Zusstand/CartStore/CartUpdateStore.js";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider.js";
import {
  DeleteCartItem,
  UpdateCartItem,
} from "../../../Core/Services/Cart-Item-Services/CartItemService.js";
import { GetUserCartItemsDetails } from "../../../Core/Services/CartServices/CartService.js";
import { GetCouponByCode } from "../../../Core/Services/OrderCouponServices/OrderCouponService.js";

type props = {
  isnewProductAdded: boolean;
  isloadingProducts: () => void;
  finishAddingNewProduct: () => void;
};

export default function CartComponent({
  isnewProductAdded,
  isloadingProducts,
  finishAddingNewProduct,
}: props) {
  const Base_Url = "https://localhost:7164";
  const [Cart, setCart] = useState<CartDetails>(new CartDetails());
  const [coupon, setCoupon] = useState<OrderCouponModel>(
    new OrderCouponModel(),
  );
  const { user } = useAuth();
  const { triggerCartUpdate } = useCartStore();
  const [appliedCoupon, setAppliedCoupon] = useState<string>("");
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

  useEffect(() => {
    if (user && Cart.cartItems.length === 0) {
      loadCartItems();
    }
  }, [user]);

  useEffect(() => {
    if (isnewProductAdded) {
      loadCartItems();
      isnewProductAdded = false;
      finishAddingNewProduct();
    }
  }, [isnewProductAdded]);

  async function DeleteItem(itemID: string) {
    try {
      const response = await DeleteCartItem(itemID);
      if (response.isSuccess) {
        loadCartItems();
        isloadingProducts();
        triggerCartUpdate();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  async function updateItemQuantity(ItemID: string, increase: boolean) {
    try {
      const response = await UpdateCartItem({
        cartItemID: ItemID,
        isIncrease: increase,
      });
      if (response.isSuccess) {
        loadCartItems();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }

  async function applyPromoCode() {
    console.log(appliedCoupon);

    try {
      const response = await GetCouponByCode(appliedCoupon);
      if (response.data && response.isSuccess) {
        setCoupon(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  return (
    <>
      <div className="cart-content">
        <div className="cart-header">
          <h1>Shopping Cart</h1>
          <span className="item-count" id="itemCount">
            {Cart.cartItems.length} Items
          </span>
        </div>
        {Cart.cartItems.length === 0 && (
          <div className="empty-cart" id="emptyCart">
            <div className="empty-icon">🛒</div>
            <h2>Your cart is empty</h2>
            <p>Add some items to get started!</p>
            <Link
              to={`/Home/Products?Category=All&SubCategory=All&Price=0&Size=All&Color=All`}
              className="btn-primary"
            >
              Start Shopping
            </Link>
          </div>
        )}
        {Cart.cartItems.length > 0 && (
          <div className="cart-items">
            {Cart.cartItems.map((item) => (
              <div className="cart-item" key={item.cartItemID}>
                <div className="item-image">
                  <img
                    src={`${Base_Url}/ProductsImages/${item.product.parentCategoryName}/${item.product.categoryName}/${item.product.productImageUrl}`}
                    alt="Linen Wrap Dress"
                  />
                </div>
                <div className="item-details">
                  <h3 className="item-name">{item.product.productName}</h3>
                  <p className="item-category">
                    {item.product.parentCategoryName}'s{" "}
                    {item.product.categoryName}
                  </p>
                  <div className="item-specs">
                    <span>
                      Size: <strong>{item.product.sizeName.slice(0, 1)}</strong>
                    </span>
                    <span>
                      Color: <strong>{item.product.colorName}</strong>
                    </span>
                  </div>
                  <button
                    className="btn-remove"
                    onClick={() => DeleteItem(item.cartItemID)}
                  >
                    <span className="icon">🗑️</span> Remove
                  </button>
                </div>
                <div className="item-quantity">
                  <label>Quantity</label>
                  <div className="quantity-control">
                    <button
                      className="qty-btn"
                      onClick={() => updateItemQuantity(item.cartItemID, false)}
                    >
                      −
                    </button>
                    <span className="qty-input"> {item.quantity}</span>
                    <button
                      className="qty-btn"
                      onClick={() => updateItemQuantity(item.cartItemID, true)}
                    >
                      +
                    </button>
                  </div>
                </div>
                <div className="item-price">
                  <span className="price-label">Price</span>
                  <span className="price-value">
                    $
                    {item.product.salePrice !== 0 && item.product.salePrice
                      ? item.product.salePrice
                      : item.product.price}
                    {item.product.salePrice !== 0 && item.product.salePrice && (
                      <span className=" sale">${item.product.price}</span>
                    )}
                  </span>
                  <span className="price-total">
                    Total: $
                    {(item.product.salePrice !== 0 && item.product.salePrice
                      ? item.product.salePrice
                      : item.product.price) * item.quantity}
                  </span>
                </div>
              </div>
            ))}
          </div>
        )}

        <div className="promo-section">
          <h3>Have a promo code?</h3>
          <div className="promo-input-group">
            <input
              type="text"
              placeholder="Enter code"
              className="promo-input"
              onChange={(e) => {
                const coupon = e.currentTarget.value;
                setAppliedCoupon(coupon);
              }}
            />
            <button
              className="btn-apply"
              type="button"
              onClick={applyPromoCode}
            >
              Apply
            </button>
          </div>
          {coupon?.orderCouponID && (
            <div className="promo-message">Coupon Applied Sucessfully</div>
          )}
        </div>
      </div>

      <aside className="order-summary">
        <h2>Order Summary</h2>

        <div className="summary-row">
          <span>Subtotal</span>
          <span>
            $
            {Cart.cartItems.reduce(
              (total, price) => total + price.product.price * price.quantity,
              0,
            )}
          </span>
        </div>

        <div className="summary-row discount-row" id="discountRow">
          <span>Discount</span>
          <span className="discount-amount">
            -$
            {Cart.cartItems
              .filter((r) => r.product.salePrice !== 0 && r.product.salePrice)
              .reduce(
                (total, item) =>
                  total +
                  (item.product.price - item.product.salePrice) * item.quantity,
                0,
              )}
          </span>
        </div>
        {coupon?.orderCouponID && (
          <div className="summary-row discount-row">
            <span>Coupon</span>
            <span className="discount-amount">
              -$
              {coupon.discount}
            </span>
          </div>
        )}

        <div className="summary-divider"></div>

        <div className="summary-row total-row">
          <span>Total</span>
          <span>
            $
            {Cart.cartItems.reduce(
              (total, price) =>
                total +
                (price.product.salePrice !== 0 && price.product.salePrice
                  ? price.product.salePrice
                  : price.product.price) *
                  price.quantity,
              0,
            ) -
              coupon?.discount! <
            0
              ? 0
              : Cart.cartItems.reduce(
                  (total, price) =>
                    total +
                    (price.product.salePrice !== 0 && price.product.salePrice
                      ? price.product.salePrice
                      : price.product.price) *
                      price.quantity,
                  0,
                ) - coupon?.discount!}
          </span>
        </div>

        <button className="btn-checkout">
          <Link
            to={`/Home/Cart/OrderPayment?CouponCode=${appliedCoupon}`}
            className="nav-link"
          >
            Proceed to Checkout
          </Link>
        </button>

        <div className="payment-methods">
          <span>We accept:</span>
          <div className="payment-icons">
            <span>💳</span>
            <span>🏦</span>
            <span>📱</span>
          </div>
        </div>

        <div className="security-badge">
          <span className="badge-icon">🔒</span>
          <span>Secure Checkout</span>
        </div>
      </aside>
    </>
  );
}
