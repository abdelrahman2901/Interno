import { useEffect, useState } from "react";
import { OrderCouponModel } from "../../../../../Core/DTO/OrdeCouponDTO/OrderCouponResponse";
import {
  DeleteCoupon_serv,
  GetAllCoupons,
  UpdateCouponActivation,
} from "../../../../../Core/Services/OrderCouponServices/OrderCouponService";
import Add_UpdateCouponModel from "./Models/Add_UpdateCouponModel";
import DeleteCouponModel from "./Models/DeleteCouponModel";

export default function CouponsComponent() {
  const [_coupons, setPrivCoupons] = useState<OrderCouponModel[]>([]);
  const [coupons, setCoupons] = useState<OrderCouponModel[]>([]);
  const [currentTab, setCurrentTab] = useState<"all" | "active" | "inactive">(
    "all",
  );
  const [selectedCouponID, setSelectedCouponID] = useState<string | null>(null);
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [isDeletingCoupon, setIsDeletingCoupon] = useState<boolean>(false);
  const loadCoupons = async () => {
    try {
      const response = await GetAllCoupons();
      if (response.isSuccess) {
        setCoupons(response.data!);
        setPrivCoupons(response.data!);
      }
    } catch (err) {
      if (err) {
        console.error(err);
      }
    }
  };
  const DeleteCoupon = async () => {
    try {
      const response = await DeleteCoupon_serv(selectedCouponID!);
      if (response.isSuccess) {
        loadCoupons();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  useEffect(() => {
    if (coupons.length === 0) {
      loadCoupons();
    }
  }, []);

  function extractCouponID(CityID: string) {
    return `Coupon-${CityID.slice(0, 3)}`;
  }
  function toggleModal() {
    setIsOpenModal(!isOpenModal);
  }
  function onDeleteResultActionEvent(result: boolean) {
    if (result) {
      DeleteCoupon();
    }
    setIsDeletingCoupon(false);
  }

  function ReloadCoupons() {
    setSelectedCouponID(null);
    loadCoupons();
  }
  async function ToggleCouponActivation(CouponID: string) {
    console.log(CouponID);

    try {
      const response = await UpdateCouponActivation(CouponID);
      if (response.isSuccess) {
        loadCoupons();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  }
  function EditCoupon(couponID: string) {
    setSelectedCouponID(couponID);
    console.log(couponID);
    console.log(selectedCouponID);
    toggleModal();
  }
  function filter(status: "all" | "active" | "inactive") {
    setCurrentTab(status);

    console.log(currentTab);
    console.log(status);

    switch (status) {
      case "all": {
        setCoupons(_coupons);
        return;
      }
      case "active": {
        setCoupons(_coupons.slice().filter((r) => r.isActive));
        return;
      }
      case "inactive": {
        setCoupons(_coupons.slice().filter((r) => !r.isActive));
        return;
      }
    }
  }
  return (
    <>
      <main className="main-content">
        <header className="top-header">
          <h2>Coupons Management</h2>
          <button className="btn-primary" onClick={toggleModal}>
            + Add New Coupon
          </button>
        </header>

        <div className="content-section">
          <div className="filter-bar">
            <div className="Coupon-filter-tabs">
              <button
                className={`Coupon-filter-tab ${currentTab === "all" ? "active" : ""}`}
                onClick={() => {
                  // setCurrentTab("all");
                  filter("all");
                }}
              >
                All Coupons
              </button>
              <button
                className={`Coupon-filter-tab ${currentTab === "active" ? "active" : ""}`}
                onClick={() => {
                  // setCurrentTab("active");
                  filter("active");
                }}
              >
                Active
              </button>
              <button
                className={`Coupon-filter-tab ${currentTab === "inactive" ? "active" : ""}`}
                onClick={() => {
                  // setCurrentTab("inactive");
                  filter("inactive");
                }}
              >
                Inactive
              </button>
            </div>
          </div>

          <div className="table-container">
            <table className="data-table" id="couponsTable">
              <thead>
                <tr>
                  <th>Coupon ID</th>
                  <th>Coupon Code</th>
                  <th>Discount</th>
                  <th>Type</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {coupons.map((coupon) => (
                  <tr key={coupon.orderCouponID}>
                    <td>
                      <span className="id-badge">
                        {extractCouponID(coupon.orderCouponID)}
                      </span>
                    </td>
                    <td>
                      <span className="coupon-code">{coupon.couponCode}</span>
                    </td>
                    <td>
                      <span className="discount-badge">
                        {coupon.discount}
                        {coupon.discountType === "Percentage" ? "%" : "$"}
                      </span>
                    </td>
                    <td>
                      <span className="type-badge"> {coupon.discountType}</span>
                    </td>
                    <td>
                      <span
                        className={`status-badge ${coupon.isActive ? "active" : "inactive"}`}
                      >
                        {coupon.isActive ? "Active" : "inActive"}
                      </span>
                    </td>
                    <td>
                      <button
                        className="btn-icon btn-toggle"
                        title="Toggle Status"
                        onClick={() =>
                          ToggleCouponActivation(coupon.orderCouponID)
                        }
                      >
                        🔄
                      </button>
                      <button
                        className="btn-icon btn-edit"
                        title="Edit"
                        onClick={() => EditCoupon(coupon.orderCouponID)}
                      >
                        ✏️
                      </button>
                      <button
                        className="btn-icon btn-delete"
                        title="Delete"
                        onClick={() => {
                          setSelectedCouponID(coupon.orderCouponID);
                          setIsDeletingCoupon(true);
                        }}
                      >
                        🗑️
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            {coupons.length === 0 && (
              <div className="empty-state">
                <div className="empty-icon">🎟️</div>
                <h3>No Coupons Found</h3>
                <p>Create your first coupon to offer discounts</p>
                <button className="btn-primary" onClick={toggleModal}>
                  Add Coupon
                </button>
              </div>
            )}
          </div>
        </div>
      </main>

      {isOpenModal && (
        <Add_UpdateCouponModel
          onReloadCoupons={ReloadCoupons}
          CouponIDProps={selectedCouponID}
          onClose={toggleModal}
        />
      )}

      {isDeletingCoupon && (
        <DeleteCouponModel onResultAction={onDeleteResultActionEvent} />
      )}
    </>
  );
}
