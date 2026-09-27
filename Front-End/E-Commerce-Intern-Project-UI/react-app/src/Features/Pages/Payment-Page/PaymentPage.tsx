import { useEffect, useState } from "react";
import Payment from "../../Components/Payment/Payment-Component/Payment";
import CheckoutProgress from "../../Components/Payment/checkout-progress-Component/checkout-progress";
import Shipping from "../../Components/Payment/Shipping-Component/Shipping";
import "./Payment.css";
import OrderSideBar from "../../Components/Payment/Order-SideBar/OrderSideBar";
import { AddressDetails, CreateOrderRequest, InternoMicroserviceClient, OrderCouponResponse } from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API } from "../../../Core/Shared/API-URL";
import { useSearchParams } from "react-router-dom";
import { PaymentCheckOutEnum } from "../../../Core/Enums/PaymentCheckOutEnum";
export default function PaymentPage() {
  const [currentStep, setCurrentStep] = useState<PaymentCheckOutEnum>(
    PaymentCheckOutEnum.Shipping,
  );
  const Client = new InternoMicroserviceClient(undefined, API);
  const [address, setAddress] = useState<AddressDetails>(new AddressDetails());
  const [orderRequest, setOrderRequest] = useState<CreateOrderRequest>(
    new CreateOrderRequest(),
  );
  const [coupon, setCoupon] = useState<OrderCouponResponse>(
    new OrderCouponResponse(),
  );
  const [params] = useSearchParams();

  const loadCoupon = async (appliedCoupon: string) => {
    try {
      const response = await Client.getCouponByCode(appliedCoupon);
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

  function onPassingRequestData(request: CreateOrderRequest) {
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
