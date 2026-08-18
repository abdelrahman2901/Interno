type props = {
  onActionResult: (result: boolean) => void;
};
export default function BannerDeleteModel({ onActionResult }: props) {
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-small">
          <div className="Custom-modal-header">
            <h3>Delete Banner</h3>
            <button
              className="modal-close"
              onClick={() => {
                onActionResult(false);
              }}
            >
              &times;
            </button>
          </div>
          <div className="Custom-modal-body">
            <p>Are you sure you want to delete this banner?</p>
            <p className="warning-text">⚠️ This action cannot be undone.</p>
          </div>
          <div className="Custom-modal-footer">
            <button
              type="button"
              className="btn-secondary"
              onClick={() => {
                onActionResult(false);
              }}
            >
              Cancel
            </button>
            <button
              type="button"
              className="btn-danger"
              onClick={() => {
                onActionResult(true);
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
