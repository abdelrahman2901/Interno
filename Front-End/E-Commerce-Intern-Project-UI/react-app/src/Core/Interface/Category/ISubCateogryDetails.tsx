import { ICategory } from "./ICategory";

export interface ISubCategoryDetails {
  categoryID: string;
  categoryName: string;
  categoryImageUrl: string | null;
  parentCategoryID: string | null;
  subCategories: ICategory[];
}
