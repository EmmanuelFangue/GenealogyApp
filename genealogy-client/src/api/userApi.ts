import apiClient from "./apiClient";

export const getUser = async (userId: string) => {
  const res = await apiClient.get(`/User/${userId}`);
  return res.data;
};

export const updateUser = async (userId: string, payload: any) => {
  const res = await apiClient.put(`/User/${userId}`, payload);
  return res.data;
};

export const login = async (usernameOrEmail: string, password: string) => {
    const res = await apiClient.post('/User/login', { usernameOrEmail, password });
    if (res.data.token) {
      localStorage.setItem('jwtToken', res.data.token);
    }
    return res.data;
};
  
export const register = async (payload: any) => {
    const res = await apiClient.post('/User/register', payload);
    return res.data;
};
