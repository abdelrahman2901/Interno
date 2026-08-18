import { useEffect, useState } from "react";
import { CartDetails } from "../../../../Core/DTO/CartDTO/CartDetails";
import { GetUserCartItemsDetails } from "../../../../Core/Services/CartServices/CartService";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import { AddressDetails } from "../../../../Core/DTO/AddressDTO/AddressDetails";
import { GetShippingCostDetailsByAreaID } from "../../../../Core/Services/ShippingCostServices/ShippingCostServices";
import { OrderRequest } from "../../../../Core/DTO/OrderDTO/OrderRequest";
import { ShippingCostDetails } from "../../../../Core/DTO/ShippingCostDTO/ShippingCostDetails";
import { OrderCouponModel } from "../../../../Core/DTO/OrdeCouponDTO/OrderCouponResponse";

type props = {
  address: AddressDetails;
  onPassRequest: (request: OrderRequest) => void;
  couponProps: OrderCouponModel;
};
export default function OrderSideBar({
  address,
  onPassRequest,
  couponProps,
}: props) {
  const { user } = useAuth();
  const [Cart, setCart] = useState<CartDetails | null>(null);
  const [subtotal, setSubtotal] = useState<number>(0);
  const [shipping, setShipping] = useState<ShippingCostDetails>(
    new ShippingCostDetails(),
  );
  const [coupon, setCoupon] = useState<OrderCouponModel>(
    new OrderCouponModel(),
  );

  const [discount, setDiscount] = useState<number>(0);
  const [total, setTotal] = useState<number>(0);
  const [selectedAddress, setSelectedAddresss] = useState<AddressDetails>(
    new AddressDetails(),
  );
  const [orderRequest, setOrderRequest] = useState<OrderRequest>(
    new OrderRequest(),
  );
  useEffect(() => {
    if (couponProps) {
      setOrderRequest((prev) => ({
        ...prev,
        orderCouponID: couponProps.orderCouponID,
      }));
      setCoupon(couponProps);
    }
  }, [couponProps]);
  const loadCartItems = async () => {
    try {
      const response = await GetUserCartItemsDetails(user?.userID!);
      if (response.isSuccess) {
        setCart(response.data!);
        calculateOrderSummary();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadAddressShippingCost = async () => {
    try {
      const response = await GetShippingCostDetailsByAreaID(address.areaID);
      if (response.isSuccess && response.data) {
        setOrderRequest((prev) => ({
          ...prev,
          shippingCostID: response.data?.shippingCostID!,
          totalAmount:
            Cart?.cartItems.reduce(
              (total, price) =>
                total +
                (price.product.salePrice !== 0
                  ? price.product.salePrice
                  : price.product.price) *
                  price.quantity,
              0,
            )! + response.data?.shippingCost!,
        }));
        setShipping(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  useEffect(() => {
    if (!Cart && user) {
      loadCartItems();
    }
  }, [user]);

  useEffect(() => {
    if (address) {
      setSelectedAddresss(address);
      loadAddressShippingCost();
    }
  }, [address]);

  function calculateOrderSummary() {
    setSubtotal(
      Cart?.cartItems.reduce(
        (total, price) => total + price.product.price * price.quantity,
        0,
      )!,
    );

    setDiscount(
      Cart?.cartItems
        .filter((r) => r.product.salePrice !== 0 && r.product.salePrice)
        .reduce(
          (total, item) =>
            total +
            (item.product.price - item.product.salePrice) * item.quantity,
          0,
        )!,
    );

    const total =
      Cart?.cartItems.reduce(
        (total, price) =>
          total +
          (price.product.salePrice !== 0 && price.product.salePrice
            ? price.product.salePrice
            : price.product.price) *
            price.quantity,
        0,
      )! +
        shipping.shippingCost -
        coupon.discount <
      0
        ? 0
        : Cart?.cartItems.reduce(
            (total, price) =>
              total +
              (price.product.salePrice !== 0 && price.product.salePrice
                ? price.product.salePrice
                : price.product.price) *
                price.quantity,
            0,
          )! +
          shipping.shippingCost -
          coupon.discount;

    setTotal(total);
  }

  useEffect(() => {
    if (Cart && shipping) {
      calculateOrderSummary();
    }
  }, [shipping, Cart]);

  useEffect(() => {
    if (!orderRequest.userID) {
      setOrderRequest((prev) => ({ ...prev, userID: user?.userID! }));
    }
    if (!orderRequest.discountAmount) {
      setOrderRequest((prev) => ({ ...prev, discountAmount: discount }));
    }
    if (!orderRequest.addressID) {
      setOrderRequest((prev) => ({ ...prev, addressID: address.addressID }));
    }
    if (!orderRequest.shippingCostID) {
      setOrderRequest((prev) => ({
        ...prev,
        shippingCostID: shipping.shippingCostID,
      }));
    }
    if (!orderRequest.subtotal) {
      setOrderRequest((prev) => ({ ...prev, subtotal: subtotal }));
    }
    // if (!orderRequest.totalAmount) {
    setOrderRequest((prev) => ({ ...prev, totalAmount: total }));
    // }
  }, [Cart, discount, shipping, subtotal, total, address]);
  useEffect(() => {
    if (
      orderRequest.addressID &&
      orderRequest.userID &&
      orderRequest.discountAmount &&
      orderRequest.shippingCostID &&
      orderRequest.subtotal &&
      orderRequest.totalAmount
    ) {
      onPassRequest(orderRequest);
    }
  }, [orderRequest]);
  return (
    <>
      <aside className="order-summary-sidebar">
        <h2>Order Summary</h2>

        <div className="summary-items"></div>

        <div className="summary-divider"></div>

        <div className="summary-row">
          <span>Subtotal</span>
          <span>${subtotal}</span>
        </div>

        <div className="summary-row">
          <span>Shipping</span>
          <span>${shipping.shippingCost}</span>
        </div>

        <div className="summary-row">
          <span>Discount</span>
          <span className="discount-amount">
            -$
            {discount}
          </span>
        </div>
        <div className="summary-row">
          <span>Coupon</span>
          <span className="discount-amount">
            -$
            {coupon.discount}
          </span>
        </div>

        <div className="summary-divider"></div>

        <div className="summary-row total-row">
          <span>Total</span>
          <span>${total}</span>
        </div>

        <div className="shipping-info">
          <h4>Shipping Address</h4>
          <p>Main Address: {selectedAddress.mainAddress}</p>
          {selectedAddress.backUpAddress && (
            <p>BackUp Address: {selectedAddress.backUpAddress}</p>
          )}
          <p>PhoneNumber : {user?.phoneNumber}</p>
          {selectedAddress.backUpPhoneNumber && (
            <p>Back Up PhoneNumber : {selectedAddress.backUpPhoneNumber}</p>
          )}
        </div>
        <div className="accepted-cards">
          <p>Accepted Payment Methods:</p>
          <div className="card-icons">
            <span>💳</span>
            <span>🅿️</span>
            <span>💵</span>
          </div>
        </div>
      </aside>
    </>
  );
}
