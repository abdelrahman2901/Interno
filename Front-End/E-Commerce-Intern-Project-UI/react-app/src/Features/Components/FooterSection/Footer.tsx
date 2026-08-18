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
              <div className="footer-heading">Shop</div>
              <div className="footer-link">Women</div>
              <div className="footer-link">Men</div>
              <div className="footer-link">Kids</div>
              <div className="footer-link">Accessories</div>
              <div className="footer-link">Sale</div>
            </div>
            <div className="footer-column">
              <div className="footer-heading">Help</div>
              <div className="footer-link">FAQ</div>
              <div className="footer-link">Shipping</div>
              <div className="footer-link">Returns</div>
              <div className="footer-link">Size Guide</div>
              <div className="footer-link">Contact</div>
            </div>
            <div className="footer-column">
              <div className="footer-heading">Company</div>
              <div className="footer-link">About Us</div>
              <div className="footer-link">Careers</div>
              <div className="footer-link">Press</div>
              <div className="footer-link">Sustainability</div>
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
