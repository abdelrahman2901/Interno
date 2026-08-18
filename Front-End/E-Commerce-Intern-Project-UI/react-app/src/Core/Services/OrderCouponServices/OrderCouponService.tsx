import { IResponse } from "../../DTO/API-Response/IResponse";
import { OrderCouponRequest } from "../../DTO/OrdeCouponDTO/OrderCouponRequest";
import { OrderCouponModel } from "../../DTO/OrdeCouponDTO/OrderCouponResponse";
import { API } from "../../Shared/API-URL";

const Base_API = "/OrderCoupon";
export const GetAllCoupons = async (): Promise<
  IResponse<OrderCouponModel[]>
> => {
  const response = await API.get<IResponse<OrderCouponModel[]>>(Base_API);
  return response.data;
};
export const GetCoupon = async (
  couponID: string,
): Promise<IResponse<OrderCouponModel>> => {
  const response = await API.get<IResponse<OrderCouponModel>>(
    `${Base_API}/${couponID}`,
  );
  return response.data;
};
export const GetCouponByCode = async (
  couponCode: string,
): Promise<IResponse<OrderCouponModel>> => {
  console.log(couponCode);

  const response = await API.get<IResponse<OrderCouponModel>>(
    `${Base_API}/GetCouponByCode`,
    {
      params: { couponCode },
    },
  );
  return response.data;
};

export const CreateNewCoupon = async (
  request: OrderCouponRequest,
): Promise<IResponse<boolean>> => {
  const response = await API.post<IResponse<boolean>>(Base_API, request);
  return response.data;
};

export const UpdateCoupon = async (
  request: OrderCouponModel,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/UpdateCoupon`,
    request,
  );
  return response.data;
};
export const UpdateCouponActivation = async (
  couponID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/UpdateCouponActivation/${couponID}`,
  );
  return response.data;
};

export const DeleteCoupon_serv = async (
  couponID: string,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/DeleteCoupon/${couponID}`,
  );
  return response.data;
};
