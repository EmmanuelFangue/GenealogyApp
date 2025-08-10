import { useEffect, useState } from "react";
import apiClient from "../api/apiClient";

type FamilyLink = {
  linkId: string;
  requesterId: string;
  receiverId: string;
  relationType: string;
  status: string;
};

export default function FamilyLinkActions({ userId }: { userId: string }) {
  const [pendingLinks, setPendingLinks] = useState<FamilyLink[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchPending = async () => {
      setLoading(true);
      try {
        // Récupère les demandes reçues
        const res = await apiClient.get(`/FamilyLink/pending/${userId}`);
        setPendingLinks(res.data);
      } catch {
        setPendingLinks([]);
      } finally {
        setLoading(false);
      }
    };
    fetchPending();
  }, [userId]);

  const handleAction = async (linkId: string, accept: boolean) => {
    setLoading(true);
    try {
      await apiClient.post(`/FamilyLink/${accept ? "accept" : "reject"}`, { linkId, userId });
      setPendingLinks(pendingLinks.filter(l => l.linkId !== linkId));
    } catch {
      alert("Erreur lors de l'action.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h3>Demandes de lien à valider</h3>
      {loading && <div>Chargement...</div>}
      <ul>
        {pendingLinks.map(link => (
          <li key={link.linkId}>
            {link.relationType} avec {link.requesterId}
            <button onClick={() => handleAction(link.linkId, true)} disabled={loading}>Accepter</button>
            <button onClick={() => handleAction(link.linkId, false)} disabled={loading}>Refuser</button>
          </li>
        ))}
        {pendingLinks.length === 0 && <div>Aucune demande en attente.</div>}
      </ul>
    </div>
  );
}