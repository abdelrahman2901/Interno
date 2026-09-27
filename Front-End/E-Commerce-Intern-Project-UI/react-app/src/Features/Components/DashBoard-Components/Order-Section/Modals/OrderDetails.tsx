import { useEffect, useState } from "react";
import { InternoMicroserviceClient, OrderDetails } from "../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API } from "../../../../../Core/Shared/API-URL";
type props = {
  orderIDProps: string | null;
  onClose: () => void;
};
const Base_Url = "http://localhost:5000";

export default function OrderDetailsModal({ orderIDProps, onClose }: props) {
  const Client = new InternoMicroserviceClient(undefined, API);
  const [orderDetails, setOrderDetails] = useState<OrderDetails>(
    new OrderDetails(),
  );
  const loadOrder = async () => {
    try {
      const response = await Client.getOrder(orderIDProps!);
      if (response.data && response.isSuccess) {
        setOrderDetails(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (orderIDProps) {
      loadOrder();
    }
  }, [orderIDProps]);

  function extractDate(orderdate?: Date | string) {
    const date = new Date(orderdate ?? Date.now());
    return `${date.toLocaleString("Default", { month: "short" })}  ${date.getDate()},${date.getFullYear()}`;
  }
  function extractDateTime(orderdate?: Date | string) {
    const date = new Date(orderdate ?? Date.now());
    return date.toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
      hour12: true,
    });
  }
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-large">
          <div className="Custom-modal-header">
            <h3>
              Order Details - <span>#{orderDetails.orderNumber}</span>
            </h3>
            <button className="modal-close" onClick={onClose}>
              &times;
            </button>
          </div>
          <div className="Custom-modal-body">
            <div className="order-info-grid">
              <div className="info-card">
                <h4>Customer Information</h4>
                <p>
                  <strong>Name:</strong>
                  <span>{orderDetails.userName}</span>
                </p>
                <p>
                  <strong>Email:</strong>
                  <span>{orderDetails.userEmail}</span>
                </p>
              </div>
              <div className="info-card">
                <h4>Order Information</h4>
                <p>
                  <strong>Order Date:</strong>
                  <span>
                    {extractDate(orderDetails.orderDate)}
                    {extractDateTime(orderDetails.orderDate)}
                  </span>
                </p>
                <p>
                  <strong>Payment Method:</strong>
                  <span>{orderDetails.paymentMethod}</span>
                </p>
                <p>
                  <strong>Status:</strong>
                  <span
                    className={`status-badge ${orderDetails.orderStatus?.toLowerCase()}`}
                  >
                    {orderDetails.orderStatus}
                  </span>
                </p>
              </div>
              <div className="info-card">
                <h4>Shipping Address</h4>
                <p>
                  {orderDetails.address?.addressLabel}
                  <br />
                  {orderDetails.address?.mainAddress}
                  <br />
                  {orderDetails.address?.cityID}-
                  {orderDetails.address?.areaID}
                  <br />
                  {orderDetails.address?.backUpPhoneNumber &&
                    orderDetails.address.backUpPhoneNumber}
                  <br />
                </p>
              </div>
            </div>

            <div className="order-items-section">
              <h4>Order Items</h4>
              <table className="items-table">
                <thead>
                  <tr>
                    <th>Product</th>

                    <th>Price</th>
                    <th>Quantity</th>
                    <th>Total</th>
                  </tr>
                </thead>
                <tbody>
                  {orderDetails.orderItems?.map((orderItem) => (
                    <tr>
                      <td>
                        <div className="item-info">
                          <img
                            src={`${Base_Url}/ProductsImages/${orderItem.product?.parentCategoryName}/${orderItem.product?.categoryName}/${orderItem.product?.productImageUrl}`}
                            alt="Product"
                          />
                          <div>
                            <strong>{orderItem.product?.productName}</strong>
                            <small>
                              Size: {orderItem.product?.sizeName?.slice(0, 1)},
                              Color: {orderItem.product?.colorName}
                            </small>
                          </div>
                        </div>
                      </td>

                      <td>
                        $
                        {orderItem.product?.salePrice !== 0
                          ? orderItem.product?.salePrice
                          : orderItem.product?.price}
                      </td>
                      <td>{orderItem.quantity}</td>
                      <td>
                        <strong>
                          $
                          {((orderItem.product?.salePrice !== 0
                            ? orderItem.product?.salePrice
                            : orderItem.product?.price) ?? 0) *
                            (orderItem.quantity ?? 0)}
                        </strong>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="order-summary-section">
              <div className="summary-row">
                <span>Subtotal:</span>
                <strong>${orderDetails.subtotal}</strong>
              </div>
              <div className="summary-row">
                <span>Shipping:</span>
                <strong>${orderDetails.shippingCosts?.shippingCost}</strong>
              </div>

              <div className="summary-row total-row">
                <span>Total:</span>
                <strong>${orderDetails.totalAmount}</strong>
              </div>
            </div>
          </div>
          <div className="Custom-modal-footer">
            <button className="btn-secondary" onClick={onClose}>
              Close
            </button>
            <button className="btn-primary">Print Invoice</button>
          </div>
        </div>
      </div>
    </>
  );
}
