import { useEffect, useState } from "react";
import { OrderDetails } from "../../../../Core/DTO/OrderDTO/OrderDetails";
import "./OrderSection.css";
import {
  GetAllOrders,
  UpdateOrderStatus,
} from "../../../../Core/Services/OrderServices/OrderService";
import OrderDetailsModal from "./Modals/OrderDetails";
import { filterType } from "../../../../Core/Types/filterType";

export default function OrderSection() {
  const [_orders, setPrivOrders] = useState<OrderDetails[]>([]);
  const [orders, setOrders] = useState<OrderDetails[]>([]);
  const [selectedOrderID, setSelectedOrderID] = useState<string | null>(null);
  const [currentFilter, setCurrentFilter] = useState<filterType>("All");
  const [isShowingDetails, setIsShowingDetails] = useState<boolean>(false);
  const [selectedOrderIDs, setSelectedOrderIDs] = useState<string[]>([]);
  // const [selectedOrderIDs, setSelectedOrderID] = useState<string>('');

  const laodOrders = async () => {
    try {
      const response = await GetAllOrders();
      if (response.isSuccess) {
        setPrivOrders(response.data!);
        setOrders(response.data!);
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };

  useEffect(() => {
    if (_orders.length === 0 || orders.length === 0) {
      laodOrders();
    }
  }, []);
  function extractDate(orderdate: string) {
    const date = new Date(orderdate);
    return `${date.toLocaleString("Default", { month: "short" })}  ${date.getDate()},${date.getFullYear()}`;
  }
  function extractDateTime(orderdate: string) {
    const date = new Date(orderdate);
    return date.toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
      hour12: true,
    });
  }
  function ShowOrderDetails(orderID: string) {
    setSelectedOrderID(orderID);
    setIsShowingDetails(true);
  }
  function CloseModalEvent() {
    setSelectedOrderID(null);
    setIsShowingDetails(false);
  }
  function filter(filterName: filterType) {
    if (filterName === "All") {
      setOrders(_orders);

      return;
    }
    setCurrentFilter(filterName);
    setOrders(_orders.slice().filter((r) => r.orderStatus === filterName));
  }

  async function markAction(MarkAs: filterType) {
    try {
      const response = await UpdateOrderStatus({
        OrderID: selectedOrderID!,
        NewStatus: MarkAs,
      });
      if (response.isSuccess) {
        laodOrders();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  return (
    <>
      <main className="main-content">
        <header className="top-header">
          <h2>Orders Management</h2>
          <div className="header-actions">
            <button className="btn-secondary">📥 Export Orders</button>
            <button className="btn-primary">🔄 Refresh</button>
          </div>
        </header>

        <div className="content-section">
          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-icon">📦</div>
              <div className="stat-info">
                <h4>Total Orders</h4>
                <p className="stat-value">{_orders.length}</p>
                <span className="stat-change positive">
                  +N% from last month
                </span>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon">⏳</div>
              <div className="stat-info">
                <h4>Proccessing Orders</h4>
                <p className="stat-value">
                  {
                    _orders
                      .slice()
                      .filter((r) => r.orderStatus === "Processing").length
                  }
                </p>
                <span className="stat-change">Needs attention</span>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon">💰</div>
              <div className="stat-info">
                <h4>Total Revenue</h4>
                <p className="stat-value">
                  $
                  {_orders
                    .filter((r) => r.orderStatus !== "Cancelled")
                    .reduce((total, order) => total + order.totalAmount, 0)}
                </p>
                <span className="stat-change positive">+N from yesterday</span>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon">💰</div>
              <div className="stat-info">
                <h4>Total Profit</h4>
                <p className="stat-value">
                  $
                  {_orders
                    .slice()
                    .filter((r) => r.orderStatus === "Delivered")
                    .reduce((total, order) => total + order.totalAmount, 0)}
                </p>
                <span className="stat-change positive">+N% from yesterday</span>
              </div>
            </div>
          </div>

          <div className="filter-bar">
            <div className="Order-filter-tabs">
              <button
                className={`Order-filter-tab ${currentFilter === "All" ? "active" : ""}`}
                onClick={() => filter("All")}
              >
                All Orders ({_orders.length})
              </button>

              <button
                onClick={() => filter("Processing")}
                className={`Order-filter-tab ${currentFilter === "Processing" ? "active" : ""}`}
              >
                Processing (
                {
                  _orders.slice().filter((r) => r.orderStatus === "Processing")
                    .length
                }
                )
              </button>
              <button
                onClick={() => filter("Shipped")}
                className={`Order-filter-tab ${currentFilter === "Shipped" ? "active" : ""}`}
              >
                Shipped (
                {
                  _orders.slice().filter((r) => r.orderStatus === "Shipped")
                    .length
                }
                )
              </button>
              <button
                onClick={() => filter("Delivered")}
                className={`Order-filter-tab ${currentFilter === "Delivered" ? "active" : ""}`}
              >
                Delivered (
                {
                  _orders.slice().filter((r) => r.orderStatus === "Delivered")
                    .length
                }
                )
              </button>
              <button
                onClick={() => filter("Cancelled")}
                className={`Order-filter-tab ${currentFilter === "Cancelled" ? "active" : ""}`}
              >
                Cancelled (
                {
                  _orders.slice().filter((r) => r.orderStatus === "Cancelled")
                    .length
                }
                )
              </button>
            </div>
          </div>

          <div className="table-container">
            <table className="orders-table">
              <thead>
                <tr>
                  <th>
                    <input
                      type="checkbox"
                      value={"All"}
                      name="All"
                      onClick={(e) => {
                        console.log("fewe");

                        const checked = e.currentTarget.checked;
                        const IDs: string[] = [];
                        for (let order of orders) {
                          console.log(order.orderID);
                          IDs.push(order.orderID);
                        }
                        if (checked) {
                          setSelectedOrderIDs(IDs);
                        } else {
                          setSelectedOrderIDs(
                            selectedOrderIDs.filter((r) => r === ""),
                          );
                        }
                      }}
                    />
                  </th>
                  <th>Order Number</th>
                  <th>Customer</th>
                  <th>Date</th>
                  <th>Items</th>
                  <th>Total</th>
                  <th>Payment</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.orderID}>
                    <td>
                      <input
                        type="checkbox"
                        value={order.orderID}
                        name={`${order.orderNumber}`}
                        onClick={(e) => {
                          const id = e.currentTarget.value;
                          const checked = e.currentTarget.checked;
                          setSelectedOrderID(id);
                          checked
                            ? setSelectedOrderIDs([...selectedOrderIDs, id])
                            : setSelectedOrderIDs(
                                selectedOrderIDs.filter((r) => r !== id),
                              );
                        }}
                        checked={
                          selectedOrderIDs.find((r) => r === order.orderID)
                            ? true
                            : false
                        }
                        className="order-checkbox"
                      />
                    </td>
                    <td>
                      <div className="order-number">
                        <strong>#{order.orderNumber}</strong>
                      </div>
                    </td>
                    <td>
                      <div className="customer-info">
                        <strong>{order.userName}</strong>
                        <span>{order.userEmail}</span>
                      </div>
                    </td>
                    <td>
                      <div className="order-date">
                        <strong>{extractDate(order.orderDate)}</strong>
                        <span>{extractDateTime(order.orderDate)}</span>
                      </div>
                    </td>
                    <td>
                      <span className="items-count">
                        {order.orderItems.length} items
                      </span>
                    </td>
                    <td>
                      <strong className="order-total">
                        ${order.totalAmount.toFixed(2)}
                      </strong>
                    </td>
                    <td>
                      <span
                        className={`payment-badge card`}
                        // className={`payment-badge cash`}
                      >
                        {order.paymentMethod}
                      </span>
                    </td>
                    <td>
                      <span
                        className={`status-select ${order.orderStatus.toLowerCase()}`}
                      >
                        {order.orderStatus}
                      </span>
                    </td>
                    <td>
                      <div className="action-buttons">
                        <button
                          className="btn-icon btn-view"
                          title="View Details"
                          onClick={() => {
                            ShowOrderDetails(order.orderID);
                          }}
                        >
                          👁️
                        </button>
                        <button
                          className="btn-icon btn-print"
                          title="Print Invoice"
                        >
                          🖨️
                        </button>
                        <button className="btn-icon btn-delete" title="Delete">
                          🗑️
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="bulk-actions">
            <div className="bulk-info">
              <span>{selectedOrderIDs.length}</span> orders selected
            </div>
            <div className="bulk-buttons">
              <button
                className="btn-bulk"
                onClick={() => markAction("Processing")}
              >
                Mark as Processing
              </button>
              <button
                className="btn-bulk"
                onClick={() => markAction("Shipped")}
              >
                Mark as Shipped
              </button>
              <button
                className="btn-bulk"
                onClick={() => markAction("Delivered")}
              >
                Mark as Delivered
              </button>
              <button
                className="btn-bulk btn-bulk-cancel"
                onClick={() => markAction("Cancelled")}
              >
                Cancel Orders
              </button>
            </div>
          </div>
        </div>
      </main>

      {isShowingDetails && (
        <OrderDetailsModal
          orderIDProps={selectedOrderID}
          onClose={CloseModalEvent}
        />
      )}
    </>
  );
}
