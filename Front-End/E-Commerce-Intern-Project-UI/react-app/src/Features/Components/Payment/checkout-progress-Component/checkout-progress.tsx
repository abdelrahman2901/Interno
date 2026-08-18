import { useEffect, useState } from "react";
import { PaymentCheckOutEnum } from "../../../../Core/Enums/PaymentCheckOutEnum";

type props = {
  step: PaymentCheckOutEnum;
};
export default function CheckoutProgress({ step }: props) {
  const [currentStep, setCurrentStep] = useState<PaymentCheckOutEnum>(
    PaymentCheckOutEnum.Shipping,
  );
  useEffect(() => {
    setCurrentStep(step);
  }, [step]);
  return (
    <>
      <div className="checkout-progress">
        <div className="progress-container">
          <div className="progress-step completed">
            <div className="step-circle">✓</div>
            <span className="step-label">Cart</span>
          </div>
          <div
            className={`progress-line ${currentStep === PaymentCheckOutEnum.Shipping ? "active" : currentStep === PaymentCheckOutEnum.Confirmation || PaymentCheckOutEnum.Payment ? "completed" : ""}`}
          ></div>
          <div
            className={`progress-step ${currentStep === PaymentCheckOutEnum.Shipping ? "active" : currentStep === PaymentCheckOutEnum.Confirmation || PaymentCheckOutEnum.Payment ? "completed" : ""}`}
          >
            <div className="step-circle">✓</div>
            <span className="step-label">Shipping</span>
          </div>
          <div
            className={`progress-line ${currentStep === PaymentCheckOutEnum.Payment ? "active" : currentStep === PaymentCheckOutEnum.Confirmation ? "completed" : ""}`}
          ></div>
          <div
            className={`progress-step ${currentStep === PaymentCheckOutEnum.Payment ? "active" : currentStep === PaymentCheckOutEnum.Confirmation ? "completed" : ""}`}
          >
            <div className="step-circle">3</div>
            <span className="step-label">Payment</span>
          </div>
          <div
            className={`progress-line ${currentStep === PaymentCheckOutEnum.Confirmation ? "active" : currentStep === PaymentCheckOutEnum.Finished ? "completed" : ""}`}
          ></div>
          <div
            className={`progress-step ${currentStep === PaymentCheckOutEnum.Confirmation ? "active" : currentStep === PaymentCheckOutEnum.Finished ? "completed" : ""}`}
          >
            <div className="step-circle">4</div>
            <span className="step-label">Confirmation</span>
          </div>
        </div>
      </div>
    </>
  );
}
