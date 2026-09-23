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

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  fullName: string;
  contactNo: string;
  address: string;
  role?: string;
}

export interface RegisterResponse {
  userId: string;
  email: string;
  fullName: string;
  roles: string[];
  message: string;
}

export interface UserSummary {
  email: string;
  fullName: string;
  roles: string[];
}
