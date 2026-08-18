import { ProductDetails } from "../ProductDTO/ProductDetails";

export class OrderItemDetails {
  orderItemID: string = "";
  orderID: string = "";
  product: ProductDetails = new ProductDetails();
  addedDate: string = "";
  quantity: number = 0;
  isDeleted: boolean = false;
}
