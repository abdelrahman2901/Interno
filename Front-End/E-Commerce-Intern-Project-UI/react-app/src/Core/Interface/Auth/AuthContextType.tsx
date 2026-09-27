import { AuthUserDetailsDTO } from "../../API/Interno-Microservice-Client/Interno-Microservice-Client";

export interface AuthContextType {
  user: AuthUserDetailsDTO | null;
  loading: boolean;
  signout: () => void;
  refreshUser: () => void;
}
