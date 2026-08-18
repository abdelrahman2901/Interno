import { useState } from "react";
import { useAuth } from "../../../../../Core/Services/AuthServices/AuthProvider";
import "../../Shared/CSS/ModelCSS.css";
import { checkPasswordDto } from "../../../../../Core/DTO/UserDTO/checkPasswordDto";
import {
  CheckPassword,
  DeleteAccount_serv,
} from "../../../../../Core/Services/AuthServices/AuthUserService";
import { useNavigate } from "react-router-dom";
type props = {
  onClose: () => void;
};
export default function DeleteAccountModel({ onClose }: props) {
  const { user, signout } = useAuth();
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const navigator = useNavigate();

  function PasswordInput(e: React.ChangeEvent<HTMLInputElement>) {
    e.preventDefault();
    const value = e.target.value;
    console.log(value);
    setPassword(value);
  }
  function DeleteAccount() {
    const checkPassRequest: checkPasswordDto = {
      userID: user?.userID!,
      password: password,
    };
    const deleteAcc = async () => {
      try {
        const response = await DeleteAccount_serv(user?.userID!);
        if (response.isSuccess) {
          signout();
          navigator("/Home");
        }
      } catch (err) {
        if (err) console.error(err);
      }
    };
    const checkPassAPI = async () => {
      try {
        const response = await CheckPassword(checkPassRequest);
        if (response.isSuccess) {
          deleteAcc();
        } else {
          setMessage("Password is InCorrect");
        }
      } catch (err) {
        if (err) console.error(err);
      }
    };
    checkPassAPI();
  }
  return (
    <>
      <div id="deleteAccountModal" className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-small">
          <div className="Custom-modal-header">
            <h3>Delete Account</h3>
            <button className="close-btn" onClick={onClose}>
              &times;
            </button>
          </div>
          <div className="Custom-modal-body">
            <p className="m-b-16">
              Are you sure you want to delete your account? This action cannot
              be undone.
            </p>
            <p className="c-r f-w-500">
              All your data including orders, addresses, and preferences will be
              permanently deleted.
            </p>
            <div className="form-group m-t-20">
              <label>Enter your password to confirm</label>
              <input
                type="password"
                onChange={PasswordInput}
                required
                placeholder="Password"
              />
              <span className="message error">{message}</span>
            </div>
          </div>
          <div className="form-actions">
            <button className="btn-secondary" onClick={onClose}>
              Cancel
            </button>
            <button className="btn-danger" onClick={DeleteAccount}>
              Delete My Account
            </button>
          </div>
        </div>
      </div>
    </>
  );
}
