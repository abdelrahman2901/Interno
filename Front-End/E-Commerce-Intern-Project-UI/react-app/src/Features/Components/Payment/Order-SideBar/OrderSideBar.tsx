import { useEffect, useState } from "react";
import { AddressDetails, CartDetailsResponse, CreateOrderRequest, InternoMicroserviceClient, OrderCouponResponse, ShippingCostDetails } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../Core/Shared/API-URL";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";
type props = {
  address: AddressDetails;
  onPassRequest: (request: CreateOrderRequest) => void;
  couponProps: OrderCouponResponse;
};
export default function OrderSideBar({
  address,
  onPassRequest,
  couponProps,
}: props) {
  const { user } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [Cart, setCart] = useState<CartDetailsResponse | null>(null);
  const [subtotal, setSubtotal] = useState<number>(0);
  const [shipping, setShipping] = useState<ShippingCostDetails>(
    new ShippingCostDetails(),
  );
  const [coupon, setCoupon] = useState<OrderCouponResponse>(
    new OrderCouponResponse(),
  );

  const [discount, setDiscount] = useState<number>(0);
  const [total, setTotal] = useState<number>(0);
  const [selectedAddress, setSelectedAddresss] = useState<AddressDetails>(
    new AddressDetails(),
  );
  const [orderRequest, setOrderRequest] = useState<CreateOrderRequest>(
    new CreateOrderRequest(),
  );
  useEffect(() => {
    if (couponProps) {
      setOrderRequest((prev) =>
        new CreateOrderRequest({
          ...prev,
          orderCouponID: couponProps.orderCouponID,
        }),
      );
      setCoupon(couponProps);
    }
  }, [couponProps]);
  const loadCartItems = async () => {
    try {
      const response = await Client.getUserCartItemsDetails();
      if (response.isSuccess) {
        setCart(response.data!);
        calculateOrderSummary();
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadAddressShippingCost = async () => {
    try {
      const response = await Client.getShippingCostDetailsByAreaID(
        address.areaID!,
      );
      if (response.isSuccess && response.data) {
        setOrderRequest((prev) =>
          new CreateOrderRequest({
            ...prev,
            shippingCostID: response.data?.shippingCostID!,
            totalAmount:
              (Cart?.cartItems?.reduce(
                (total, price) =>
                  total +
                  ((price.product?.salePrice !== 0
                    ? price.product?.salePrice
                    : price.product?.price) ?? 0) *
                    (price.quantity ?? 0),
                0,
              ) ?? 0) + (response.data?.shippingCost ?? 0),
          }),
        );
        setShipping(response.data);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
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
      Cart?.cartItems?.reduce(
        (total, price) =>
          total + (price.product?.price ?? 0) * (price.quantity ?? 0),
        0,
      ) ?? 0,
    );

    setDiscount(
      Cart?.cartItems
        ?.filter((r) => r.product?.salePrice !== 0 && r.product?.salePrice)
        .reduce(
          (total, item) =>
            total +
            ((item.product?.price ?? 0) - (item.product?.salePrice ?? 0)) *
              (item.quantity ?? 0),
          0,
        ) ?? 0,
    );

    const cartTotal =
      Cart?.cartItems?.reduce(
        (total, price) =>
          total +
          ((price.product?.salePrice !== 0 && price.product?.salePrice
            ? price.product.salePrice
            : price.product?.price) ?? 0) *
            (price.quantity ?? 0),
        0,
      ) ?? 0;
    const total =
      cartTotal + (shipping.shippingCost ?? 0) - (coupon.discount ?? 0) < 0
        ? 0
        : cartTotal + (shipping.shippingCost ?? 0) - (coupon.discount ?? 0);

    setTotal(total);
  }

  useEffect(() => {
    if (Cart && shipping) {
      calculateOrderSummary();
    }
  }, [shipping, Cart]);

  useEffect(() => {
    if (!orderRequest.userID) {
      setOrderRequest((prev) =>
        new CreateOrderRequest({ ...prev, userID: user?.userID! }),
      );
    }
    if (!orderRequest.discountAmount) {
      setOrderRequest((prev) =>
        new CreateOrderRequest({ ...prev, discountAmount: discount }),
      );
    }
    if (!orderRequest.addressID) {
      setOrderRequest((prev) =>
        new CreateOrderRequest({ ...prev, addressID: address.addressID }),
      );
    }
    if (!orderRequest.shippingCostID) {
      setOrderRequest((prev) =>
        new CreateOrderRequest({
          ...prev,
          shippingCostID: shipping.shippingCostID,
        }),
      );
    }
    if (!orderRequest.subtotal) {
      setOrderRequest((prev) =>
        new CreateOrderRequest({ ...prev, subtotal: subtotal }),
      );
    }
    setOrderRequest((prev) =>
      new CreateOrderRequest({ ...prev, totalAmount: total }),
    );
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
