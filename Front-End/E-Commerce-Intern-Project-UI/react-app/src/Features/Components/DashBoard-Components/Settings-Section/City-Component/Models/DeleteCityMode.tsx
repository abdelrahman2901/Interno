type props = {
  onActionResult: (result: boolean) => void;
};
export default function DeleteCityModel({ onActionResult }: props) {
  return (
    <>
      <div id="deleteModal" className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-small">
          <div className="Custom-modal-header">
            <h3>Delete City</h3>
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
            <p>Are you sure you want to delete this city?</p>
            <p className="warning-text">
              ⚠️ All areas and shipping costs associated with this city will
              also be affected.
            </p>
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
