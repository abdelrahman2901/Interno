import { useEffect, useState } from "react";
import Payment from "../../Components/Payment/Payment-Component/Payment";
import CheckoutProgress from "../../Components/Payment/checkout-progress-Component/checkout-progress";
import Shipping from "../../Components/Payment/Shipping-Component/Shipping";
import "./Payment.css";
import OrderSideBar from "../../Components/Payment/Order-SideBar/OrderSideBar";
import { AddressDetails } from "../../../Core/DTO/AddressDTO/AddressDetails";
import { OrderRequest } from "../../../Core/DTO/OrderDTO/OrderRequest";
import { useSearchParams } from "react-router-dom";
import { OrderCouponModel } from "../../../Core/DTO/OrdeCouponDTO/OrderCouponResponse";
import { GetCouponByCode } from "../../../Core/Services/OrderCouponServices/OrderCouponService";
import { PaymentCheckOutEnum } from "../../../Core/Enums/PaymentCheckOutEnum";

export default function PaymentPage() {
  const [currentStep, setCurrentStep] = useState<PaymentCheckOutEnum>(
    PaymentCheckOutEnum.Shipping,
  );
  const [address, setAddress] = useState<AddressDetails>(new AddressDetails());
  const [orderRequest, setOrderRequest] = useState<OrderRequest>(
    new OrderRequest(),
  );
  const [coupon, setCoupon] = useState<OrderCouponModel>(
    new OrderCouponModel(),
  );
  const [params] = useSearchParams();

  const loadCoupon = async (appliedCoupon: string) => {
    try {
      const response = await GetCouponByCode(appliedCoupon);
      if (response.data && response.isSuccess) {
        setCoupon(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  useEffect(() => {
    const couponCode = params.get("CouponCode");
    console.log(couponCode);

    if (couponCode) {
      loadCoupon(couponCode);
    }
  }, []);

  function onSelectAddress(address: AddressDetails) {
    setAddress(address);
  }

  function onPassingRequestData(request: OrderRequest) {
    setOrderRequest(request);
  }

  function onRedirectToStep(value: PaymentCheckOutEnum) {
    console.log(value);

    setCurrentStep(value);
  }
  function renderStep() {
    switch (currentStep) {
      case "Shipping": {
        return (
          <Shipping
            onSelectingAddress={onSelectAddress}
            onRedirectToPayment={onRedirectToStep}
          />
        );
      }
      case "Confirmation":
      case "Finished":
      case "Payment": {
        return (
          <Payment onRedirect={onRedirectToStep} orderRequest={orderRequest} />
        );
      }
    }
  }
  return (
    <>
      <CheckoutProgress step={currentStep} />
      <div className="payment-container">
        {renderStep()}

        <OrderSideBar
          address={address}
          couponProps={coupon}
          onPassRequest={onPassingRequestData}
        />
      </div>
    </>
  );
}
