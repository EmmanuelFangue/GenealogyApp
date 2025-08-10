import AuthForm from "./components/AuthForm";
import { AuthProvider } from "./contexts/AuthContext";
import Navbar from "./components/Navbar";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import MainContent from "./components/MainContent";
import RequireAuth from "./components/RequireAuth";
import ProfilePage from "./pages/ProfilePage";


function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Navbar />
        <Routes>
          {/* Page de connexion */}
          <Route path="/login" element={<AuthForm />} />

          {/* Page principale protégée */}
          <Route
            path="/"
            element={
              <RequireAuth>
                <MainContent />
              </RequireAuth>
            }
          />
          <Route
            path="/profile"
            element={
              <RequireAuth>
                <ProfilePage />
              </RequireAuth>
            }
          />

          {/* Redirection par défaut */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
