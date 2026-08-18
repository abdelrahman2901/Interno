import { OrderDetails } from "../../../../Core/DTO/OrderDTO/OrderDetails";
import { GetAllUserOrders } from "../../../../Core/Services/OrderServices/OrderService";
import "./OverVIew.css";
type props = {
  swtichSection_OutPut: (
    section:
      | "Orders"
      | "Profile-Settings"
      | "Security"
      | "OverView"
      | "Addresses",
  ) => void;

  orders: OrderDetails[];
};
export default function AccountOverView({
  swtichSection_OutPut,
  orders,
}: props) {
  function switchSection(
    section:
      | "Orders"
      | "Profile-Settings"
      | "Security"
      | "OverView"
      | "Addresses",
  ) {
    swtichSection_OutPut(section);
  }

  return (
    <>
      <h2 className="section-title">Account Overview</h2>

      <div className="Account-stats-grid">
        <div className="Account-stat-card">
          <div className="Account-stat-icon">📦</div>
          <div className="Account-stat-info">
            <h4>Total Orders</h4>
            <p className="Account-stat-value">{orders.length}</p>
          </div>
        </div>
        <div className="Account-stat-card">
          <div className="Account-stat-icon">💰</div>
          <div className="Account-stat-info">
            <h4>Total Spent</h4>
            <p className="Account-stat-value">
              $
              {orders
                .slice()
                .filter((r) => r.orderStatus !== "Cancelled")
                .reduce((total, order) => total + order.totalAmount, 0)}
            </p>
          </div>
        </div>
        <div className="Account-stat-card">
          <div className="Account-stat-icon">♡</div>
          <div className="Account-stat-info">
            <h4>Wishlist Items</h4>
            <p className="Account-stat-value">N</p>
          </div>
        </div>
        <div className="Account-stat-card">
          <div className="Account-stat-icon">🎁</div>
          <div className="Account-stat-info">
            <h4>Reward Points</h4>
            <p className="Account-stat-value">N</p>
          </div>
        </div>
      </div>

      <div className="Account-quick-actions">
        <h3>Quick Actions</h3>
        <div className="Account-actions-grid">
          <button
            className="Account-action-card"
            onClick={() => switchSection("Orders")}
          >
            <span className="Account-action-icon">📦</span>
            <span className="Account-action-text">View Orders</span>
          </button>
          <button
            className="Account-action-card"
            onClick={() => switchSection("Profile-Settings")}
          >
            <span className="Account-action-icon">✏️</span>
            <span className="Account-action-text">Edit Profile</span>
          </button>
          <button
            className="Account-action-card"
            onClick={() => switchSection("Security")}
          >
            <span className="Account-action-icon">🔒</span>
            <span className="Account-action-text">Change Password</span>
          </button>
          <button
            className="Account-action-card"
            onClick={() => switchSection("Addresses")}
          >
            <span className="Account-action-icon">📍</span>
            <span className="Account-action-text">Manage Addresses</span>
          </button>
        </div>
      </div>

      <div className="Account-recent-orders">
        <h3>Recent Orders</h3>
        {/* <div className="Account-orders-list">
          <div className="Account-order-item">
            <div className="Account-order-info">
              <span className="Account-order-number">#ORD-2024-001</span>
              <span className="Account-order-date">Jan 15, 2024</span>
            </div>
            <span className="Account-order-status Account-delivered">
              Delivered
            </span>
            <span className="Account-order-total">$89.00</span>
            <button className="Account-btn-small">View Details</button>
          </div>
          <div className="Account-order-item">
            <div className="Account-order-info">
              <span className="Account-order-number">#ORD-2024-002</span>
              <span className="Account-order-date">Jan 20, 2024</span>
            </div>
            <span className="Account-order-status Account-shipped">
              Shipped
            </span>
            <span className="Account-order-total">$145.00</span>
            <button className="Account-btn-small">Track Order</button>
          </div>
        </div> */}
      </div>
    </>
  );
}
