import { useState } from "react";
import { useAuth } from "../../../../../Core/Services/AuthServices/AuthProvider";
import "../../Shared/CSS/ModelCSS.css";
import { InternoMicroserviceClient, LoginUserCommand } from "../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../../Core/Shared/API-URL";
import { useNavigate } from "react-router-dom";
import { getApiErrorMessage } from "../../../../../Core/Shared/getApiErrorMessage";
type props = {
  onClose: () => void;
};
export default function DeleteAccountModel({ onClose }: props) {
  const { user, signout } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
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
    const deleteAcc = async () => {
      try {
        const response = await Client.deleteUser();
        if (response.isSuccess) {
          signout();
          navigator("/Home");
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    };
    const checkPassAPI = async () => {
      try {
        const response = await Client.loginUser(
          new LoginUserCommand({
            email: user?.email,
            password: password,
          }),
        );
        if (response.isSuccess) {
          deleteAcc();
        } else {
          setMessage("Password is InCorrect");
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
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
