import { Link } from "react-router-dom";
import { OrderResponse } from "../../../../Core/DTO/OrderDTO/OrderResponse";
type props = {
  order: OrderResponse;
};
export default function Order_Success_Model({ order }: props) {
  return (
    <>
      <div className="Order-confirmation-modal">
        <div className="Order-confirmation-modal-content">
          <div className="Order-confirmation-animation">
            <div className="Order-success-checkmark">
              <div className="Order-check-icon">
                <span className="Order-icon-line line-tip"></span>
                <span className="Order-icon-line line-long"></span>
                <div className="Order-icon-circle"></div>
                <div className="Order-icon-fix"></div>
              </div>
            </div>
          </div>

          <h2 className="Order-confirmation-title">Order Confirmed! 🎉</h2>
          <p className="Order-confirmation-message">
            Thank you for your purchase!
          </p>

          <div className="Order-order-details">
            <div className="Order-order-info-row">
              <span>Order Number:</span>
              <strong id="orderNumber">{order.orderNumber}</strong>
            </div>
            <div className="Order-order-info-row">
              <span>Total Amount:</span>
              <strong id="orderTotal">${order.totalAmount}</strong>
            </div>
            <div className="Order-order-info-row">
              <span>Estimated Delivery:</span>
              <strong id="estimatedDelivery">3-5 business days</strong>
            </div>
          </div>

          <p className="Order-confirmation-note">
            A confirmation email has been sent to your email address.
          </p>

          <div className="Order-confirmation-actions">
            <Link to="/Home" className="Order-btn-primary ">
              Continue Shopping
            </Link>
          </div>
        </div>
      </div>
    </>
  );
}
