import { BannerSlidesDetails } from "../../../Core/DTO/BannerSlidesDTO/BannerSlidesDetails";
import { GetAllBanners } from "../../../Core/Services/BannerServices/BannerService";
import "./Header.css";
import { useEffect, useState } from "react";
import {
  Truck,
  RotateCcw,
  Shield,
  Diamond,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

const Base_Url = "https://localhost:7164/BannerImages";
export default function Header() {
  const [slide, setSlide] = useState<BannerSlidesDetails>();
  const [slides, setSlides] = useState<BannerSlidesDetails[]>([]);
  const [currentIndex, setCurrentIndex] = useState<number>(0);

  const loadSlides = async () => {
    try {
      const response = await GetAllBanners();
      if (response.data && response.isSuccess) {
        setCurrentIndex(
          response.data.findIndex(
            (r) => r.bannerSlideID === response.data![0].bannerSlideID!,
          ),
        );
        setSlides(response.data);
        setSlide(response.data[0]);
      }
    } catch (err) {
      if (err) console.error(err);
    }
  };
  useEffect(() => {
    if (slides.length === 0) {
      loadSlides();
    }
  }, []);

  function imageSlide(Direction: string) {
    console.log(slide);
    console.log(slides);

    const index = slides.findIndex(
      (r) => r.bannerSlideID === slide?.bannerSlideID,
    );
    console.log(index);
    console.log(currentIndex);

    switch (Direction) {
      case "Right": {
        setSlide(slides[(index + 1 + slides.length) % slides.length]);
        setCurrentIndex((index + 1 + slides.length) % slides.length);

        break;
      }
      case "Left": {
        setSlide(slides[(index - 1 + slides.length) % slides.length]);
        setCurrentIndex((index - 1 + slides.length) % slides.length);
        break;
      }
    }
  }
  return (
    <>
      <div className="announcement-bar">
        FREE SHIPPING ON ORDERS OVER $75 &nbsp;|&nbsp; USE CODE:
        <strong>WELCOME10</strong> FOR 10% OFF
      </div>

      <section
        className="hero"
        style={{ background: slide?.backgroundColor.colorHexCode }}
      >
        <div className="hero-container">
          <div className="hero-content">
            <div
              className="hero-label"
              style={{ color: slide?.accentColor.colorHexCode }}
            >
              ── {slide?.label}
            </div>
            <h1
              className="hero-title"
              dangerouslySetInnerHTML={{ __html: slide?.title! }}
            />
            <p className="hero-subtitle">{slide?.subtitle}</p>
            <div className="hero-buttons">
              <button
                className="btn-primary "
                style={{ background: slide?.accentColor.colorHexCode }}
              >
                {slide?.cta}
              </button>
              <button className="btn-outline">View All</button>
            </div>
            <div className="slide-dots">
              {slides.map((slide, index) => (
                <button
                  key={index}
                  className={`dot  ${currentIndex === index ? "active" : " "}`}
                ></button>
              ))}
            </div>
          </div>
          <div className="hero-image">
            <img src={`${Base_Url}/${slide?.imageUrl}`} />
            <button
              className="slide-btn prev"
              onClick={() => imageSlide("Left")}
            >
              <ChevronLeft size={18} />
            </button>
            <button
              className="slide-btn next"
              onClick={() => imageSlide("Right")}
            >
              <ChevronRight size={18} />
            </button>
          </div>
        </div>
      </section>

      <section className="trust-badges">
        <div className="trust-container">
          <div className="trust-item">
            <span className="trust-icon">
              <Truck size={22} />
            </span>
            <div>
              <div className="trust-title">Free Shipping</div>
              <div className="trust-sub">On orders over $75</div>
            </div>
          </div>
          <div className="trust-item">
            <span className="trust-icon">
              <RotateCcw size={22} />
            </span>
            <div>
              <div className="trust-title">Easy Returns</div>
              <div className="trust-sub">30-day return policy</div>
            </div>
          </div>
          <div className="trust-item">
            <span className="trust-icon">
              <Shield size={22} />
            </span>
            <div>
              <div className="trust-title">Secure Payment</div>
              <div className="trust-sub">100% protected</div>
            </div>
          </div>
          <div className="trust-item">
            <span className="trust-icon">
              <Diamond size={22} />
            </span>
            <div>
              <div className="trust-title">Premium Quality</div>
              <div className="trust-sub">Crafted with care</div>
            </div>
          </div>
        </div>
      </section>
    </>
  );
}
