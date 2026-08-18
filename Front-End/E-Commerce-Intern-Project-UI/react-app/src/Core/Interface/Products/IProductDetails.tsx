export interface IProductDetails {
  productID: string;
  categoryID: string;
  parentcategoryID: string;
  categoryName: string;
  parentCategoryName: string;
  productName: string;
  price: number;
  sizeID: string;
  colorID: string;
  sizeName: string;
  colorName: string;
  salePrice: number;
  stock: number;
  rating: number;
  totalRating: number;
  productImageUrl: string;
  createdAt: string;
  IsDeleted: boolean;
  active: boolean;
}
