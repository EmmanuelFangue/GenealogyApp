import apiClient from "./apiClient";

export const searchMembers = async (criteria: any) => {
    // Ajoute clientId si nécessaire
    const params = { ...criteria, clientId: localStorage.getItem('clientId') };
    const res = await apiClient.get('/FamilyMember/search', { params });
    return res.data;
};