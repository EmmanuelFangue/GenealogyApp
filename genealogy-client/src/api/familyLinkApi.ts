import apiClient from "./apiClient";

// Liste des liens pour un utilisateur
export const getUserFamilyLinks = async (userId: string) => {
  const res = await apiClient.get(`/FamilyLink/user/${userId}`);
  return res.data;
};

// Demander un lien familial
export const requestFamilyLink = async (requesterId: string, receiverId: string, relationType: string) => {
  const res = await apiClient.post("/FamilyLink/request", { requesterId, receiverId, relationType });
  return res.data;
};

// Accepter/refuser une demande
export const acceptFamilyLink = async (linkId: string, userId: string) => {
  const res = await apiClient.post("/FamilyLink/accept", { linkId, userId });
  return res.data;
};
export const rejectFamilyLink = async (linkId: string, userId: string) => {
  const res = await apiClient.post("/FamilyLink/reject", { linkId, userId });
  return res.data;
};

// Liste des demandes en attente
export const getPendingFamilyLinks = async (userId: string) => {
  const res = await apiClient.get(`/FamilyLink/pending/${userId}`);
  return res.data;
};