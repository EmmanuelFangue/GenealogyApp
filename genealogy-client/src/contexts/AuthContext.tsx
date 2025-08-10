import React, { createContext, useState, useEffect } from "react";
import type { ReactNode } from "react";
import { isJwtExpired } from "../utils/jwt";

type User = {
  id: string;
  username: string;
};

type AuthContextType = {
  user: User | null;
  setUser: React.Dispatch<React.SetStateAction<User | null>>;
  logout: () => void;
};

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);

  const logout = () => {
    localStorage.removeItem("jwtToken");
    setUser(null);
    // Ne pas rediriger ici — laisse RequireAuth gérer la navigation
  };

  // Initialisation : vérifie le token et extrait l'utilisateur
  useEffect(() => {
    const token = localStorage.getItem("jwtToken");
    if (token && !isJwtExpired(token)) {
      try {
        const [, payloadBase64] = token.split(".");
        const payload = JSON.parse(atob(payloadBase64));
        setUser({ username: payload.username, id: payload.sub });
      } catch {
        setUser(null);
      }
    } else {
      setUser(null);
    }
  }, []);

  // Vérification périodique du token
  useEffect(() => {
    const interval = setInterval(() => {
      const token = localStorage.getItem("jwtToken");
      if (!token || isJwtExpired(token)) {
        logout();
      }
    }, 60 * 1000); // toutes les minutes
    return () => clearInterval(interval);
  }, []);

  return (
    <AuthContext.Provider value={{ user, setUser, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
