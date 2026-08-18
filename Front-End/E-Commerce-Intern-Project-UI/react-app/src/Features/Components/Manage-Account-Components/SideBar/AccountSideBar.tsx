import { useEffect, useState } from "react";
import "./AccountSideBar.css";
import { useAuth } from "../../../../Core/Services/AuthServices/AuthProvider";
type props = {
  swtichSection_OutPut: (
    section:
      | "Orders"
      | "Profile-Settings"
      | "Security"
      | "OverView"
      | "Addresses",
  ) => void;
  swtichSection_InPut:
    | "Orders"
    | "Profile-Settings"
    | "Security"
    | "OverView"
    | "Addresses";
};
export default function AccountSideBar({
  swtichSection_OutPut,
  swtichSection_InPut,
}: props) {
  const [CurrentSection, SetCurrentSection] = useState<
    "Orders" | "Profile-Settings" | "Security" | "OverView" | "Addresses"
  >("OverView");
  const { user } = useAuth();

  useEffect(() => {
    SetCurrentSection(swtichSection_InPut);
  }, [swtichSection_InPut]);
  function switchSection(
    section:
      | "Orders"
      | "Profile-Settings"
      | "Security"
      | "OverView"
      | "Addresses",
  ) {
    SetCurrentSection(section);
    swtichSection_OutPut(section);
  }
  return (
    <>
      <aside className="Account-account-sidebar">
        <div className="Account-user-info">
          <div className="Account-user-avatar">
            <img
              src={`https://ui-avatars.com/api/?name=${user?.personName}&background=8b6f5e&color=fff&size=80`}
              alt="User Avatar"
            />
          </div>
          <h3>{user?.personName}</h3>
          <p>{user?.email}</p>
        </div>

        <nav className="Account-account-nav">
          {user?.role === "User" && (
            <>
              <a
                onClick={() => switchSection("OverView")}
                className={
                  "Account-nav-item " +
                  (CurrentSection === "OverView" ? "active" : "")
                }
                data-section="overview"
              >
                <span className="icon">📊</span>
                <span>Overview</span>
              </a>

              <a
                onClick={() => switchSection("Orders")}
                className={
                  "Account-nav-item " +
                  (CurrentSection === "Orders" ? "active" : "")
                }
                data-section="orders"
              >
                <span className="Account-icon">📦</span>
                <span>My Orders</span>
              </a>

              <a
                onClick={() => switchSection("Addresses")}
                className={
                  "Account-nav-item " +
                  (CurrentSection === "Addresses" ? "active" : "")
                }
                data-section="addresses"
              >
                <span className="Account-icon">📍</span>
                <span>Addresses</span>
              </a>
            </>
          )}
          <a
            onClick={() => switchSection("Profile-Settings")}
            className={
              "Account-nav-item " +
              (CurrentSection === "Profile-Settings" ? "active" : "")
            }
            data-section="profile"
          >
            <span className="Account-icon">👤</span>
            <span>Profile Settings</span>
          </a>
          <a
            onClick={() => switchSection("Security")}
            className={
              "Account-nav-item " +
              (CurrentSection === "Security" ? "active" : "")
            }
            data-section="security"
          >
            <span className="Account-icon">🔒</span>
            <span>Security</span>
          </a>
        </nav>
      </aside>
    </>
  );
}
