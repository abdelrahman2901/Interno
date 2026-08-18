import React, { useEffect, useState } from "react";
import { OrderCouponModel } from "../../../../../../Core/DTO/OrdeCouponDTO/OrderCouponResponse";
import {
  CreateNewCoupon,
  GetCoupon,
  UpdateCoupon,
} from "../../../../../../Core/Services/OrderCouponServices/OrderCouponService";

type props = {
  onClose: () => void;
  CouponIDProps: string | null;
  onReloadCoupons: () => void;
};

export default function Add_UpdateCouponModel({
  onClose,
  CouponIDProps,
  onReloadCoupons,
}: props) {
  const [coupon, setCoupon] = useState<OrderCouponModel>(
    new OrderCouponModel(),
  );
  const loadCouponDetails = async () => {
    try {
      const response = await GetCoupon(CouponIDProps!);
      if (response.isSuccess) {
        setCoupon(response.data!);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    console.log(CouponIDProps);

    if (CouponIDProps) {
      loadCouponDetails();
    }
  }, [CouponIDProps]);

  async function onSaveCoupon(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    console.log(coupon);
    if (CouponIDProps) {
      //update
      try {
        const response = await UpdateCoupon(coupon);
        if (response.isSuccess) {
          onClose();
          onReloadCoupons();
        }
      } catch (err) {
        if (err) console.error(err);
      }
    } else {
      try {
        const response = await CreateNewCoupon({
          discount: coupon.discount,
          discountType: coupon.discountType,
          isActive: coupon.isActive,
          couponCode: coupon.couponCode,
        });
        if (response.isSuccess) {
          onClose();
          onReloadCoupons();
        }
      } catch (err) {
        if (err) console.error(err);
      }
    }
  }

  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content">
          <div className="Custom-modal-header">
            <h3>Add New Coupon</h3>
            <button className="modal-close" onClick={onClose}>
              &times;
            </button>
          </div>
          <form onSubmit={onSaveCoupon}>
            <div className="Custom-modal-body">
              <div className="Custom-form-group">
                <label>Coupon Code *</label>
                <input
                  type="text"
                  name="couponCode"
                  onChange={(e) => {
                    const couponCode = e.currentTarget.value;
                    setCoupon((prev) => ({ ...prev, couponCode: couponCode }));
                  }}
                  value={coupon.couponCode}
                  placeholder="e.g., WELCOME10, SAVE20"
                />
                <small className="Custom-form-hint">
                  Letters and numbers only, no spaces
                </small>
              </div>

              <div className="Custom-form-row">
                <div className="Custom-form-group">
                  <label>Discount Type *</label>
                  <select
                    name="discoundType"
                    onChange={(e) => {
                      const discountType = e.currentTarget.value;
                      setCoupon((prev) => ({
                        ...prev,
                        discountType: discountType,
                      }));
                    }}
                    value={coupon.discountType}
                  >
                    <option value="">Select Type</option>
                    <option value="Percentage">Percentage (%)</option>
                    <option value="Fixed">Fixed Amount ($)</option>
                  </select>
                </div>
                <div className="form-group">
                  <label>Discount Value *</label>
                  <input
                    type="number"
                    name="discound"
                    placeholder="10"
                    onChange={(e) => {
                      const discount = Number(e.currentTarget.value);
                      setCoupon((prev) => ({
                        ...prev,
                        discount: discount,
                      }));
                    }}
                    value={coupon.discount}
                  />
                </div>
              </div>

              <div className="Custom-form-group">
                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    name="isActive"
                    checked={coupon.isActive}
                    onChange={(e) => {
                      const isChecked = e.currentTarget.checked;
                      console.log(isChecked);

                      setCoupon((prev) => ({
                        ...prev,
                        isActive: isChecked,
                      }));
                      console.log(coupon);
                    }}
                  />
                  <span>Active (Available for customers)</span>
                </label>
              </div>

              <div className="coupon-preview">
                <div className="preview-label">Coupon Preview:</div>
                <div className="preview-code">WELCOME10</div>
                <div className="preview-discount">10% OFF</div>
              </div>
            </div>
            <div className="Custom-modal-footer">
              <button type="button" className="btn-secondary" onClick={onClose}>
                Cancel
              </button>
              <button type="submit" className="btn-primary">
                Save Coupon
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
