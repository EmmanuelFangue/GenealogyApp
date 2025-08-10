
import  { jwtDecode }  from "jwt-decode";

export function isJwtExpired(token: string): boolean {
  try {
    const decoded: any = jwtDecode(token);
    return decoded.exp * 1000 < Date.now();
  } catch {
    return true;
  }
}
