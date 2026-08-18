import { ProductDetails } from "../ProductDTO/ProductDetails";

export class WishListDetailsModel {
  wishlistID: string = "";
  userID: string = "";
  product: ProductDetails = new ProductDetails();
  addedDate: string = "";
  isDeleted: boolean = false;
}
