export interface AuthTokenResponse {
  personName: string;
  email: string;
  role: string;
  token: string;
  expiration: string;
  refreshToken: string;
  refreshTokenExpirationDateTime: string;
}
