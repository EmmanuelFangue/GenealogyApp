
import { useContext } from "react";
import { AuthContext } from "../contexts/AuthContext";

export default function LogoutButton({ onLogout }: { onLogout?: () => void }) {
  const context = useContext(AuthContext);

  const handleLogout = () => {
    if (onLogout) {
      onLogout(); // appel externe si fourni
    } else if (context?.logout) {
      context.logout(); // appel du contexte
    } else {
      // fallback
      localStorage.removeItem("jwtToken");
      window.location.href = "/login";
    }
  };

  return (
    <button onClick={handleLogout}>
      Se déconnecter
    </button>
  );
}

