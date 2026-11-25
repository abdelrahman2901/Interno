import { useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import gsap from "gsap";
import FooterSection from "../../Components/FooterSection/Footer";
import "./PageLayout.css";
import "./InfoPages.css";

export type PageCrumb = {
  label: string;
  to?: string;
};

type props = {
  title: string;
  subtitle: string;
  crumbs: PageCrumb[];
  children: React.ReactNode;
};

export default function PageLayout({ title, subtitle, crumbs, children }: props) {
  const bodyRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    if (!bodyRef.current) return;
    gsap.fromTo(
      bodyRef.current,
      { opacity: 0, y: 18 },
      { opacity: 1, y: 0, duration: 0.5, ease: "power2.out" },
    );
  }, []);

  return (
    <>
      <div className="page-layout">
        <header className="page-hero">
          <div className="page-hero-inner">
            <nav className="page-breadcrumb" aria-label="Breadcrumb">
              <ol>
                {crumbs.map((crumb, index) => {
                  const isLast = index === crumbs.length - 1;
                  return (
                    <li key={`${crumb.label}-${index}`}>
                      {crumb.to && !isLast ? (
                        <Link to={crumb.to}>{crumb.label}</Link>
                      ) : (
                        <span aria-current={isLast ? "page" : undefined}>
                          {crumb.label}
                        </span>
                      )}
                    </li>
                  );
                })}
              </ol>
            </nav>
            <p className="page-kicker">Interno</p>
            <h1>{title}</h1>
            <p className="page-subtitle">{subtitle}</p>
          </div>
        </header>
        <div className="page-body" ref={bodyRef}>
          {children}
        </div>
      </div>
      <FooterSection />
    </>
  );
}
