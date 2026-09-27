import { Navigate } from "react-router-dom";
import { RolesEnum } from "../API/Interno-Microservice-Client/Interno-Microservice-Client";
import { useAuth } from "../Services/AuthServices/AuthProvider";

export default function AdminAuthGuard({
  children,
}: {
  children: JSX.Element;
}) {
  const { user } = useAuth();

  if (!user?.roles?.includes(RolesEnum.Admin)) {
    return <Navigate to={"/Home"} />;
  }
  return children;
}
