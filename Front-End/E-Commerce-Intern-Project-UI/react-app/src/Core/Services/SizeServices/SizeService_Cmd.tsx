import { SizeRequest } from "../../DTO/Size/SizeRequest";
import { API } from "../../Shared/API-URL";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { ISize } from "../../Interface/Size/ISize";

const Base_API = `/Size`;

export async function AddSize_Serv(
  Request: SizeRequest,
): Promise<IResponse<ISize>> {
  const response = await API.post<IResponse<ISize>>(Base_API, Request);
  return response.data;
}
