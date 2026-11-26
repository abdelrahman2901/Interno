import { useEffect, useState } from "react";
import CategoryDashBoardSection from "../../Components/DashBoard-Components/CategoryDashBoardSection/CategoryDashBoardSection";
import DashSideBar from "../../Components/DashBoard-Components/DashBoard-SideBar/DashBoardSideBar";
import OrderSection from "../../Components/DashBoard-Components/Order-Section/OrderSection";
import OverViewSection from "../../Components/DashBoard-Components/OverView-Section/OverViewSection";
import ProductDashBoardSection from "../../Components/DashBoard-Components/ProductDashBoardSection/ProductDashBoardSection";
import { InternoMicroserviceClient, ProductDetails_vw } from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config } from "../../../Core/Shared/API-URL";
import SettingsSection from "../../Components/DashBoard-Components/Settings-Section/Settings-Section";
import BannerSection from "../../Components/DashBoard-Components/BannerSection/BannerSection";
import AuditSection from "../../Components/DashBoard-Components/AuditSection/AuditSection";
import { SectionTypes } from "../../../Core/Types/SectionTypes";
import { SubSectionTypes } from "../../../Core/Types/SubSectionTypes";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";
import "./AdminDashBoard.css";

export default function Admin_Dashboard() {
  const [CurrentSection, setCurrentSection] = useState<SectionTypes>();
  const [CurrentSubSection, setCurrentSubSection] = useState<SubSectionTypes>();
  const [Products, SetProducts] = useState<ProductDetails_vw[]>([]);
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const loadProducts = async () => {
    try {
      const response = await Client.productDetailsVW();
      console.log(response.data);
      if (response.isSuccess) {
        SetProducts(response.data!);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  useEffect(() => {
    if (Products.length === 0) {
      loadProducts();
    }
  }, []);
  function onReLoadProductsEvent() {
    loadProducts();
  }
  const renderedSection = () => {
    switch (CurrentSection) {
      case "OverView":
        return <OverViewSection />;
      case "CategorySection":
        return <CategoryDashBoardSection />;
      case "ProductSection":
        return (
          <ProductDashBoardSection
            Products={Products}
            reLoadProduct={onReLoadProductsEvent}
          />
        );
      case "SettingsSection":
        return <SettingsSection section={CurrentSubSection!} />;
      case "OrderSection":
        return <OrderSection />;
      case "AuditSection":
        return <AuditSection />;
      case "BannerSection":
        return <BannerSection />;
      default:
        return <OverViewSection />;
    }
  };
  return (
    <>
      <div className="Custom-dashboard-container">
        <DashSideBar
          onToggle={setCurrentSection}
          onToggleSubSection={setCurrentSubSection}
        />
        <main className="main-content">
          <header className="top-header">
            <h2>Dashboard Overview</h2>
          </header>
          <section className="content-section">{renderedSection()}</section>
        </main>
      </div>
    </>
  );
}
