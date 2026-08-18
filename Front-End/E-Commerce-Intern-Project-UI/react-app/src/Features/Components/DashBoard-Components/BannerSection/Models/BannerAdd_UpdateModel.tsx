import { useEffect, useState } from "react";
import { BannerSlidesDetails } from "../../../../../Core/DTO/BannerSlidesDTO/BannerSlidesDetails";
import {
  CreateBanner,
  GetBannnerByID,
  UpdateBanner,
} from "../../../../../Core/Services/BannerServices/BannerService";
import { ColorDetails } from "../../../../../Core/DTO/Colors/ColorDetails";
import { GetColors_Serv } from "../../../../../Core/Services/ColorsServices/ColorService_Query";
import { BannerSlidesRequest } from "../../../../../Core/DTO/BannerSlidesDTO/BannerSlidesRequest";

type props = {
  onClose: () => void;
  bannerIDProps: string | null;
  onActionResult: () => void;
};

export default function BannerAdd_UpdateModel({
  bannerIDProps,
  onClose,
  onActionResult,
}: props) {
  const [banner, setBanner] = useState<BannerSlidesDetails>(
    new BannerSlidesDetails(),
  );
  const [colors, setColors] = useState<ColorDetails[]>([]);
  const [selectedBg, setSelectedBg] = useState<string>("");
  const [selectedaccent, setSelectedAccent] = useState<string>("");
  const [selectedImage, setSelectedImage] = useState<File | null>(null);

  const loadBanner = async () => {
    try {
      const response = await GetBannnerByID(bannerIDProps!);
      if (response.data && response.isSuccess) {
        setBanner(response.data);
        setSelectedAccent(response.data.accentColor.colorHexCode);
        setSelectedBg(response.data.backgroundColor.colorHexCode);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadColors = async () => {
    try {
      const response = await GetColors_Serv();
      if (response.data && response.isSuccess) {
        setColors(response.data);
        setSelectedBg(response.data[0].colorHexCode);
        setSelectedAccent(response.data[0].colorHexCode);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (colors.length === 0) {
      loadColors();
    }
  }, []);
  useEffect(() => {
    if (bannerIDProps) {
      loadBanner();
    }
  }, [bannerIDProps]);

  const add_bannerReq = async (request: BannerSlidesRequest) => {
    console.log("wgwrgwr");

    try {
      const response = await CreateBanner(request);
      if (response.isSuccess) {
        onActionResult();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const update_bannerReq = async (request: BannerSlidesRequest) => {
    console.log("wgwrgwr");

    try {
      const response = await UpdateBanner(request);
      if (response.isSuccess) {
        onActionResult();
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };

  function SaveChanges() {
    const request: BannerSlidesRequest = {
      BannerSlideID: banner.bannerSlideID ? banner.bannerSlideID : null,
      title: banner.title,
      subtitle: banner.subtitle,
      CTA: banner.cta,
      backgroundColorID: banner.backgroundColor.colorID,
      accentColorID: banner.accentColor.colorID,
      label: banner.label,
      bannerImage: selectedImage!,
    };
    if (banner.bannerSlideID) {
      update_bannerReq(request);
    } else {
      add_bannerReq(request);
    }
  }
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-large">
          <div className="Custom-modal-header">
            <h3>Add New Banner</h3>
            <button className="modal-close" onClick={onClose}>
              &times;
            </button>
          </div>
          <form>
            <div className="Custom-modal-body">
              <div className="Custom-form-section">
                <h4 className="Custom-section-title">Banner Content</h4>

                <div className="Custom-form-group">
                  <label>Title *</label>
                  <input
                    type="text"
                    required
                    placeholder="e.g., Summer Collection 2026"
                    onChange={(e) => {
                      const title = e.currentTarget.value;
                      setBanner((prev) => ({ ...prev, title: title }));
                    }}
                    value={banner.title}
                  />
                  <small className="Custom-form-hint">
                    Use &lt;br&gt; for line breaks
                  </small>
                </div>

                <div className="Custom-form-group">
                  <label>Subtitle *</label>
                  <input
                    type="text"
                    onChange={(e) => {
                      const subtitle = e.currentTarget.value;
                      setBanner((prev) => ({ ...prev, subtitle: subtitle }));
                    }}
                    value={banner.subtitle}
                    placeholder="e.g., Light fabrics, timeless style"
                  />
                </div>

                <div className="Custom-form-row">
                  <div className="Custom-form-group">
                    <label>Label</label>
                    <input
                      type="text"
                      placeholder="e.g., SEASON 2026"
                      onChange={(e) => {
                        const label = e.currentTarget.value;
                        setBanner((prev) => ({ ...prev, label: label }));
                      }}
                      value={banner.label}
                    />
                  </div>
                  <div className="Custom-form-group">
                    <label>Button Text *</label>
                    <input
                      type="text"
                      placeholder="e.g., Shop Women"
                      onChange={(e) => {
                        const cta = e.currentTarget.value;
                        setBanner((prev) => ({ ...prev, cta: cta }));
                      }}
                      value={banner.cta}
                    />
                  </div>
                </div>

                {/* <div className="Custom-form-group">
                  <label>Button Link *</label>
                  <input
                    type="url"
                    placeholder="e.g., /products?category=women"
                  />
                </div> */}
              </div>

              <div className="Custom-form-section">
                <h4 className="Custom-section-title">Visual Design</h4>
                <div className="Custom-form-row">
                  <div className="Custom-form-group">
                    <label>Background Color *</label>
                    <div className="Custom-color-input-group">
                      <input
                        type="color"
                        disabled
                        onChange={() => {}}
                        value={selectedBg}
                      />
                      <select
                        onChange={(e) => {
                          const bgID = e.currentTarget.value;
                          const color = colors.find((r) => r.colorID === bgID);
                          setSelectedBg(color?.colorHexCode!);
                          setBanner((prev) => ({
                            ...prev,
                            backgroundColor: color!,
                          }));
                        }}
                        value={banner.backgroundColor.colorID}
                      >
                        {colors.map((color) => (
                          <option key={color.colorID} value={color.colorID}>
                            {color.colorName}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                  <div className="Custom-form-group">
                    <label>Accent Color *</label>
                    <div className="Custom-color-input-group">
                      <input
                        type="color"
                        disabled
                        onChange={() => {}}
                        value={selectedaccent}
                      />
                      <select
                        onChange={(e) => {
                          const bgID = e.currentTarget.value;
                          const color = colors.find((r) => r.colorID === bgID);
                          setSelectedAccent(color?.colorHexCode!);
                          setBanner((prev) => ({
                            ...prev,
                            accentColor: color!,
                          }));
                        }}
                        value={banner.accentColor.colorID}
                      >
                        {colors.map((color) => (
                          <option key={color.colorID} value={color.colorID}>
                            {color.colorName}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                </div>

                <div className="Custom-form-group">
                  <label>Image *</label>
                  <input
                    type="file"
                    placeholder="https://example.com/image.jpg"
                    onChange={(e) => {
                      const image = e.currentTarget.files![0];

                      setSelectedImage(image);
                    }}
                  />
                </div>
              </div>
              {!banner.bannerSlideID && (
                <div className="Custom-form-section">
                  <div className="Custom-form-row">
                    <div className="Custom-form-group">
                      <label className="Custom-checkbox-label">
                        <input type="checkbox" />
                        <span>Active (Visible to customers)</span>
                      </label>
                    </div>
                  </div>
                </div>
              )}

              <div className="form-section">
                <h4 className="section-title">Live Preview</h4>
                <div
                  className="banner-preview-container"
                  style={{ background: banner?.backgroundColor.colorHexCode }}
                >
                  <div className="banner-content">
                    {banner.label && (
                      <span className="banner-label">{banner.label}</span>
                    )}
                    <h3
                      className="banner-title"
                      style={{ color: banner?.accentColor.colorHexCode }}
                      dangerouslySetInnerHTML={{ __html: banner.title }}
                    ></h3>
                    <p className="banner-subtitle">{banner.subtitle}</p>
                    {banner.cta && (
                      <button
                        className="banner-cta"
                        style={{ background: banner?.accentColor.colorHexCode }}
                      >
                        {banner.cta}
                      </button>
                    )}
                  </div>
                  <div className="banner-image">
                    {!selectedImage && (
                      // <div className="placeholder-image">📷 Image Preview</div>
                      <div className="placeholder-image">📷 Image Preview</div>
                    )}
                    {selectedImage && (
                      <img src={URL.createObjectURL(selectedImage)} alt="" />
                    )}
                  </div>
                </div>
              </div>
            </div>
            <div className="Custom-modal-footer">
              <button type="button" className="btn-secondary" onClick={onClose}>
                Cancel
              </button>
              <button
                type="button"
                className="btn-primary"
                onClick={SaveChanges}
              >
                Save Banner
              </button>
            </div>
          </form>
        </div>
      </div>
    </>
  );
}
