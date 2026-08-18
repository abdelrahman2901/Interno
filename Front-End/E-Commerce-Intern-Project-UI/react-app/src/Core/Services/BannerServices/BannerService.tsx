import { IResponse } from "../../DTO/API-Response/IResponse";
import { BannerSlidesDetails } from "../../DTO/BannerSlidesDTO/BannerSlidesDetails";
import { BannerSlidesRequest } from "../../DTO/BannerSlidesDTO/BannerSlidesRequest";
import { API } from "../../Shared/API-URL";

const Base_API = "/Banner";
export const GetAllBanners = async (): Promise<
  IResponse<BannerSlidesDetails[]>
> => {
  const response = await API.get<IResponse<BannerSlidesDetails[]>>(Base_API);
  return response.data;
};

export const GetBannnerByID = async (
  BannerID: string,
): Promise<IResponse<BannerSlidesDetails>> => {
  const response = await API.get<IResponse<BannerSlidesDetails>>(
    `${Base_API}/${BannerID}`,
  );
  return response.data;
};

export const CreateBanner = async (
  request: BannerSlidesRequest,
): Promise<IResponse<boolean>> => {
  const form = new FormData();
  form.append("accentColorID", request.accentColorID);
  form.append("backgroundColorID", request.backgroundColorID);
  form.append("bannerImage", request.bannerImage);
  form.append("CTA", request.CTA);
  form.append("label", request.label);
  form.append("subtitle", request.subtitle);
  form.append("title", request.title);

  const response = await API.post<IResponse<boolean>>(Base_API, form);
  return response.data;
};

export const UpdateBanner = async (
  request: BannerSlidesRequest,
): Promise<IResponse<boolean>> => {
  const form = new FormData();
  form.append("BannerSlideID", request.BannerSlideID!);
  form.append("accentColorID", request.accentColorID);
  form.append("backgroundColorID", request.backgroundColorID);
  form.append("bannerImage", request.bannerImage);
  form.append("CTA", request.CTA);
  form.append("label", request.label);
  form.append("subtitle", request.subtitle);
  form.append("title", request.title);

  const response = await API.put<IResponse<boolean>>(Base_API, form);
  return response.data;
};

export const DeleteBanner = async (
  BannerID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/DeleteBanner/${BannerID}`,
  );
  return response.data;
};

export const ToggleBannerActiviation = async (
  BannerID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/ToggleBannerActiviation/${BannerID}`,
  );
  return response.data;
};
