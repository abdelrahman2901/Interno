import { useEffect, useState } from "react";
import { BannerSlideResponse, InternoMicroserviceClient } from "../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config, getBannerImageUrl } from "../../../../Core/Shared/API-URL";
import BannerDeleteModel from "./Models/BannerDeleteModel";
import BannerAdd_UpdateModel from "./Models/BannerAdd_UpdateModel";
import { getApiErrorMessage } from "../../../../Core/Shared/getApiErrorMessage";
import "./BannerSection.css";

type filterTypes = "all" | "active" | "inActive";
export default function BannerSection() {
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const [_banners, setPrivBanners] = useState<BannerSlideResponse[]>([]);
  const [banners, setBanners] = useState<BannerSlideResponse[]>([]);
  const [selectedBannerID, setSelectedBannerID] = useState<string | null>(null);
  const [isOpenModel, setIsOpenModel] = useState<boolean>(false);
  const [isDeletingBanner, setIsDeletingBanner] = useState<boolean>(false);
  const [currentTab, setCurrentTab] = useState<filterTypes>("all");

  const loadBanners = async () => {
    try {
      const response = await Client.getAllBanners();
      if (response.isSuccess && response.data) {
        setBanners(response.data);
        setPrivBanners(response.data);
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  const deleteBanner = async () => {
    try {
      const response = await Client.deleteBanner(selectedBannerID!);
      if (response.data && response.isSuccess) {
        loadBanners();
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  };
  useEffect(() => {
    if (banners.length === 0) {
      loadBanners();
    }
  }, []);

  function onDeleteEvent(result: Boolean) {
    if (result) {
      deleteBanner();
    }
    setIsDeletingBanner(false);
  }
  function onReloadBannerEvent() {
    loadBanners();
    toggleModel();
  }
  function toggleModel() {
    setIsOpenModel(!isOpenModel);
  }

  function filter(tab: filterTypes) {
    setCurrentTab(tab);
    if (tab === "all") {
      setBanners(_banners);
      return;
    }
    setBanners(
      _banners.slice().filter((r) => r.isActive === (tab === "active")),
    );
  }
  function popUpModal(BannerID: string | null) {
    setSelectedBannerID(BannerID);
    setIsOpenModel(true);
  }
  async function toggleBannerStatus(bannerID: string) {
    try {
      const response = await Client.toggleBannerActiviation(bannerID);
      if (response.isSuccess) {
        loadBanners();
      }
    } catch (err) {
      console.error(getApiErrorMessage(err));
    }
  }
  return (
    <>
      <main className="main-content">
        <header className="top-header">
          <h2>Banners Management</h2>
          <button className="btn-primary" onClick={() => popUpModal(null)}>
            + Add New Banner
          </button>
        </header>

        <div className="content-section">
          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-icon">🎨</div>
              <div className="stat-info">
                <h4>Total Banners</h4>
                <p className="stat-value">{_banners.length}</p>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon">✅</div>
              <div className="stat-info">
                <h4>Active Banners</h4>
                <p className="stat-value">
                  {_banners.slice().filter((r) => r.isActive).length}
                </p>
              </div>
            </div>
            <div className="stat-card">
              <div className="stat-icon">⏸️</div>
              <div className="stat-info">
                <h4>Inactive Banners</h4>
                <p className="stat-value">
                  {_banners.slice().filter((r) => !r.isActive).length}
                </p>
              </div>
            </div>
          </div>

          <div className="filter-bar">
            <div className="banner-filter-tabs">
              <button
                className={`banner-filter-tab ${currentTab === "all" ? "active" : ""}`}
                onClick={() => filter("all")}
              >
                All Banners
              </button>
              <button
                className={`banner-filter-tab ${currentTab === "active" ? "active" : ""}`}
                onClick={() => filter("active")}
              >
                Active
              </button>
              <button
                className={`banner-filter-tab ${currentTab === "inActive" ? "active" : ""}`}
                onClick={() => filter("inActive")}
              >
                Inactive
              </button>
            </div>
          </div>

          <div className="banners-grid">
            {banners.map((banner) => (
              <div className="banner-card" key={banner.bannerSlideID}>
                <div
                  className="banner-preview"
                  style={{ background: banner?.backgroundColor?.colorHexCode }}
                >
                  <div className="banner-content">
                    <span
                      className="banner-label"
                      style={{ color: banner?.accentColor?.colorHexCode }}
                    >
                      {banner.label}
                    </span>
                    <h3
                      className="banner-title"
                      dangerouslySetInnerHTML={{ __html: banner.title ?? "" }}
                    ></h3>
                    <p className="banner-subtitle">{banner.subtitle}</p>
                    <button
                      className="banner-cta"
                      style={{ background: banner?.accentColor?.colorHexCode }}
                    >
                      {banner.cta}
                    </button>
                  </div>
                  <div className="banner-image">
                    <img
                      src={getBannerImageUrl(
                        banner.bannerSlideID ?? banner.imageUrl,
                      )}
                      alt="Banner"
                    />
                  </div>
                </div>
                <div className="banner-info">
                  <div className="banner-details">
                    <div className="banner-meta">
                      <span
                        className={`status-badge  ${banner.isActive ? "active" : "inactive"}`}
                      >
                        {banner.isActive ? "Active" : "InActive"}
                      </span>
                    </div>
                  </div>
                  <div className="banner-actions">
                    <button
                      className="btn-icon btn-toggle"
                      title="Toggle Status"
                      onClick={() => toggleBannerStatus(banner.bannerSlideID!)}
                    >
                      <span
                        className={`toggle-switch ${banner.isActive ? "active" : "inactive"}`}
                      ></span>
                    </button>
                    <button
                      className="btn-icon btn-edit"
                      title="Edit"
                      onClick={() => popUpModal(banner.bannerSlideID ?? null)}
                    >
                      ✏️
                    </button>
                    <button
                      className="btn-icon btn-delete"
                      title="Delete"
                      onClick={() => {
                        setSelectedBannerID(banner.bannerSlideID ?? null);
                        setIsDeletingBanner(true);
                      }}
                    >
                      🗑️
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
          {banners.length === 0 && (
            <div className="empty-state">
              <div className="empty-icon">🎨</div>
              <h3>No Banners Found</h3>
              <p>Create your first banner for the homepage</p>
              <button className="btn-primary">Add Banner</button>
            </div>
          )}
        </div>
      </main>

      {isDeletingBanner && <BannerDeleteModel onActionResult={onDeleteEvent} />}

      {isOpenModel && (
        <BannerAdd_UpdateModel
          onActionResult={onReloadBannerEvent}
          onClose={toggleModel}
          bannerIDProps={selectedBannerID}
        />
      )}
    </>
  );
}
