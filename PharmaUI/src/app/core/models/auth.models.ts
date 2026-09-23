export interface LoginRequest {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface LoginResponse {
  token: string;
  expiration: string;
  email: string;
  fullName: string;
  roles: string[];
  defaultRedirectUrl: string;
}

export interface UserSummary {
  email: string;
  fullName: string;
  roles: string[];
}
