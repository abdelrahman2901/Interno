import { CartModel } from "../CartDTO/CartModel";
import { ProductDetails } from "../ProductDTO/ProductDetails";

export class CartItemDetails {
  cartItemID: string = "";
  cart: CartModel = new CartModel();
  product: ProductDetails = new ProductDetails();
  quantity: number = 1;
  addedDate: string = "";
  isDeleted: boolean = false;
}
