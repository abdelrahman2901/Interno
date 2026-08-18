import { IResponse } from "../../DTO/API-Response/IResponse";
import { IColors } from "../../Interface/Colors/IColors";
import { API } from "../../Shared/API-URL";
const Base_API = `/Color`;

export async function GetColors_Serv(): Promise<IResponse<IColors[]>> {
  const response = await API.get<IResponse<IColors[]>>(Base_API);
  return response.data;
}
