import { API } from "../../Shared/API-URL";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { ISize } from "../../Interface/Size/ISize";

const Base_API = `/Size`;

export async function GetSizes_Serv(): Promise<IResponse<ISize[]>> {
  const response = await API.get<IResponse<ISize[]>>(Base_API);
  return response.data;
}
