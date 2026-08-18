import { IUser } from "../User-Interfaces/IUser";

export interface AuthContextType {
  user: IUser | null;
  loading: boolean;
  signout: () => void;
  refreshUser: () => void;
}
