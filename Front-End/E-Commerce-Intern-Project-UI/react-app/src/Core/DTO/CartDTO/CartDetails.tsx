import { CartItemDetails } from "../CartItemDTO/CartItemDetails";

export class CartDetails {
  cartID: string = "";
  userID: string = "";
  createdDate: string = "";
  cartItems: CartItemDetails[] = [];
}
