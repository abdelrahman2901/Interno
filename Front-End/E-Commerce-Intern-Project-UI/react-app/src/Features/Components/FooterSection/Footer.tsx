import { Link } from "react-router-dom";
import "./Footer.css";

export default function FooterSection() {
  return (
    <>
      <footer className="footer">
        <div className="footer-container">
          <div className="footer-grid">
            <div className="footer-column brand">
              <div className="footer-logo">Interno</div>
              <p className="footer-desc">Our Social Accounts.</p>
              <div className="social-icons">
                <div className="social-icon">f</div>
                <div className="social-icon">in</div>
                <div className="social-icon">tw</div>
                <div className="social-icon">ig</div>
              </div>
            </div>
            <div className="footer-column">
              <div className="footer-heading">Company</div>
              <Link to="/Home/About" className="footer-link">
                About
              </Link>
              <Link to="/Home/Contact" className="footer-link">
                Contact
              </Link>
            </div>
            <div className="footer-column">
              <div className="footer-heading">Help</div>
              <Link to="/Home/FAQ" className="footer-link">
                FAQ
              </Link>
              <Link to="/Home/Shipping" className="footer-link">
                Shipping
              </Link>
              <Link to="/Home/Returns" className="footer-link">
                Returns
              </Link>
              <Link to="/Home/SizeGuide" className="footer-link">
                Size Guide
              </Link>
              <Link to="/Home/Contact" className="footer-link">
                Contact
              </Link>
            </div>
          </div>
          <div className="footer-bottom">
            <span className="copyright">
              © 2026 Interno. All rights reserved.
            </span>
            <div className="footer-legal">
              <span className="footer-link">Privacy Policy</span>
              <span className="footer-link">Terms of Service</span>
              <span className="footer-link">Cookie Policy</span>
            </div>
          </div>
        </div>
      </footer>
    </>
  );
}
