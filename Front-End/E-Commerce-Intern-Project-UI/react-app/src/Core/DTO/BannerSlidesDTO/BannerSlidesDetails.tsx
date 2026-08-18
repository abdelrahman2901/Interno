import { ColorDetails } from "../Colors/ColorDetails";

export class BannerSlidesDetails {
  bannerSlideID: string = "";
  title: string = "";
  subtitle: string = "";
  cta: string = "";
  backgroundColor: ColorDetails = new ColorDetails();
  accentColor: ColorDetails = new ColorDetails();
  label: string = "";
  imageUrl: string = "";
  isActive: Boolean = false;
}
