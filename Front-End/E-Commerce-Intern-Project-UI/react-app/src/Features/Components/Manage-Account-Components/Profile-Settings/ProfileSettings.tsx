import { useState } from "react";
import { InternoMicroserviceClient, UpdateUserCommand } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../../Core/Shared/API-URL";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";
import "./ProfileSettings.css";
export default function AccountProfileSettings() {
  const { user, refreshUser } = useAuth();
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [message, setMessage] = useState<{
    message: string;
    isSuccess: boolean;
  }>();

  function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    const form = new FormData(e.currentTarget);

    console.log(form.get("userName"));
    console.log(form.get("email"));
    console.log(form.get("phoneNumber"));
    const updateRequest = new UpdateUserCommand({
      personName: form.get("userName") as string,
      email: form.get("email") as string,
      phoneNumber: form.get("phoneNumber") as string,
    });

    const update = async () => {
      try {
        const response = await Client.updateUser(updateRequest);
        if (response.isSuccess) {
          refreshUser();
          setMessage({ message: "Update Done Successfuly", isSuccess: true });
        }
        if (!response.isSuccess) {
          setMessage({ message: "SomeThing Went Wrong", isSuccess: false });
        }
      } catch (err) {
        console.error(getApiErrorMessage(err));
      }
    };

    update();
  }
  return (
    <>
      <h2 className="section-title">Profile Settings</h2>

      <form className="account-form" onSubmit={onSubmit}>
        <div className="form-row">
          <div className="form-group">
            <label>Full Name</label>
            <input
              type="text"
              name="userName"
              required
              defaultValue={user?.personName}
            />
          </div>
        </div>

        <div className="form-group">
          <label>Email Address</label>
          <input
            type="email"
            name="email"
            required
            defaultValue={user?.email}
          />
        </div>

        <div className="form-group">
          <label>Phone Number</label>
          <input
            type="tel"
            name="phoneNumber"
            placeholder="+1 (555) 000-0000"
            defaultValue={user?.phoneNumber}
          />
        </div>

        {/* <div className="form-row">
          <div className="form-group">
            <label>Date of Birth</label>
            <input type="date" id="birthdate" value="1990-01-01" />
          </div>
          <div className="form-group">
            <label>Gender</label>
            <select id="gender">
              <option value="">Prefer not to say</option>
              <option value="male">Male</option>
              <option value="female">Female</option>
              <option value="other">Other</option>
            </select>
          </div>
        </div> */}

        <div className="form-actions">
          <button type="button" className="btn-secondary">
            Cancel
          </button>
          <button type="submit" className="btn-primary">
            Save Changes
          </button>
        </div>
        <span
          className={"message " + (message?.isSuccess ? "success" : "error")}
        >
          {message?.message}
        </span>
      </form>
    </>
  );
}
