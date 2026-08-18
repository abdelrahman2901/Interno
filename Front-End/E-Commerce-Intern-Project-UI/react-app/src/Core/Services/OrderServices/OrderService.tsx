import { IResponse } from "../../DTO/API-Response/IResponse";
import { OrderDetails } from "../../DTO/OrderDTO/OrderDetails";
import { OrderRequest } from "../../DTO/OrderDTO/OrderRequest";
import { OrderResponse } from "../../DTO/OrderDTO/OrderResponse";
import { UpdateOrderRequest } from "../../DTO/OrderDTO/UpdateOrderRequest";
import { UpdateOrderStatusRequest } from "../../DTO/OrderDTO/UpdateOrderStatus";
import { API } from "../../Shared/API-URL";

const Base_API = "/Orders";

export const GetAllOrders = async (): Promise<IResponse<OrderDetails[]>> => {
  const response = await API.get<IResponse<OrderDetails[]>>(
    `${Base_API}/GetAllOrders`,
  );
  return response.data;
};
export const GetOrder = async (
  orderID: string,
): Promise<IResponse<OrderDetails>> => {
  const response = await API.get<IResponse<OrderDetails>>(
    `${Base_API}/GetOrder/${orderID}`,
  );

  return response.data;
};
export const GetAllUserOrders = async (
  UserID: string,
): Promise<IResponse<OrderDetails[]>> => {
  const response = await API.get<IResponse<OrderDetails[]>>(
    `${Base_API}/GetAllUserOrders/${UserID}`,
  );
  return response.data;
};
export const CreateOrder = async (
  request: OrderRequest,
): Promise<IResponse<OrderResponse>> => {
  console.log(request);

  const response = await API.post<IResponse<OrderResponse>>(Base_API, request);
  return response.data;
};
export const UpdateOrderStatus = async (
  request: UpdateOrderStatusRequest,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/UpdateOrderStatus`,
    request,
  );
  return response.data;
};
export const UpdateOrder = async (
  request: UpdateOrderRequest,
): Promise<IResponse<boolean>> => {
  const response = await API.put<IResponse<boolean>>(
    `${Base_API}/UpdateOrder`,
    request,
  );
  return response.data;
};
