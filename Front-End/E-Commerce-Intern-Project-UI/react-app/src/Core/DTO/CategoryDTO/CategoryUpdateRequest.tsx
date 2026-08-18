export class CategoryUpdateRequest {
  categoryID: string = "";
  categoryName: string = "";
  parentCategoryID: string | null = null;
  categoryImage: File = new File([], "");
}
