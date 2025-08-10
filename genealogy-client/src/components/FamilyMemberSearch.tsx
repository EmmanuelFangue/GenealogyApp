import React, { useState } from "react";
import apiClient from "../api/apiClient";

type Member = {
  memberId: string;
  firstName: string;
  lastName?: string;
  birthDate?: string;
  relationToUser?: string;
  gender?: string;
};

export default function FamilyMemberSearch() {
  const [criteria, setCriteria] = useState({
    firstName: "",
    lastName: "",
    relationToUser: "",
    gender: "",
  });
  const [results, setResults] = useState<Member[]>([]);
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setCriteria({ ...criteria, [e.target.name]: e.target.value });
  };

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      const res = await apiClient.get("/FamilyMember/search", { params: criteria });
      setResults(res.data);
    } catch (err) {
      alert("Erreur lors de la recherche");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Recherche avancée de membres</h2>
      <form onSubmit={handleSearch}>
        <input name="firstName" placeholder="Prénom" value={criteria.firstName} onChange={handleChange} />
        <input name="lastName" placeholder="Nom" value={criteria.lastName} onChange={handleChange} />
        <input name="relationToUser" placeholder="Lien familial" value={criteria.relationToUser} onChange={handleChange} />
        <select name="gender" value={criteria.gender} onChange={handleChange}>
          <option value="">Genre</option>
          <option value="Homme">Homme</option>
          <option value="Femme">Femme</option>
        </select>
        <button type="submit" disabled={loading}>Rechercher</button>
      </form>
      <ul>
        {results.map(m => (
          <li key={m.memberId}>
            {m.firstName} {m.lastName} {m.relationToUser && `- ${m.relationToUser}`}
          </li>
        ))}
      </ul>
    </div>
  );
}