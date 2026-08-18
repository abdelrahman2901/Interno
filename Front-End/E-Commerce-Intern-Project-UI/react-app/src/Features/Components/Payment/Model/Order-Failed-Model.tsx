import { Link } from "react-router-dom";

type props = {
  onTryAgain: () => void;
};
export default function Order_Failed_Model({ onTryAgain }: props) {
  return (
    <>
      <div className="Order-confirmation-modal">
        <div className="Order-confirmation-modal-content failed">
          <div className="Order-confirmation-animation">
            <div className="Order-error-icon">
              <span className="Order-error-x">✗</span>
            </div>
          </div>

          <h2 className="Order-confirmation-title error">Payment Failed</h2>
          <p className="Order-confirmation-message" id="errorMessage">
            We couldn't process your payment. Please try again.
          </p>

          <div className="Order-error-reasons">
            <p>
              <strong>Common reasons:</strong>
            </p>
            <ul>
              <li>Insufficient funds</li>
              <li>Incorrect card details</li>
              <li>Card expired</li>
              <li>Payment gateway error</li>
            </ul>
          </div>

          <div className="Order-confirmation-actions">
            <button className="Order-btn-secondary" onClick={onTryAgain}>
              Try Again
            </button>
            <Link to="/Home/Cart" className="Order-btn-outline">
              Back to Cart
            </Link>
          </div>
        </div>
      </div>
    </>
  );
}
