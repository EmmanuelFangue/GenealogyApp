import { useContext } from "react";
import { AuthContext } from "../contexts/AuthContext";
import LogoutButton from "./LogoutButton";
import { Link } from "react-router-dom";


export default function Navbar() {
  const context = useContext(AuthContext);

  if (!context) return null;

  const { user, logout } = context;

  return (
    <nav style={{ display: "flex", justifyContent: "space-between", padding: "1rem", background: "#f5f5f5" }}>
      <div>
        <strong>GenealogyApp</strong>
      </div>
      <div>
        {user ? (
          <>
            <span style={{ marginRight: "1rem" }}>Bienvenue, {user.username}</span>
            <Link to="/profile" style={{ marginRight: "1rem" }}>Mon profil</Link>
            <LogoutButton onLogout={logout} />
          </>
        ) : (
          <span>Non connecté</span>
        )}
      </div>
    </nav>
  );
}
