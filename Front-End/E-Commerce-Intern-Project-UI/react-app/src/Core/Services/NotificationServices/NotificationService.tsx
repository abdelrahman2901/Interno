import axios from "axios";
import { IResponse } from "../../DTO/API-Response/IResponse";
import { NotificationModel } from "../../DTO/NotificationDTO/NotificationModel";

const Base_API = "https://localhost:7054/api/Notificaiton";
export const GetAllNotifications = async (): Promise<
  IResponse<NotificationModel[]>
> => {
  const response = await axios.get<IResponse<NotificationModel[]>>(
    `${Base_API}/GetNotification`,
  );
  return response.data;
};
export const GetAllUserNotifications = async (
  userID: string,
): Promise<IResponse<NotificationModel[]>> => {
  const response = await axios.get<IResponse<NotificationModel[]>>(
    `${Base_API}/GetUserNotifications/${userID}`,
  );
  return response.data;
};

export const MarkNotificationAsReadList = async (
  userID: string,
): Promise<IResponse<boolean>> => {
  const response = await axios.put<IResponse<boolean>>(
    `${Base_API}/MarkNotificationAsReadList/${userID}`,
  );
  return response.data;
};
export const MarkNotificationAsRead = async (
  NotificationID: string,
): Promise<IResponse<boolean>> => {
  const response = await axios.put<IResponse<boolean>>(
    `${Base_API}/MarkNotificationAsRead/${NotificationID}`,
  );
  return response.data;
};
