import { useEffect, useState } from "react";
import AccountSideBar from "../../Components/Manage-Account-Components/SideBar/AccountSideBar";
import "./Manage-Account.css";
import AccountSecurity from "../../Components/Manage-Account-Components/Security/security";
import AccountOverView from "../../Components/Manage-Account-Components/OverView/OverViewComponent";
import AccountProfileSettings from "../../Components/Manage-Account-Components/Profile-Settings/ProfileSettings";
import AccountOrders from "../../Components/Manage-Account-Components/Orders/Orders";
import AccountAddress from "../../Components/Manage-Account-Components/addresses/addresses";
import { OrderDetails } from "../../../Core/DTO/OrderDTO/OrderDetails";
import { useAuth } from "../../../Core/Services/AuthServices/AuthProvider";
import { GetAllUserOrders } from "../../../Core/Services/OrderServices/OrderService";
export default function ManageAccount() {
  const [CurrentSection, SetCurrentSection] = useState<
    "Orders" | "Profile-Settings" | "Security" | "OverView" | "Addresses"
  >("OverView");

  function onSwitchSectionEvent(
    value:
      | "Orders"
      | "Profile-Settings"
      | "Security"
      | "OverView"
      | "Addresses",
  ) {
    SetCurrentSection(value);
  }

  const [orders, setOrders] = useState<OrderDetails[]>([]);
  const { user } = useAuth();
  const loadOrders = async () => {
    try {
      const response = await GetAllUserOrders(user?.userID!);
      if (response.isSuccess && response.data) {
        setOrders(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (orders?.length === 0 && user) {
      loadOrders();
    }
  }, [user]);
  useEffect(() => {
    if (user?.role === "Admin") {
      SetCurrentSection("Profile-Settings");
    }
  }, []);

  function RenderedSection() {
    switch (CurrentSection) {
      case "OverView": {
        return (
          <AccountOverView
            orders={orders}
            swtichSection_OutPut={onSwitchSectionEvent}
          />
        );
      }
      case "Profile-Settings": {
        return <AccountProfileSettings />;
      }
      case "Orders": {
        return <AccountOrders orders_Prop={orders} />;
      }
      case "Security": {
        return <AccountSecurity />;
      }
      case "Addresses": {
        return <AccountAddress />;
      }
      default: {
        return (
          <AccountOverView
            orders={orders}
            swtichSection_OutPut={onSwitchSectionEvent}
          />
        );
      }
    }
  }

  return (
    <>
      <div className="account-container">
        <AccountSideBar
          swtichSection_InPut={CurrentSection}
          swtichSection_OutPut={onSwitchSectionEvent}
        />
        <main className="account-main">
          <section className="content-section">{RenderedSection()}</section>
        </main>
      </div>
    </>
  );
}
