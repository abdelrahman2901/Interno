export class CategoryRequest {
  categoryName: string = "";
  parentCategoryID: string | null = null;
  categoryImage: File = new File([], "");
}
