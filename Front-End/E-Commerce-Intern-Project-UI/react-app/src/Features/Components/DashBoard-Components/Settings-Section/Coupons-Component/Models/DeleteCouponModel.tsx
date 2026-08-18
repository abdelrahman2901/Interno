type props = {
  onResultAction: (result: boolean) => void;
};
export default function DeleteCouponModel({ onResultAction }: props) {
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-small">
          <div className="Custom-modal-header">
            <h3>Delete Coupon</h3>
            <button
              className="modal-close"
              onClick={() => {
                onResultAction(false);
              }}
            >
              &times;
            </button>
          </div>
          <div className="Custom-modal-body">
            <p>Are you sure you want to delete this coupon?</p>
            <p className="warning-text">
              ⚠️ This action cannot be undone. Coupon usage history will be
              preserved.
            </p>
          </div>
          <div className="Custom-modal-footer">
            <button
              type="button"
              className="btn-secondary"
              onClick={() => {
                onResultAction(false);
              }}
            >
              Cancel
            </button>
            <button
              type="button"
              className="btn-danger"
              onClick={() => {
                onResultAction(true);
              }}
            >
              Delete
            </button>
          </div>
        </div>
      </div>
    </>
  );
}
