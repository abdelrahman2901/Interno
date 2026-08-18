import "../css/Model.css";
type props = {
  onDeleteConfirm: (value: boolean) => void;
};
export function DeleteModel({ onDeleteConfirm }: props) {
  return (
    <>
      <div className="Custom-modal">
        <div className="Custom-modal-content Custom-modal-small">
          <div className="Custom-modal-header">
            <h3>Confirm Delete</h3>
            <button className="close-btn" id="closeDeleteModal">
              &times;
            </button>
          </div>
          <div className="Custom-modal-body">
            <p id="deleteMessage">Are you sure you want to delete this item?</p>
          </div>
          <div className="Custom-form-actions">
            <button
              className="btn-secondary"
              onClick={() => onDeleteConfirm(false)}
            >
              Cancel
            </button>
            <button
              className="btn-danger"
              onClick={() => onDeleteConfirm(true)}
            >
              Delete
            </button>
          </div>
        </div>
      </div>
    </>
  );
}
