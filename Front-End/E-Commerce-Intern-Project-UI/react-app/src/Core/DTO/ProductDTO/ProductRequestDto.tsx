export class ProductRequest {
  productName: string = "";
  categoryID: string = "";
  price: number = 0;
  sizeID: string = "";
  colorID: string = "";
  salePrice: number = 0;
  stock: number = 0;
  productImage: File = new File([], "");
  active: boolean = false;
}
