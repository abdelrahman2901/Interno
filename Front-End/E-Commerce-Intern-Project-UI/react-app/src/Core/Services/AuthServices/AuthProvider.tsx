import { createContext, useContext, useEffect, useState } from "react";
import { IUser } from "../../Interface/User-Interfaces/IUser";
import { GetCurrentUser } from "../UserServices/UserService";
import { jwtDecode } from "jwt-decode";
import { AuthContextType } from "../../Interface/Auth/AuthContextType";

interface JWTPayload {
  sub: string;
  exp?: number;
  iat?: number;
  [key: string]: any;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<IUser | null>(null);
  const [loading, setLoading] = useState(true);

  const loadUserFromToken = async (token: string) => {
    try {
      const decoded = jwtDecode(token) as JWTPayload;
      if (!decoded.sub) {
        setUser(null);
        setLoading(false);
        return;
      }
      const res = await GetCurrentUser(decoded.sub);
      if (res.isSuccess && res.data) {
        setUser(res.data);
      } else {
        setUser(null);
      }
    } catch (err) {
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("Token");
    if (token) loadUserFromToken(token);
    else setLoading(false);
  }, []);

  const signout = () => {
    localStorage.clear();
    setUser(null);
  };

  const refreshUser = () => {
    const token = localStorage.getItem("Token");
    if (token) {
      loadUserFromToken(token);
    }
  };
  return (
    <AuthContext.Provider value={{ user, loading, signout, refreshUser }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used inside AuthProvider");
  return context;
};
