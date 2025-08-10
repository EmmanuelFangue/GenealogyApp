import React, { useState, useContext } from "react";
import { login, register } from "../api/userApi";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../contexts/AuthContext";

export default function AuthForm() {
  const [isRegister, setIsRegister] = useState(false);
  const [form, setForm] = useState({
    usernameOrEmail: "",
    password: "",
    email: "",
    username: "",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();
  const { setUser } = useContext(AuthContext)!;

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      if (isRegister) {
        await register({
          username: form.username,
          email: form.email,
          password: form.password,
        });
        alert("Inscription réussie, veuillez vous connecter.");
        setIsRegister(false);
      } else {
        const res = await login(form.usernameOrEmail, form.password);
        if (res.token) {
          localStorage.setItem("jwtToken", res.token);

          // Décodage du token pour extraire les infos utilisateur
          const [, payloadBase64] = res.token.split(".");
          const payload = JSON.parse(atob(payloadBase64));
          setUser({ username: payload.username, id: payload.sub });

          navigate("/");
        }
      }
    } catch (err: any) {
      setError(err?.response?.data ?? "Erreur inconnue");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4">
      <div className="bg-white p-8 rounded shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">
          {isRegister ? "Créer un compte" : "Se connecter"}
        </h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          {isRegister && (
            <>
              <input
                name="username"
                placeholder="Nom d'utilisateur"
                value={form.username}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border rounded"
              />
              <input
                name="email"
                type="email"
                placeholder="Email"
                value={form.email}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 border rounded"
              />
            </>
          )}
          {!isRegister && (
            <input
              name="usernameOrEmail"
              placeholder="Nom d'utilisateur ou email"
              value={form.usernameOrEmail}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border rounded"
            />
          )}
          <div className="relative">
            <input
              name="password"
              type={showPassword ? "text" : "password"}
              placeholder="Mot de passe"
              value={form.password}
              onChange={handleChange}
              required
              className="w-full px-4 py-2 border rounded pr-10"
            />
            <button
              type="button"
              onClick={() => setShowPassword(!showPassword)}
              className="absolute right-2 top-2 text-sm text-gray-500"
            >
              {showPassword ? "🙈" : "👁️"}
            </button>
          </div>
          {error && <div className="text-red-500 text-sm">{error}</div>}
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
          >
            {loading
              ? "Chargement..."
              : isRegister
              ? "S'inscrire"
              : "Se connecter"}
          </button>
        </form>
        <div className="mt-4 text-center">
          <button
            onClick={() => setIsRegister(!isRegister)}
            className="text-blue-600 hover:underline text-sm"
          >
            {isRegister ? "J'ai déjà un compte" : "Créer un compte"}
          </button>
        </div>
      </div>
    </div>
  );
}
