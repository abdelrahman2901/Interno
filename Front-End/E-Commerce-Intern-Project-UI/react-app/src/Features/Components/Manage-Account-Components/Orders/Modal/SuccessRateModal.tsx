type props = {
  onClose: () => void;
};
export default function SuccessRateModal({ onClose }: props) {
  return (
    <>
      <div id="ratingSuccessModal" className="Custom-modal">
        <div className="rating-modal-content rating-success-content">
          <div className="success-animation">
            <div className="success-checkmark">
              <div className="check-icon">
                <span className="icon-line line-tip"></span>
                <span className="icon-line line-long"></span>
                <div className="icon-circle"></div>
                <div className="icon-fix"></div>
              </div>
            </div>
          </div>

          <h2>Thank You!</h2>
          <p>Your review has been submitted successfully.</p>
          <p className="success-note">
            It will be published after our team reviews it.
          </p>

          <button className="btn-primary" onClick={onClose}>
            Close
          </button>
        </div>
      </div>
    </>
  );
}
