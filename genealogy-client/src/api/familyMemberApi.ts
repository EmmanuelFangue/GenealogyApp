import apiClient from "./apiClient";

export const searchFamilyMembers = async (criteria: any) => {
  const res = await apiClient.get("/FamilyMember/search", { params: criteria });
  return res.data;
};

export const getFamilyMember = async (memberId: string) => {
  const res = await apiClient.get(`/FamilyMember/${memberId}`);
  return res.data;
};

export const createFamilyMember = async (payload: any) => {
  const res = await apiClient.post("/FamilyMember", payload);
  return res.data;
};

export const updateFamilyMember = async (memberId: string, payload: any) => {
  const res = await apiClient.put(`/FamilyMember/${memberId}`, payload);
  return res.data;
};