import "./Header.css";
import { useEffect, useRef, useState } from "react";
import gsap from "gsap";
import { ScrollTrigger } from "gsap/ScrollTrigger";
import {
  Truck,
  RotateCcw,
  Shield,
  Diamond,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";
import {
  BannerSlideResponse,
  InternoMicroserviceClient,
} from "../../../Core/API/Interno-Microservice-Client/Interno-Microservice-Client";
import { API, config, getBannerImageUrl } from "../../../Core/Shared/API-URL";
import { getApiErrorMessage } from "../../../Core/Shared/getApiErrorMessage";

gsap.registerPlugin(ScrollTrigger);

export default function Header() {
  const [slide, setSlide] = useState<BannerSlideResponse>();
  const [slides, setSlides] = useState<BannerSlideResponse[]>([]);
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const Client = new InternoMicroserviceClient(config.gatewayUrl, API);
  const loadSlides = async () => {
    try {
      const response = await Client.getAllBanners();
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
      console.error(getApiErrorMessage(err));
    }
  };
  const heroRef = useRef<HTMLDivElement | null>(null);
  const heroImageRef = useRef<HTMLDivElement | null>(null);
  const trustRef = useRef<HTMLDivElement | null>(null);
  const hasAnimated = useRef(false);
  useEffect(() => {
    if (slides.length === 0) {
      loadSlides();
    }
  }, []);
  useEffect(() => {
    if (!heroRef.current || !slide || hasAnimated.current) return;
    hasAnimated.current = true;
    const ctx = gsap.context(() => {
      gsap.fromTo(
        heroRef.current,
        { opacity: 0, y: 22 },
        { opacity: 1, y: 0, duration: 0.7, ease: "power3.out" },
      );
      if (heroImageRef.current) {
        gsap.fromTo(
          heroImageRef.current,
          { opacity: 0, x: 28, scale: 1.04 },
          { opacity: 1, x: 0, scale: 1, duration: 0.85, ease: "power3.out" },
        );
        gsap.to(heroImageRef.current, {
          yPercent: 8,
          ease: "none",
          scrollTrigger: {
            trigger: heroImageRef.current,
            start: "top top",
            end: "bottom top",
            scrub: true,
          },
        });
      }
      if (trustRef.current) {
        gsap.fromTo(
          trustRef.current.querySelectorAll(".trust-item"),
          { opacity: 0, y: 16 },
          {
            opacity: 1,
            y: 0,
            duration: 0.45,
            stagger: 0.08,
            delay: 0.2,
            ease: "power2.out",
          },
        );
      }
    });
    return () => ctx.revert();
  }, [!!slide]);

  function imageSlide(Direction: string) {
    console.log(slide);
    console.log(slides);

    const index = slides.findIndex(
      (r) => r.bannerSlideID === slide?.bannerSlideID,
    );

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
        style={{ background: slide?.backgroundColor?.colorHexCode }}
      >
        <div className="hero-container">
          <div className="hero-content" ref={heroRef}>
            <div
              className="hero-label"
              style={{ color: slide?.accentColor?.colorHexCode }}
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
                style={{ background: slide?.accentColor?.colorHexCode }}
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
          <div className="hero-image" ref={heroImageRef}>
            <img src={getBannerImageUrl(slide?.bannerSlideID ?? slide?.imageUrl)} />
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
        <div className="trust-container" ref={trustRef}>
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
