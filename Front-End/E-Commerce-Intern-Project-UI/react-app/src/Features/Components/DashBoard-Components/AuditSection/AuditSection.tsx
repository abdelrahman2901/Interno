import React, { useEffect, useState } from "react";
import { AuditModel } from "../../../../Core/DTO/AuditDTO/AuditModel";
import { GetAllAudit } from "../../../../Core/Services/AuditServices/AuditService";
import { actionTypes } from "./Arrays/ActionTypesArr";
import { entityNames } from "./Arrays/EntityNamesArr";

export default function AuditSection() {
  const [_audits, setPrivAudits] = useState<AuditModel[]>([]);
  const [audits, setAudits] = useState<AuditModel[]>([]);
  const [fromDate, setFromDate] = useState<string>("");
  const [toDate, setToDate] = useState<string>("");
  const [currentActionType, setCurrentActionType] = useState<string>("All");
  const [currentEntityName, setCurrentEntityName] = useState<string>("All");
  useEffect(() => {
    loadAudits();
  }, []);

  const loadAudits = async () => {
    try {
      const response = await GetAllAudit();
      if (response.isSuccess && response.data) {
        setAudits(response.data);
        setPrivAudits(response.data);
        // setEntityNames([...new Set(response.data.map((r) => r.entityName))]);
        // setActionTypes([...new Set(response.data.map((r) => r.actionType))]);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

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

  function filterBasedOnActionType(e: React.ChangeEvent<HTMLSelectElement>) {
    const value = e.currentTarget.value;
    setCurrentActionType(value);
    if (value === "All") {
      setAudits(_audits);
      return;
    }
    setAudits(_audits.slice().filter((r) => r.actionType === value));
  }
  function filterBasedOnEntityName(e: React.ChangeEvent<HTMLSelectElement>) {
    const value = e.currentTarget.value;
    setCurrentEntityName(value.toLowerCase());
    if (value === "All") {
      setAudits(_audits);
      return;
    }
    setAudits(
      _audits
        .slice()
        .filter((r) => r.entityName.toLowerCase() === value.toLowerCase()),
    );
  }
  function filterBasedOnDate(e: React.ChangeEvent<HTMLInputElement>) {
    const value = e.currentTarget.value;
    setToDate(value);
    console.log(value);
    console.log(fromDate);
    const fromDateTime = new Date(fromDate).getTime();
    const toDateTime = new Date(value).getTime();
    console.log(fromDateTime);
    console.log(toDateTime);

    setToDate(value);
    if (value === "All") {
      setAudits(_audits);
      return;
    }
    setAudits(
      _audits
        .slice()
        .filter(
          (r) =>
            new Date(r.createdAt).getTime() >= fromDateTime &&
            new Date(r.createdAt).getTime() <= toDateTime,
        ),
    );
  }
  function ClearFilters() {
    setFromDate("");
    setToDate("");
    setCurrentActionType("All");
    setCurrentEntityName("All");
    setAudits(_audits);
  }
  return (
    <>
      <main className="main-content">
        <header className="top-header">
          <h2>Audit Logs</h2>
          <div className="header-actions">
            <button className="btn-secondary">📥 Export Logs</button>
            <button className="btn-primary">🔄 Refresh</button>
          </div>
        </header>

        <div className="content-section">
          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-icon">📋</div>
              <div className="stat-info">
                <h4>Total Actions</h4>
                <p className="stat-value">{_audits.length}</p>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon">✏️</div>
              <div className="stat-info">
                <h4>Updates Today</h4>
                <p className="stat-value">
                  {
                    _audits.slice().filter((r) => r.actionType === "Update")
                      .length
                  }
                </p>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon">🗑️</div>
              <div className="stat-info">
                <h4>Deletions Today</h4>
                <p className="stat-value">
                  {
                    _audits.slice().filter((r) => r.actionType === "Delete")
                      .length
                  }
                </p>
                <span className="stat-change">Requires attention</span>
              </div>
            </div>
          </div>

          <div className="filter-bar">
            <div className="filter-row">
              <div className="filter-group">
                <label>Action Type:</label>
                <select
                  value={currentActionType}
                  className="filter-select"
                  onChange={(e) => filterBasedOnActionType(e)}
                >
                  <option value="All">All Actions</option>
                  {actionTypes.map((action) => (
                    <option value={action}>{action}</option>
                  ))}
                </select>
              </div>
              <div className="filter-group">
                <label>Entity Type:</label>
                <select
                  value={currentEntityName}
                  className="filter-select"
                  onChange={(e) => filterBasedOnEntityName(e)}
                >
                  <option value="All">All Entities</option>
                  {entityNames.map((entity) => (
                    <option value={entity}>{entity}</option>
                  ))}
                </select>
              </div>
              <div className="filter-group">
                <label>User:</label>
                <select className="filter-select" id="userFilter">
                  <option value="">All Users</option>
                  <option value="admin@gmail.com">Admin User</option>
                </select>
              </div>
              <div className="filter-group">
                <label>Date Range:</label>
                <input
                  type="date"
                  className="date-filter"
                  onChange={(e) => {
                    const value = e.currentTarget.value;
                    setFromDate(value);
                  }}
                  value={fromDate}
                />
              </div>
              <div className="filter-group">
                <span className="date-separator">to</span>
                <input
                  type="date"
                  className="date-filter"
                  onChange={(e) => filterBasedOnDate(e)}
                  value={toDate}
                />
              </div>
              <div className="filter-group">
                <button className="btn-secondary" onClick={ClearFilters}>
                  Clear
                </button>
              </div>
            </div>
          </div>

          <div className="table-container">
            <table className="audit-table" id="auditTable">
              <thead>
                <tr>
                  <th>Timestamp</th>
                  <th>User</th>
                  <th>Action</th>
                  <th>Entity Type</th>
                  <th>IP Address</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {audits.map((audit) => (
                  <tr>
                    <td>
                      <div className="timestamp">
                        <strong>{extractDate(audit.createdAt)}</strong>
                        <span>{extractDateTime(audit.createdAt)}</span>
                      </div>
                    </td>
                    <td>
                      <div className="user-info">
                        U{audit.createdAt.slice(0, 3)}
                      </div>
                    </td>
                    <td>
                      <span className="action-badge update">
                        {audit.actionType}
                      </span>
                    </td>
                    <td>
                      <span className="entity-badge">{audit.entityName}</span>
                    </td>

                    <td>
                      <code className="ip-address">192.168.1.100</code>
                    </td>
                    <td>
                      <button
                        className="btn-icon btn-view"
                        title="View Full Details"
                      >
                        👁️
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* <div className="pagination">
            <button className="page-btn" disabled>
              ← Previous
            </button>
            <div className="page-numbers">
              <button className="page-num active">1</button>
              <button className="page-num">2</button>
              <button className="page-num">3</button>
              <span className="page-dots">...</span>
              <button className="page-num">50</button>
            </div>
            <button className="page-btn">Next →</button>
          </div> */}
        </div>
      </main>
    </>
  );
}
