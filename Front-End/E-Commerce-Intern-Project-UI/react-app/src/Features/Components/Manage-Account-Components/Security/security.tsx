import React, { useState } from "react";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import "./security.css";
import { changePasswordDto } from "../../../../Core/DTO/UserDTO/ChangePasswordDto";
import { ChangePassword } from "../../../../Core/Services/AuthServices/AuthUserService";
import DeleteAccountModel from "./Model/DeleteAccountModel";
export default function AccountSecurity() {
  const { user, refreshUser } = useAuth();
  const [message, setMessage] = useState<{
    message: string;
    isSuccess: boolean;
  }>();
  const [isDeletingAcc, SetisDeletingAcc] = useState(false);
  function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);
    const request: changePasswordDto = {
      userID: user?.userID!,
      newPassword: form.get("newPassword") as string,
      confirmPassword: form.get("confirmPassword") as string,
      currentPassword: form.get("currentPassword") as string,
    };
    console.log(request);
    const changePassword = async () => {
      try {
        const response = await ChangePassword(request);
        if (response.isSuccess) {
          setMessage({
            message: "Password Updated Successfuly",
            isSuccess: true,
          });
          refreshUser();
        } else {
          setMessage({ message: response.errorMessage!, isSuccess: false });
        }
      } catch (err) {
        if (err) console.error(err);
      }
    };
    changePassword();
  }
  function OnCloseModelEvent() {
    SetisDeletingAcc(false);
  }
  return (
    <>
      <h2 className="section-title">Security Settings</h2>

      <div className="Account-security-section">
        <h3>Change Password</h3>
        <form className="account-form" onSubmit={onSubmit}>
          <div className="form-group">
            <label>Current Password</label>
            <input
              type="password"
              name="currentPassword"
              required
              placeholder="Enter current password"
            />
          </div>

          <div className="form-group">
            <label>New Password</label>
            <input
              type="password"
              name="newPassword"
              required
              placeholder="Enter new password"
            />
            <small className="form-hint">
              Must be at least 8 characters long
            </small>
          </div>

          <div className="form-group">
            <label>Confirm New Password</label>
            <input
              type="password"
              name="confirmPassword"
              required
              placeholder="Re-enter new password"
            />
          </div>

          <div className="form-actions">
            <button type="submit" className="btn-primary">
              Update Password
            </button>
          </div>
          <span
            className={"message " + (message?.isSuccess ? "success" : "error")}
          >
            {message?.message}
          </span>
        </form>
      </div>
      {user?.role === "User" && (
        <div className="Account-security-section Account-danger-zone">
          <h3>Danger Zone</h3>
          <div className="Account-danger-card">
            <div>
              <h4>Delete Account</h4>
              <p>
                Once you delete your account, there is no going back. Please be
                certain.
              </p>
            </div>
            <button
              className="btn-danger"
              onClick={() => {
                SetisDeletingAcc(true);
              }}
            >
              Delete Account
            </button>
          </div>
        </div>
      )}

      {isDeletingAcc && <DeleteAccountModel onClose={OnCloseModelEvent} />}
    </>
  );
}
