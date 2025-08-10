import apiClient from "./apiClient";

// Upload photo
export const uploadPhoto = async (memberId: string, file: File) => {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("memberId", memberId);
  const res = await apiClient.post("/Photo/add", formData, {
    headers: { "Content-Type": "multipart/form-data" }
  });
  return res.data;
};

// Liste des photos d'un membre
export const getMemberPhotos = async (memberId: string) => {
  const res = await apiClient.get(`/Photo/member/${memberId}`);
  return res.data;
};

export const getPhotos = async (memberId: string) => {
  const res = await apiClient.get(`/Photo/${memberId}`);
  return res.data;
};

export const setProfilePhoto = async (memberId: string, photoId: string) => {
  await apiClient.put(`/Photo/${memberId}/set-profile/${photoId}`);
};

