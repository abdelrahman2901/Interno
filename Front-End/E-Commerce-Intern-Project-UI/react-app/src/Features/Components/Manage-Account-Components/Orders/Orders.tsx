import { useEffect, useState } from "react";
import "./Orders.css";
import { OrderDetails } from "../../../../Core/DTO/OrderDTO/OrderDetails";
import { Link } from "react-router-dom";
import { UpdateOrderRequest } from "../../../../Core/DTO/OrderDTO/UpdateOrderRequest";
import {
  GetAllUserOrders,
  UpdateOrder,
  UpdateOrderStatus,
} from "../../../../Core/Services/OrderServices/OrderService";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import RatingModal from "./Modal/RatingModal";
import { ProductRatesDetails } from "../../../../Core/DTO/ProductRatesDTO/ProductRatesDetails";
import { GetUserRating } from "../../../../Core/Services/ProductRatingServices/ProductRatingService";
import SuccessRateModal from "./Modal/SuccessRateModal";
import { GetCouponByCode } from "../../../../Core/Services/OrderCouponServices/OrderCouponService";

type props = {
  orders_Prop: OrderDetails[];
};
export default function AccountOrders({ orders_Prop }: props) {
  const Base_Url = "https://localhost:7164";
  const [CurrentOrder, SetCurrentOrder] = useState<
    "all" | "delivered" | "shipped" | "processing" | "cancelled"
  >("all");
  const [_orders, setPrivOrders] = useState<OrderDetails[]>([]);
  const [orders, setOrders] = useState<OrderDetails[]>([]);
  const [isRatingProduct, setIsRatingProduct] = useState<boolean>(false);
  const [selectedOrderID, setSelectedOrderID] = useState<string>("");
  const [userOrderRating, setUserOrderRating] = useState<ProductRatesDetails[]>(
    [],
  );
  const [isRateDoneSuccessfully, setIsRateDoneSuccessfully] =
    useState<boolean>(false);
  const { user } = useAuth();
  const loadUserRating = async () => {
    try {
      const response = await GetUserRating(user?.userID!);
      if (response.data && response.isSuccess) {
        setUserOrderRating(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const laodOrders = async () => {
    try {
      const response = await GetAllUserOrders(user?.userID!);
      if (response.isSuccess) {
        console.log(response.data);

        setPrivOrders(response.data!);
        setOrders(response.data!);
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };

  useEffect(() => {
    if (_orders.length === 0 || orders.length === 0) {
      console.log(orders_Prop);

      setOrders(orders_Prop);
      console.log(orders_Prop);

      setPrivOrders(orders_Prop);
    }

    if (userOrderRating.length === 0) {
      loadUserRating();
    }
  }, []);

  function extractDate(orderdate: string) {
    const date = new Date(orderdate);
    return `${date.toLocaleString("Default", { month: "long" })} ${date.getDate()} ${date.getFullYear()}`;
  }
  function extractStatus(orderStatus: string) {
    switch (orderStatus) {
      case "Processing": {
        return "processing";
      }
      case "Shipped": {
        return "shipped";
      }
      case "Delivered": {
        return "delivered";
      }
      case "Cancelled": {
        return "cancelled";
      }
    }
  }

  function filterOrders(
    filter: "all" | "delivered" | "shipped" | "processing" | "cancelled",
  ) {
    SetCurrentOrder(filter);
    if (filter === "all") {
      setOrders(_orders);
      return;
    }
    setOrders(
      _orders.slice().filter((r) => r.orderStatus.toLowerCase() === filter),
    );
  }

  async function CancelOrder(order: OrderDetails) {
    const request: UpdateOrderRequest = {
      orderID: order.orderID,
      orderStatus: "Cancelled",
      shippingCostID: order.shippingCosts.shippingCostID,
      addressID: order.address.addressID,
    };
    console.log(request);

    try {
      const response = await UpdateOrder(request);
      if (response.isSuccess) {
        laodOrders();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }

  function RateOrder(orderID: string) {
    setIsRatingProduct(true);
    setSelectedOrderID(orderID);
  }

  function onActionResultEvent() {
    loadUserRating();
    setIsRatingProduct(false);

    setIsRateDoneSuccessfully(true);
  }
  function onCloseSuccessModalEvent() {
    setIsRateDoneSuccessfully(false);
  }
  async function ChangeOrderToProc(orderID: string) {
    try {
      const response = await UpdateOrderStatus({
        OrderID: orderID,
        NewStatus: "Processing",
      });
      if (response.isSuccess) {
        laodOrders();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  return (
    <>
      <h2 className="section-title">My Orders</h2>

      <div className="Account-orders-filters">
        <button
          className={
            "Account-filter-btn " + (CurrentOrder === "all" ? "active" : "")
          }
          onClick={() => filterOrders("all")}
        >
          All Orders
        </button>
        <button
          className={
            "Account-filter-btn " +
            (CurrentOrder === "delivered" ? "active" : "")
          }
          onClick={() => filterOrders("delivered")}
        >
          Delivered
        </button>
        <button
          className={
            "Account-filter-btn " + (CurrentOrder === "shipped" ? "active" : "")
          }
          onClick={() => filterOrders("shipped")}
        >
          Shipped
        </button>
        <button
          className={
            "Account-filter-btn " +
            (CurrentOrder === "processing" ? "active" : "")
          }
          onClick={() => filterOrders("processing")}
        >
          Processing
        </button>
        <button
          className={
            "Account-filter-btn " +
            (CurrentOrder === "cancelled" ? "active" : "")
          }
          data-status="cancelled"
          onClick={() => filterOrders("cancelled")}
        >
          Cancelled
        </button>
      </div>

      <div className="Account-orders-grid">
        {orders.length === 0 && (
          <div className="empty-state">
            <div className="empty-state-icon">📁</div>
            <h4>Looks Like You Didnt Order Yet See Our Latest Products Now</h4>
            <button className="btn-place-order">
              <Link
                to={`/Home/Products?Category=All&SubCategory=All&Price=0&Size=All&Color=All`}
                className="nav-link"
              >
                Go To Product
              </Link>
            </button>
          </div>
        )}
        {orders.length > 0 && (
          <>
            {orders.map((order) => (
              <>
                <div className="Account-order-card" key={order.orderID}>
                  <div className="Account-order-header">
                    <div>
                      <h4 className="Account-order-number">
                        {order.orderNumber}
                      </h4>
                      <p className="Account-order-date">
                        Placed on {extractDate(order.orderDate)}
                      </p>
                    </div>
                    <span
                      className={`Account-order-status Account-${extractStatus(order.orderStatus)}`}
                    >
                      {order.orderStatus}
                    </span>
                  </div>

                  {order.orderItems.map((orderItem) => (
                    <div
                      className="Account-order-items"
                      key={orderItem.orderItemID}
                    >
                      <div className="Account-order-product">
                        <img
                          alt="Product"
                          src={`${Base_Url}/ProductsImages/${orderItem.product.parentCategoryName}/${orderItem.product.categoryName}/${orderItem.product.productImageUrl}`}
                        />
                        <div className="Account-product-details">
                          <h5>{orderItem.product.productName}</h5>
                          <p>
                            Size: {orderItem.product.sizeName} | Color:{" "}
                            {orderItem.product.colorName}
                          </p>
                          <p>Qty: {orderItem.quantity}</p>
                        </div>
                        <div className="Account-product-price">
                          $
                          {orderItem.product.salePrice !== 0 &&
                            orderItem.product.salePrice}
                          {!orderItem.product.salePrice &&
                            orderItem.product.price}
                        </div>
                      </div>
                    </div>
                  ))}
                  <div className="Account-order-footer">
                    <div className="Account-order-total ">
                      <span>Shipping Cost:</span>
                      <strong>${order.shippingCosts.shippingCost}</strong>
                    </div>
                    <div className="Account-order-total ">
                      <span>Discount:</span>
                      <strong className="discount-amount">
                        -${order.orderCoupon ? order.orderCoupon.discount : 0}
                      </strong>
                    </div>
                    <div className="Account-order-total ">
                      <span>Total:</span>
                      <strong>
                        $
                        {order.orderItems.reduce(
                          (total, order) =>
                            total +
                            (order.product.salePrice !== 0 &&
                            order.product.salePrice
                              ? order.product.salePrice
                              : order.product.price) *
                              order.quantity,
                          0,
                        ) +
                          order.shippingCosts.shippingCost -
                          (order.orderCoupon ? order.orderCoupon.discount : 0)}
                      </strong>
                    </div>
                    <div className="Account-order-actions">
                      {order.orderStatus === "Processing" && (
                        <button
                          className="Account-btn-small Account-btn-danger"
                          onClick={() => CancelOrder(order)}
                        >
                          Cancel Order
                        </button>
                      )}
                      {order.orderStatus === "Delivered" &&
                        !userOrderRating.some(
                          (r) =>
                            r.userID === user?.userID &&
                            order.orderItems.some(
                              (t) =>
                                t.product.productID === r.product.productID,
                            ),
                        ) && (
                          <button
                            className="Account-btn-small btn-submit-rating"
                            onClick={() => RateOrder(order.orderID)}
                          >
                            Rate Product
                          </button>
                        )}
                      {order.orderStatus === "Cancelled" && (
                        <button
                          className="Account-btn-small btn-submit-rating"
                          onClick={() => ChangeOrderToProc(order.orderID)}
                        >
                          Order Again
                        </button>
                      )}
                    </div>
                  </div>
                </div>
              </>
            ))}
          </>
        )}
      </div>
      {isRatingProduct && (
        <RatingModal
          onActionResult={onActionResultEvent}
          onClose={() => setIsRatingProduct(false)}
          orderIDProps={selectedOrderID}
        />
      )}
      {isRateDoneSuccessfully && (
        <SuccessRateModal onClose={onCloseSuccessModalEvent} />
      )}
    </>
  );
}
