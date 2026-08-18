type props = {
  ConfirmResult: (resut: boolean) => void;
};
export default function DeleteAreaModel({ ConfirmResult }: props) {
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content modal-small">
          <div className="Custom-modal-header">
            <h3>Delete Area</h3>
            <button
              className="modal-close"
              onClick={() => {
                ConfirmResult(false);
              }}
            >
              &times;
            </button>
          </div>
          <div className="Custom-modal-body">
            <p>Are you sure you want to delete this area?</p>
            <p className="warning-text">
              ⚠️ All shipping costs associated with this area will also be
              affected.
            </p>
          </div>
          <div className="Custom-modal-footer">
            <button
              type="button"
              className="btn-secondary"
              onClick={() => {
                ConfirmResult(false);
              }}
            >
              Cancel
            </button>
            <button
              type="button"
              className="btn-danger"
              onClick={() => {
                ConfirmResult(true);
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
