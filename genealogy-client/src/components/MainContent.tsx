import { useContext } from "react";
import { AuthContext } from "../contexts/AuthContext";

export default function MainContent() {
  const context = useContext(AuthContext);

  if (!context) {
    // Ici tu peux afficher un loader, une erreur, ou rediriger
    return <div>Chargement du contexte utilisateur...</div>;
  }

  const { user } = context;

  return (
    <main>
      <h1>GenealogyApp</h1>
      {user
        ? (
          <section>
            <h2>Bienvenue, {user.username} !</h2>
            {/* Ici tu pourras insérer l’arbre familial, les membres, etc. */}
            <p>Commencez à explorer votre arbre généalogique.</p>
            {/* Ex : <FamilyTree userId={user.id} /> */}
          </section>
        ) : (
          <section>
            <h2>Bienvenue sur GenealogyApp</h2>
            <p>Connectez-vous pour afficher et gérer votre famille.</p>
            {/* Tu peux ajouter un bouton de login ou une redirection */}
          </section>
        )
      }
    </main>
  );
}