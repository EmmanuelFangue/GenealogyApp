import { useEffect, useState } from "react";
import apiClient from "../api/apiClient";

type FamilyLink = {
  linkId: string;
  requesterId: string;
  receiverId: string;
  relationType: string;
  status: string;
  createdAt?: string;
  confirmedAt?: string;
};

export default function FamilyLinkList({ userId }: { userId: string }) {
  const [links, setLinks] = useState<FamilyLink[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchLinks = async () => {
      setLoading(true);
      try {
        const res = await apiClient.get(`/FamilyLink/user/${userId}`);
        setLinks(res.data);
      } catch {
        setLinks([]);
      } finally {
        setLoading(false);
      }
    };
    fetchLinks();
  }, [userId]);

  return (
    <div>
      <h2>Liens familiaux</h2>
      {loading && <div>Chargement...</div>}
      <ul>
        {links.map(link => (
          <li key={link.linkId}>
            {link.relationType} avec {link.requesterId === userId ? link.receiverId : link.requesterId} 
            ({link.status})
            {link.status === "Pending" && <span> (en attente de confirmation)</span>}
            {link.status === "Confirmed" && link.confirmedAt && <span> (Confirmé le {new Date(link.confirmedAt).toLocaleDateString()})</span>}
          </li>
        ))}
      </ul>
    </div>
  );
}