import axios from "axios";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { AuditModel } from "../../DTO/AuditDTO/AuditModel";

const Base_API = "https://localhost:7114/api/Audit";

export const GetAllAudit = async (): Promise<IResponse<AuditModel[]>> => {
  const response = await axios.get<IResponse<AuditModel[]>>(
    `${Base_API}/GetAllAudit`,
  );
  return response.data;
};
