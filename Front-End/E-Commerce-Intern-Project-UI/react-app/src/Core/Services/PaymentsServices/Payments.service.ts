import { IResponse } from "../../DTO/API-Response/IResponse";
import { PaymentModel } from "../../DTO/Payment/Payment-Model";
import { PaymentRequestModel } from "../../DTO/Payment/Payment-Request-Model";
import { API } from "../../Shared/API-URL";

const Base_API = "/Payments";

export const addPayment = async (
  newPayment: PaymentRequestModel,
): Promise<IResponse<PaymentModel>> => {
  const response = await API.post<IResponse<PaymentModel>>(
    `${Base_API}/AddNewPayment`,
    newPayment,
  );
  return response.data;
};

export const DeleteLastPayment = async (): Promise<IResponse<boolean>> => {
  const response = await API.delete<IResponse<boolean>>(
    `${Base_API}/DeleteLastPayment`,
  );

  return response.data;
};
