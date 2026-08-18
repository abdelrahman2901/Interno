import { Navigate } from "react-router-dom";
import { useAuth } from "../Services/AuthServices/AuthProvider";

export default function AdminAuthGuard({
  children,
}: {
  children: JSX.Element;
}) {
  const { user } = useAuth();

  if (user?.role !== "Admin") {
    return <Navigate to={"/Home"} />;
  }
  return children;
}
