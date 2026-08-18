// import "./DashBoardSideBar.css";
import "../Shared/css/Buttons.css";
import { useState } from "react";
import { Link } from "react-router-dom";
import { SectionTypes } from "../../../../Core/Types/SectionTypes";
import { SubSectionTypes } from "../../../../Core/Types/SubSectionTypes";
type props = {
  onToggle: (value: SectionTypes) => void;
  onToggleSubSection: (value: SubSectionTypes) => void;
};

export default function DashSideBar({ onToggle, onToggleSubSection }: props) {
  const [currentSection, SetCurrentSection] =
    useState<SectionTypes>("OverView");

  const [currentSubSection, SetCurrentSubSection] =
    useState<SubSectionTypes>("Area");

  function toggle(section: SectionTypes) {
    onToggle(section);
    SetCurrentSection(section);
  }
  function toggleSubSection(section: SubSectionTypes) {
    onToggleSubSection(section);
    SetCurrentSubSection(section);
  }
  return (
    <>
      <aside className="sidebar">
        <div className="logo">
          <h1>Admin Panel</h1>
        </div>
        <nav className="sidebar-nav">
          <span
            onClick={() => toggle("OverView")}
            className={
              "nav-item " + (currentSection === "OverView" ? "active" : "")
            }
            data-section="overview"
          >
            <span className="icon">📊</span>
            <span>Overview</span>
          </span>
          <span
            onClick={() => toggle("CategorySection")}
            className={
              "nav-item " +
              (currentSection === "CategorySection" ? "active" : "")
            }
            data-section="categories"
          >
            <span className="icon">📁</span>
            <span>Categories</span>
          </span>
          <span
            onClick={() => toggle("ProductSection")}
            className={
              "nav-item " +
              (currentSection === "ProductSection" ? "active" : "")
            }
            data-section="products"
          >
            <span className="icon">🏷️</span>
            <span>Products</span>
          </span>
          <span
            onClick={() => toggle("OrderSection")}
            className={
              "nav-item " + (currentSection === "OrderSection" ? "active" : "")
            }
          >
            <span className="icon">📦</span>
            <span>Orders</span>
          </span>

          <span
            onClick={() => toggle("BannerSection")}
            className={
              "nav-item " + (currentSection === "BannerSection" ? "active" : "")
            }
          >
            <span className="icon">🏴</span>
            <span>Banner</span>
          </span>
          <span
            onClick={() => toggle("AuditSection")}
            className={
              "nav-item " + (currentSection === "AuditSection" ? "active" : "")
            }
          >
            <span className="icon">🔉</span>
            <span>Audit</span>
          </span>
          <span
            onClick={() => toggle("SettingsSection")}
            className={
              "nav-item " +
              (currentSection === "SettingsSection" ? "active" : "")
            }
          >
            <span className="icon">⚙️</span>
            <span>Settings</span>
          </span>
          {currentSection === "SettingsSection" && (
            <>
              <span
                onClick={() => toggleSubSection("Area")}
                className={
                  "nav-item " + (currentSubSection === "Area" ? "active" : "")
                }
              >
                <div className="sub">
                  <span className="icon">📍</span>
                  <span>Areas</span>
                </div>
              </span>
              <span
                onClick={() => toggleSubSection("City")}
                className={
                  "nav-item " + (currentSubSection === "City" ? "active" : "")
                }
              >
                <div className="sub">
                  <span className="icon">🏙️</span>
                  <span>Cities</span>
                </div>
              </span>

              <span
                onClick={() => toggleSubSection("Coupon")}
                className={
                  "nav-item " + (currentSubSection === "Coupon" ? "active" : "")
                }
              >
                <div className="sub">
                  <span className="icon">🎟️</span>
                  <span>Coupons</span>
                </div>
              </span>
            </>
          )}
        </nav>
        <div className="sidebar-footer">
          <Link to={"/Home"} className="btn-secondary">
            Go Back Home
          </Link>
        </div>
      </aside>
    </>
  );
}
