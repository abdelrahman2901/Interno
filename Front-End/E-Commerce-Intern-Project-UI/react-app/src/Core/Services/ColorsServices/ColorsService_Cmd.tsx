import { IResponse } from "../../DTO/API-Response/IResponse";
import { IColors } from "../../Interface/Colors/IColors";
import { ColorRequest } from "../../DTO/Colors/ColorRequest";
import { API } from "../../Shared/API-URL";
const Base_API = `/Color`;

export async function AddColor_Serv(
  request: ColorRequest,
): Promise<IResponse<IColors>> {
  const response = await API.post<IResponse<IColors>>(Base_API, request);
  return response.data;
}
