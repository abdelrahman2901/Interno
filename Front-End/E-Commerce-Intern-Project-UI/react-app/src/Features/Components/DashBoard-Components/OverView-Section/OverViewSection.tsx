import { useEffect, useRef, useState } from "react";
import gsap from "gsap";
import { InternoMicroserviceClient } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";

const ORDERS_PAGE_SIZE = 20;

function useCountUp(value: number) {
  const [display, setDisplay] = useState(0);
  useEffect(() => {
    const counter = { n: 0 };
    const tween = gsap.to(counter, {
      n: value,
      duration: 0.7,
      ease: "power2.out",
      onUpdate: () => setDisplay(Math.round(counter.n)),
    });
    return () => {
      tween.kill();
    };
  }, [value]);
  return display;
}

export default function OverViewSection() {
  const [totalProducts, setTotalProducts] = useState<number>(0);
  const [totalOrders, setTotalOrders] = useState<number>(0);
  const [totalRevenue, setTotalRevenue] = useState<number>(0);
  const [totalProfit, setTotalProfit] = useState<number>(0);
  const statsRef = useRef<HTMLDivElement | null>(null);
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const shownRevenue = useCountUp(totalRevenue);
  const shownProfit = useCountUp(totalProfit);
  const shownOrders = useCountUp(totalOrders);
  const shownProducts = useCountUp(totalProducts);

  const loadOrdersCount = async () => {
    try {
      const response = await Client.getAllOrders(1, ORDERS_PAGE_SIZE);
      if (response.data && response.isSuccess) {
        const page = response.data;
        const items = page.items ?? [];
        setTotalOrders(page.totalCount ?? items.length);
        setTotalProfit(
          items
            .filter((r) => r.orderStatus === "Delivered")
            .reduce((total, order) => total + (order.totalAmount ?? 0), 0),
        );
        setTotalRevenue(
          items
            .filter((r) => r.orderStatus !== "Cancelled")
            .reduce((total, order) => total + (order.totalAmount ?? 0), 0),
        );
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const loadProductsCount = async () => {
    try {
      const response = await Client.productDetailsVW();
      if (response.data && response.isSuccess) {
        setTotalProducts(response.data.length);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  useEffect(() => {
    loadOrdersCount();
    loadProductsCount();
  }, []);

  useEffect(() => {
    if (!statsRef.current) return;
    const cards = statsRef.current.querySelectorAll(".stat-card");
    gsap.fromTo(
      cards,
      { opacity: 0, y: 18 },
      { opacity: 1, y: 0, duration: 0.45, stagger: 0.08, ease: "power2.out" },
    );
  }, []);

  return (
    <>
      <div className="stats-grid" ref={statsRef}>
        <div className="stat-card">
          <div className="stat-icon">💰</div>
          <div className="stat-info">
            <h3>Total Revenue</h3>
            <p className="stat-value">${shownRevenue}</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">💰</div>
          <div className="stat-info">
            <h3>Total Profit</h3>
            <p className="stat-value">${shownProfit}</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">📦</div>
          <div className="stat-info">
            <h3>Total Orders</h3>
            <p className="stat-value">{shownOrders}</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">🏷️</div>
          <div className="stat-info">
            <h3>Total Products</h3>
            <p className="stat-value">{shownProducts}</p>
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
