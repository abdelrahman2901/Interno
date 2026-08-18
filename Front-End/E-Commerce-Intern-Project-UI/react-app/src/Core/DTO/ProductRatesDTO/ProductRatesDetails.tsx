import { ProductDetails } from "../ProductDTO/ProductDetails";

export class ProductRatesDetails {
  rateID: string = "";
  userID: string = "";
  product: ProductDetails = new ProductDetails();
  rating: number = 0;
}
