import React, { useEffect, useState } from "react";
import { PaymentRequestModel } from "../../../../Core/DTO/Payment/Payment-Request-Model";
import Order_Failed_Model from "../Model/Order-Failed-Model";
import Order_Success_Model from "../Model/Order-Success-Model";
import { OrderRequest } from "../../../../Core/DTO/OrderDTO/OrderRequest";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import {
  addPayment,
  DeleteLastPayment,
} from "../../../../Core/Services/PaymentsServices/Payments.service";
import { CreateOrder } from "../../../../Core/Services/OrderServices/OrderService";
import { OrderResponse } from "../../../../Core/DTO/OrderDTO/OrderResponse";
import { useCartStore } from "../../../../Core/Global-State-Management/Zusstand/CartStore/CartUpdateStore";
import { PaymentTypeEnum } from "../../../../Core/Enums/PaymentTypeEnum";
import { PaymentCheckOutEnum } from "../../../../Core/Enums/PaymentCheckOutEnum";

type props = {
  onRedirect: (value: PaymentCheckOutEnum) => void;
  orderRequest: OrderRequest;
};
export default function Payment({ onRedirect, orderRequest }: props) {
  const [currentPaymentType, setCurrentPaymentType] = useState<PaymentTypeEnum>(
    PaymentTypeEnum.CreditCard,
  );
  const [order, setOrder] = useState<OrderResponse>(new OrderResponse());
  const [isLoading, setLoading] = useState<boolean>(false);
  const [isPaymentSuccess, setIsPaymentSuccess] = useState<boolean>(false);
  const [isPaymentDone, setisPaymentDone] = useState<boolean>(false);
  const { user } = useAuth();
  const { triggerCartUpdate } = useCartStore();

  const addNewOrder = async () => {
    console.log(orderRequest);
    try {
      const response = await CreateOrder(orderRequest);
      setLoading(false);
      setisPaymentDone(true);
      if (response.isSuccess && response.data) {
        setOrder(response.data);
        setisPaymentDone(true);
        setIsPaymentSuccess(true);
        triggerCartUpdate();
        onRedirect(PaymentCheckOutEnum.Finished);
      } else {
        setIsPaymentSuccess(false);
      }
    } catch (err) {
      if (err) {
        console.error(err);
        setIsPaymentSuccess(false);
      }
    }
  };
  async function submitFormOrder() {
    setLoading(true);
    const paymentRequest: PaymentRequestModel = new PaymentRequestModel();
    paymentRequest.amount = orderRequest.totalAmount;
    paymentRequest.paymentMethod = currentPaymentType;
    paymentRequest.userID = user?.userID!;
    try {
      const response = await addPayment(paymentRequest);
      console.log(response.data);

      if (response.isSuccess && response.data) {
        console.log("payment response  :", response.data);
        setIsPaymentSuccess(true);

        onRedirect(PaymentCheckOutEnum.Confirmation);
        orderRequest.paymentID = response.data?.paymentID;
        setIsPaymentSuccess(response.isSuccess);
      } else {
        setLoading(false);
        setIsPaymentSuccess(false);
        setisPaymentDone(true);
      }
    } catch (err) {
      if (err) {
        setLoading(false);
        setisPaymentDone(true);
        setIsPaymentSuccess(false);
        console.error(err);
      }
    }
  }

  useEffect(() => {
    if (isPaymentSuccess) {
      orderRequest.orderCouponID = orderRequest.orderCouponID
        ? orderRequest.orderCouponID
        : null;
      console.log("order request sent ");
      console.log(orderRequest);

      addNewOrder();
    }
  }, [isPaymentSuccess]);
  return (
    <>
      <div className="payment-content">
        <h1 className="page-title">Payment Information</h1>

        <div className="payment-methods-tabs">
          <button
            className={`payment-tab ${currentPaymentType === PaymentTypeEnum.CreditCard ? "active" : ""}`}
            onClick={() => setCurrentPaymentType(PaymentTypeEnum.CreditCard)}
          >
            <span className="tab-icon">💳</span>
            <span>Credit/Debit Card</span>
          </button>

          <button
            className={`payment-tab ${currentPaymentType === PaymentTypeEnum.Cash ? "active" : ""}`}
            onClick={() => setCurrentPaymentType(PaymentTypeEnum.Cash)}
          >
            <span className="tab-icon">💵</span>
            <span>Cash on Delivery</span>
          </button>
        </div>
        {currentPaymentType === "CreditCard" && (
          <div className="payment-form-container">
            <form className="payment-form">
              <div className="form-section">
                <h3 className="section-title">Card Details</h3>

                <div className="form-group">
                  <label>Card Number</label>
                  <div className="card-input-wrapper">
                    <input type="text" placeholder="1234 5678 9012 3456" />
                    <div className="card-logos">
                      <img
                        src="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='30' height='20'%3E%3Crect width='30' height='20' rx='3' fill='%231434CB'/%3E%3Ctext x='15' y='14' font-family='Arial' font-size='8' fill='white' text-anchor='middle' font-weight='bold'%3EVISA%3C/text%3E%3C/svg%3E"
                        alt="Visa"
                      />
                      <img
                        src="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='30' height='20'%3E%3Crect width='30' height='20' rx='3' fill='%23EB001B'/%3E%3Ccircle cx='12' cy='10' r='6' fill='%23FF5F00' opacity='0.8'/%3E%3Ccircle cx='18' cy='10' r='6' fill='%23F79E1B' opacity='0.8'/%3E%3C/svg%3E"
                        alt="Mastercard"
                      />
                    </div>
                  </div>
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label>Cardholder Name</label>
                    <input type="text" placeholder="John Doe" />
                  </div>
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label>Expiry Date</label>
                    <input type="text" placeholder="MM/YY" />
                  </div>
                  <div className="form-group">
                    <label>CVV</label>
                    <div className="cvv-input-wrapper">
                      <input type="text" placeholder="123" />
                      <span
                        className="cvv-info"
                        title="3-4 digit security code on the back of your card"
                      >
                        ℹ️
                      </span>
                    </div>
                  </div>
                </div>
              </div>

              <div className="form-actions">
                <button
                  type="button"
                  className="btn-place-order"
                  onClick={submitFormOrder}
                >
                  {!isLoading && <span className="btn-text">Place Order</span>}
                  {isLoading && (
                    <span className="btn-loader">Processing...</span>
                  )}
                </button>
              </div>
            </form>
          </div>
        )}

        {currentPaymentType === "Cash" && (
          <div className="payment-form-container">
            <div className="payment-placeholder">
              <div className="placeholder-icon">💵</div>
              <h3>Cash on Delivery</h3>
              <p>
                Pay when you receive your order. A small COD fee of $5 will be
                added.
              </p>
              <div className="cod-notice">
                <p>
                  <strong>Please Note:</strong>
                </p>
                <ul>
                  <li>Payment must be made in cash to the delivery person</li>
                  <li>Please keep exact change ready</li>
                  <li>COD orders cannot be cancelled once shipped</li>
                </ul>
              </div>
              <button className="btn-primary" onClick={submitFormOrder}>
                Confirm Order
              </button>
            </div>
          </div>
        )}

        <div className="security-badges">
          <div className="badge">
            <span className="badge-icon">🔒</span>
            <div>
              <strong>Secure Payment</strong>
              <p>Your payment information is encrypted</p>
            </div>
          </div>
          <div className="badge">
            <span className="badge-icon">✓</span>
            <div>
              <strong>SSL Secured</strong>
              <p>256-bit encryption</p>
            </div>
          </div>
          <div className="badge">
            <span className="badge-icon">🛡️</span>
            <div>
              <strong>PCI Compliant</strong>
              <p>Payment Card Industry certified</p>
            </div>
          </div>
        </div>
      </div>

      {isPaymentDone && (
        <>
          {!isPaymentSuccess && (
            <Order_Failed_Model onTryAgain={() => setisPaymentDone(false)} />
          )}
          {isPaymentSuccess && <Order_Success_Model order={order} />}
        </>
      )}
    </>
  );
}
