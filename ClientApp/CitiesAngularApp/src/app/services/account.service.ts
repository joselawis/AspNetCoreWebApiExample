import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterUser } from '../models/register-user';
import { LoginUser } from '../models/login-user';
import { AuthenticationResponse } from '../models/authentication-response';

const API_BASE_URL: string = 'https://localhost:7122/api/v1/account/';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  public currentUserName: string | null = null;

  constructor(private httpClient: HttpClient) {}

  public postRegister(
    registerUser: RegisterUser,
  ): Observable<AuthenticationResponse> {
    return this.httpClient.post<AuthenticationResponse>(
      `${API_BASE_URL}register`,
      registerUser,
    );
  }

  public postLogin(loginUser: LoginUser): Observable<AuthenticationResponse> {
    return this.httpClient.post<AuthenticationResponse>(
      `${API_BASE_URL}login`,
      loginUser,
    );
  }

  public getLogout(): Observable<string> {
    return this.httpClient.get<string>(`${API_BASE_URL}logout`);
  }

  public isAuthentified(): boolean {
    return (
      this.currentUserName !== null &&
      this.currentUserName !== '' &&
      this.currentUserName !== undefined
    );
  }

  public postGenerateNewToken(): Observable<any> {
    var token = localStorage['token'];
    var refreshToken = localStorage['refreshToken'];

    return this.httpClient.post<any>(`${API_BASE_URL}generate-new-jwt-token`, {
      token: token,
      refreshToken: refreshToken,
    });
  }
}
