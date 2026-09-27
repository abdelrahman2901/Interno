import { useEffect, useState } from "react";
import {
  BannerSlideResponse,
  ColorResponse,
  FileParameter,
  InternoMicroserviceClient,
} from "../../../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API } from "../../../../../Core/Shared/API-URL";
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
  const Client = new InternoMicroserviceClient(undefined, API);
  const [banner, setBanner] = useState<BannerSlideResponse>(
    new BannerSlideResponse(),
  );
  const [colors, setColors] = useState<ColorResponse[]>([]);
  const [selectedImage, setSelectedImage] = useState<File | null>(null);

  const loadBanner = async () => {
    try {
      const response = await Client.getBannnerByID(bannerIDProps!);
      if (response.data && response.isSuccess) {
        setBanner(response.data);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  const loadColors = async () => {
    try {
      const response = await Client.getAllColors();
      if (response.data && response.isSuccess) {
        setColors(response.data);
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

  const toFileParameter = (file: File): FileParameter => ({
    data: file,
    fileName: file.name,
  });

  function SaveChanges() {
    const bannerImage = selectedImage
      ? toFileParameter(selectedImage)
      : undefined;
    const save = async () => {
      try {
        const response = banner.bannerSlideID
          ? await Client.updateBanner(
              banner.bannerSlideID,
              banner.title,
              banner.subtitle,
              banner.cta,
              banner.backgroundColor?.colorID ?? null,
              banner.backgroundColor?.colorHexCode ?? null,
              banner.accentColor?.colorID ?? null,
              banner.accentColor?.colorHexCode ?? null,
              banner.label,
              bannerImage,
            )
          : await Client.createBanner(
              banner.title,
              banner.subtitle,
              banner.cta,
              banner.backgroundColor?.colorID ?? null,
              banner.backgroundColor?.colorHexCode ?? null,
              banner.accentColor?.colorID ?? null,
              banner.accentColor?.colorHexCode ?? null,
              banner.label,
              bannerImage,
            );
        if (response.isSuccess) {
          onActionResult();
        }
      } catch (err) {
        if (err) console.error(err);
      }
    };
    save();
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
                      setBanner(
                        (prev) =>
                          new BannerSlideResponse({ ...prev, title: title }),
                      );
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
                      setBanner(
                        (prev) =>
                          new BannerSlideResponse({
                            ...prev,
                            subtitle: subtitle,
                          }),
                      );
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
                        setBanner(
                          (prev) =>
                            new BannerSlideResponse({ ...prev, label: label }),
                        );
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
                        setBanner(
                          (prev) =>
                            new BannerSlideResponse({ ...prev, cta: cta }),
                        );
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
                        onChange={(e) => {
                          debugger;
                          const SelectedColor = e.currentTarget.value;
                          const color =
                            colors.find(
                              (r) =>
                                r.colorID === SelectedColor ||
                                r.colorHexCode === SelectedColor,
                            ) ?? new ColorResponse();
                          color!.colorHexCode = SelectedColor;
                          setBanner((prev) =>
                            new BannerSlideResponse({
                              ...prev,
                              backgroundColor: color!,
                            }),
                          );
                          console.log(banner.backgroundColor);
                        }}
                        // value={selectedBg}
                      />
                      <select
                        onChange={(e) => {
                          const SelectedColor = e.currentTarget.value;
                          const color =
                            colors.find(
                              (r) =>
                                r.colorID === SelectedColor ||
                                r.colorHexCode === SelectedColor,
                            ) ?? new ColorResponse();
                          color!.colorHexCode = SelectedColor;
                          setBanner(
                            (prev) =>
                              new BannerSlideResponse({
                                ...prev,
                                backgroundColor: color!,
                              }),
                          );
                        }}
                        value={banner.backgroundColor?.colorID}
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
                        onChange={(e) => {
                          debugger;
                          const SelectedColor = e.currentTarget.value;
                          const color =
                            colors.find(
                              (r) =>
                                r.colorID === SelectedColor ||
                                r.colorHexCode === SelectedColor,
                            ) ?? new ColorResponse();
                          color!.colorHexCode = SelectedColor;

                          setBanner((prev) =>
                            new BannerSlideResponse({
                              ...prev,
                              accentColor: color!,
                            }),
                          );
                          console.log(banner.accentColor);
                        }}

                        // value={banner.backgroundColor?.colorID}
                      />
                      <select
                        onChange={(e) => {
                          const SelectedColor = e.currentTarget.value;
                          const color = colors.find(
                            (r) =>
                              r.colorID === SelectedColor ||
                              r.colorHexCode === SelectedColor,
                          );
                          banner.accentColor!.colorHexCode = SelectedColor;
                          setBanner(
                            (prev) =>
                              new BannerSlideResponse({
                                ...prev,
                                accentColor: color!,
                              }),
                          );
                        }}
                        value={banner.accentColor?.colorID}
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
                  style={{ background: banner?.backgroundColor?.colorHexCode }}
                >
                  <div className="banner-content">
                    {banner.label && (
                      <span className="banner-label">{banner.label}</span>
                    )}
                    <h3
                      className="banner-title"
                      style={{ color: banner?.accentColor?.colorHexCode }}
                      dangerouslySetInnerHTML={{ __html: banner.title ?? "" }}
                    ></h3>
                    <p className="banner-subtitle">{banner.subtitle}</p>
                    {banner.cta && (
                      <button
                        className="banner-cta"
                        style={{
                          background: banner?.accentColor?.colorHexCode,
                        }}
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
