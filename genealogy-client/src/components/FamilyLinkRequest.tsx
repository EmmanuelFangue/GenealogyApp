import React, { useState } from "react";
import apiClient from "../api/apiClient";

type Member = {
  memberId: string;
  firstName: string;
  lastName: string;
};

const RELATIONS = [
  "Parent",
  "Enfant",
  "Frère/Soeur",
  "Conjoint",
  "Autre"
];

export default function FamilyLinkRequest({ userId, members }: { userId: string; members: Member[] }) {
  const [receiverId, setReceiverId] = useState("");
  const [relationType, setRelationType] = useState(RELATIONS[0]);
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setSuccess(false);
    try {
      await apiClient.post("/FamilyLink/request", {
        requesterId: userId,
        receiverId,
        relationType
      });
      setSuccess(true);
    } catch {
      alert("Erreur lors de la demande de lien.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h3>Demander un lien familial</h3>
      <form onSubmit={handleSubmit}>
        <select value={receiverId} onChange={e => setReceiverId(e.target.value)} required>
          <option value="">Choisir un membre</option>
          {members.map(m => (
            <option key={m.memberId} value={m.memberId}>{m.firstName} {m.lastName}</option>
          ))}
        </select>
        <select value={relationType} onChange={e => setRelationType(e.target.value)}>
          {RELATIONS.map(rel => <option key={rel} value={rel}>{rel}</option>)}
        </select>
        <button type="submit" disabled={loading || !receiverId}>Envoyer la demande</button>
      </form>
      {success && <div>Demande envoyée !</div>}
    </div>
  );
}