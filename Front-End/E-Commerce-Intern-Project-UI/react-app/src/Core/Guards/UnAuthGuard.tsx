import { Navigate } from "react-router-dom";

export default function UnAuthGuard({ children }: { children: JSX.Element }) {
  const token = localStorage.getItem("Token");
  console.log(token);

  if (!token) {
    return <Navigate to={"/Home/Login"} />;
  }
  return children;
}
