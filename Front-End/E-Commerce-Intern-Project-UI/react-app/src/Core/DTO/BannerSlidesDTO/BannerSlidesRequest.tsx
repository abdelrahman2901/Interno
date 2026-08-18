export class BannerSlidesRequest {
  BannerSlideID: string | null = null;
  title: string = "";
  subtitle: string = "";
  CTA: string = "";
  backgroundColorID: string = "";
  accentColorID: string = "";
  label: string = "";
  bannerImage: File = new File([], "");
}
