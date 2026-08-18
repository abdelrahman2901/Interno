import { useState } from "react";
// import "./ProductDashBoardSection.css";
import { DeleteModel } from "../Shared/Model/DeleteModel";
import { IProductDetails } from "../../../../Core/Interface/Products/IProductDetails";
import { DeleteProduct_Serv } from "../../../../Core/Services/ProductServices/ProductServiceCmd";
import ProductModel from "./Model/ProductModel";
type props = {
  Products: IProductDetails[];
  reLoadProduct: () => void;
};
export default function ProductDashBoardSection({
  Products,
  reLoadProduct,
}: props) {
  const [UpdateProduct, SetUpdateProduct] = useState<string | null>(null);
  const [isAddingProduct, SetIsAddingProduct] = useState<boolean>(false);
  const [isDeleteingProduct, SetIsDeletingingProduct] =
    useState<boolean>(false);
  const [CurrentProductToDelete, SetCurrentProductToDelete] =
    useState<string>("");

  function onAddUpdateEvent() {
    SetIsAddingProduct(false);
    reLoadProduct();
  }

  function deleteProduct(ProductID: string) {
    const deleteProduct_Req = async () => {
      const response = await DeleteProduct_Serv(ProductID);
      console.log(response);
      if (response.isSuccess) {
        reLoadProduct();
      }
    };
    deleteProduct_Req();
  }

  function onDeleteProductEvent(value: Boolean) {
    SetIsDeletingingProduct(false);
    if (value) {
      deleteProduct(CurrentProductToDelete);
    }
  }
  return (
    <>
      <div className="section-header">
        <h3>Manage Products</h3>
        <button
          className="btn-primary"
          id="addProductBtn"
          onClick={() => SetIsAddingProduct(true)}
        >
          + Add Product
        </button>
      </div>

      <div className="products-table-wrapper">
        <table className="products-table" id="productsTable">
          <thead>
            <tr>
              <th>Image</th>
              <th>Name</th>
              <th>Category</th>
              <th>Price</th>
              <th>Stock</th>
              <th>Size</th>
              <th>Color</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody id="productsTableBody">
            {Products.map((product) => (
              <tr key={product.productID}>
                <td>
                  <img
                    src={`https://localhost:7164/ProductsImages/${product.parentCategoryName}/${product.categoryName}/${product.productImageUrl}`}
                    alt={product.productName}
                    className="product-image"
                  />
                </td>
                <td>
                  <strong>{product.productName}</strong>
                </td>
                <td>{product.categoryName}</td>
                <td>
                  <strong>
                    $
                    {product.salePrice
                      ? product.salePrice.toFixed(2)
                      : product.price.toFixed(2)}
                  </strong>

                  {product.salePrice !== null && product.salePrice !== 0 && (
                    <span className="sale">
                      <br /> {product.price.toFixed(2)}
                    </span>
                  )}
                </td>
                <td>{product.stock}</td>
                <td>{product.sizeName}</td>
                <td>{product.colorName}</td>
                <td>
                  <span
                    className={
                      "product-status " +
                      (product.active ? "active" : "inactive")
                    }
                  >
                    {product.active ? "Active" : "Inactive"}
                  </span>
                </td>
                <td>
                  <div className="table-actions">
                    <button
                      className="action-btn edit"
                      onClick={() => {
                        SetIsAddingProduct(true);
                        SetUpdateProduct(product.productID);
                      }}
                      title="Edit"
                    >
                      ✏️
                    </button>
                    <button
                      className="action-btn delete"
                      onClick={() => {
                        SetCurrentProductToDelete(product.productID);
                        SetIsDeletingingProduct(true);
                      }}
                      title="Delete"
                    >
                      🗑️
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {isAddingProduct && (
        <ProductModel
          onClose={() => {
            SetUpdateProduct(null);
            SetIsAddingProduct(false);
          }}
          onAddUpdateProduct={onAddUpdateEvent}
          ProductProps={UpdateProduct}
        />
      )}
      {isDeleteingProduct && (
        <DeleteModel onDeleteConfirm={onDeleteProductEvent} />
      )}
    </>
  );
}
