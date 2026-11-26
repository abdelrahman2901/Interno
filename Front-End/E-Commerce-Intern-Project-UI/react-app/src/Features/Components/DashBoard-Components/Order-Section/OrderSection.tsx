import { useEffect, useState } from "react";
import { InternoMicroserviceClient, OrderDetails, OrderStatus, UpdateOrderStatusRequest } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../Core/Shared/API-URL";
import "./OrderSection.css";
import OrderDetailsModal from "./Modals/OrderDetails";
import { filterType } from "../../../../Core/Types/filterType";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";
const PAGE_SIZE = 10;
const PAGE_WINDOW = 5;

export default function OrderSection() {
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [_orders, setPrivOrders] = useState<OrderDetails[]>([]);
  const [orders, setOrders] = useState<OrderDetails[]>([]);
  const [selectedOrderID, setSelectedOrderID] = useState<string | null>(null);
  const [currentFilter, setCurrentFilter] = useState<filterType>("All");
  const [isShowingDetails, setIsShowingDetails] = useState<boolean>(false);
  const [selectedOrderIDs, setSelectedOrderIDs] = useState<string[]>([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);

  const laodOrders = async (page = pageNumber) => {
    try {
      const response = await Client.getAllOrders(page, PAGE_SIZE);
      if (response.isSuccess && response.data) {
        const paged = response.data;
        const items = paged.items ?? [];
        setPrivOrders(items);
        setOrders(
          currentFilter === "All"
            ? items
            : items.filter((r) => r.orderStatus === currentFilter),
        );
        setTotalCount(paged.totalCount ?? items.length);
        setTotalPages(paged.totalPages || 1);
        setHasNextPage(!!paged.hasNextPage);
        setHasPreviousPage(!!paged.hasPreviousPage);
        setPageNumber(paged.currentPage || page);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };

  useEffect(() => {
    laodOrders(1);
  }, []);
  function extractDate(orderdate?: Date | string) {
    const date = new Date(orderdate ?? Date.now());
    return `${date.toLocaleString("Default", { month: "short" })}  ${date.getDate()},${date.getFullYear()}`;
  }
  function extractDateTime(orderdate?: Date | string) {
    const date = new Date(orderdate ?? Date.now());
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
    setCurrentFilter(filterName);
    setOrders(
      filterName === "All"
        ? _orders
        : _orders.filter((r) => r.orderStatus === filterName),
    );
  }

  const windowStart =
    Math.floor((pageNumber - 1) / PAGE_WINDOW) * PAGE_WINDOW + 1;
  const windowEnd = Math.min(windowStart + PAGE_WINDOW - 1, totalPages);
  const visiblePages = Array.from(
    { length: Math.max(0, windowEnd - windowStart + 1) },
    (_, i) => windowStart + i,
  );
  const hasPrevWindow = windowStart > 1;
  const hasNextWindow = windowEnd < totalPages;

  function goToPage(page: number) {
    const next = Math.min(totalPages, Math.max(1, page));
    setPageNumber(next);
    laodOrders(next);
  }

  async function markAction(MarkAs: filterType) {
    try {
      const response = await Client.updateOrderStatus(
        new UpdateOrderStatusRequest({
          orderID: selectedOrderID!,
          newStatus: MarkAs as OrderStatus,
        }),
      );
      if (response.isSuccess) {
        laodOrders();
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }
  return (
    <>
      <main className="main-content">
        <header className="top-header">
          <h2>Orders Management</h2>
          <div className="header-actions">
            <button className="btn-secondary">📥 Export Orders</button>
            <button
              className="btn-primary"
              type="button"
              onClick={() => laodOrders(pageNumber)}
            >
              🔄 Refresh
            </button>
          </div>
        </header>

        <div className="content-section">
          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-icon">📦</div>
              <div className="stat-info">
                <h4>Total Orders</h4>
                <p className="stat-value">{totalCount}</p>
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
                    .reduce((total, order) => total + (order.totalAmount ?? 0), 0)}
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
                    .reduce((total, order) => total + (order.totalAmount ?? 0), 0)}
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
                All Orders ({totalCount})
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
                          IDs.push(order.orderID!);
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
                        {order.orderItems?.length ?? 0} items
                      </span>
                    </td>
                    <td>
                      <strong className="order-total">
                        ${order.totalAmount?.toFixed(2)}
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
                        className={`status-select ${order.orderStatus?.toLowerCase()}`}
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
                            ShowOrderDetails(order.orderID!);
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

          {totalPages > 1 && (
            <div className="pagination">
              <button
                type="button"
                className="page-btn"
                disabled={!hasPreviousPage || pageNumber <= 1}
                onClick={() => goToPage(pageNumber - 1)}
              >
                Previous
              </button>
              <div className="page-numbers">
                {hasPrevWindow && (
                  <button
                    type="button"
                    className="page-dots"
                    onClick={() => goToPage(windowStart - 1)}
                  >
                    ...
                  </button>
                )}
                {visiblePages.map((page) => (
                  <button
                    type="button"
                    key={page}
                    className={`page-num ${page === pageNumber ? "active" : ""}`}
                    onClick={() => goToPage(page)}
                  >
                    {page}
                  </button>
                ))}
                {hasNextWindow && (
                  <button
                    type="button"
                    className="page-dots"
                    onClick={() => goToPage(windowEnd + 1)}
                  >
                    ...
                  </button>
                )}
              </div>
              <button
                type="button"
                className="page-btn"
                disabled={!hasNextPage || pageNumber >= totalPages}
                onClick={() => goToPage(pageNumber + 1)}
              >
                Next
              </button>
            </div>
          )}

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
