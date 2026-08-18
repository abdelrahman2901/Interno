import { useEffect, useState } from "react";
// import "./OverViewSection.css";
import { GetAllOrders } from "../../../../Core/Services/OrderServices/OrderService";
import { GetProducts } from "../../../../Core/Services/ProductServices/ProductServiceQuery";

export default function OverViewSection() {
  const [totalProducts, setTotalProducts] = useState<number>(0);
  const [totalOrders, setTotalOrders] = useState<number>(0);
  const [totalRevenue, setTotalRevenue] = useState<number>(0);
  const [totalProfit, setTotalProfit] = useState<number>(0);

  const loadOrdersCount = async () => {
    try {
      const response = await GetAllOrders();
      if (response.data && response.isSuccess) {
        setTotalOrders(response.data.length);
        setTotalProfit(
          response.data
            .slice()
            .filter((r) => r.orderStatus === "Delivered")
            .reduce((total, order) => total + order.totalAmount, 0),
        );
        setTotalRevenue(
          response.data
            .slice()
            .filter((r) => r.orderStatus !== "Cancelled")
            .reduce((total, order) => total + order.totalAmount, 0),
        );
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadProductsCount = async () => {
    try {
      const response = await GetProducts();
      if (response.data && response.isSuccess) {
        setTotalProducts(response.data.length);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  useEffect(() => {
    loadOrdersCount();
    loadProductsCount();
  }, []);
  return (
    <>
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon">💰</div>
          <div className="stat-info">
            <h3>Total Revenue</h3>
            <p className="stat-value">${totalRevenue}</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">💰</div>
          <div className="stat-info">
            <h3>Total Profit</h3>
            <p className="stat-value">${totalProfit}</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">📦</div>
          <div className="stat-info">
            <h3>Total Orders</h3>
            <p className="stat-value">{totalOrders}</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">🏷️</div>
          <div className="stat-info">
            <h3>Total Products</h3>
            <p className="stat-value">{totalProducts}</p>
          </div>
        </div>
      </div>

      <div className="recent-activity">
        <h3>Recent Activity</h3>
        <div className="activity-list">
          <div className="activity-item">
            <span className="activity-icon">🛍️</span>
            <div className="activity-content">
              <p>
                <strong>New order</strong> from John Doe
              </p>
              <span className="activity-time">2 minutes ago</span>
            </div>
          </div>
          <div className="activity-item">
            <span className="activity-icon">📦</span>
            <div className="activity-content">
              <p>
                <strong>Order #1234</strong> has been shipped
              </p>
              <span className="activity-time">15 minutes ago</span>
            </div>
          </div>
          <div className="activity-item">
            <span className="activity-icon">🏷️</span>
            <div className="activity-content">
              <p>
                <strong>New product</strong> added to catalog
              </p>
              <span className="activity-time">1 hour ago</span>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
