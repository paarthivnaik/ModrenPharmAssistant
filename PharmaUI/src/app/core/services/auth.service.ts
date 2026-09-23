import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse, UserSummary } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private readonly API_URL = 'http://localhost:5000/api/auth';
  private readonly TOKEN_KEY = 'pharm_auth_token';
  private readonly USER_KEY = 'pharm_user_profile';

  // Reactive State via Angular Signals
  currentUser = signal<UserSummary | null>(this.loadUserFromStorage());
  isAuthenticated = computed(() => !!this.currentUser());
  userRoles = computed(() => this.currentUser()?.roles ?? []);
  isAdmin = computed(() => this.userRoles().includes('Admin'));
  isStaff = computed(() => this.userRoles().includes('Staff'));

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.API_URL}/login`, request).pipe(
      tap((response) => {
        this.saveAuthSession(response, request.rememberMe);
      })
    );
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.API_URL}/register`, request);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.USER_KEY);
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY) || sessionStorage.getItem(this.TOKEN_KEY);
  }

  private saveAuthSession(response: LoginResponse, rememberMe: boolean): void {
    const user: UserSummary = {
      email: response.email,
      fullName: response.fullName,
      roles: response.roles
    };

    const storage = rememberMe ? localStorage : sessionStorage;
    storage.setItem(this.TOKEN_KEY, response.token);
    storage.setItem(this.USER_KEY, JSON.stringify(user));

    this.currentUser.set(user);
  }

  private loadUserFromStorage(): UserSummary | null {
    const raw = localStorage.getItem(this.USER_KEY) || sessionStorage.getItem(this.USER_KEY);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as UserSummary;
    } catch {
      return null;
    }
  }
}
