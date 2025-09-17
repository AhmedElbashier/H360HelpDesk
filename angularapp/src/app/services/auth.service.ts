/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";
//import { User } from "./user.service";
import { LoggerService } from "./logger.service";
import { User } from "./user.service";
export interface Role {
  id: any;
  admin: boolean;
  agent: boolean;
  backOffice: boolean;
  supervisor: boolean;
  support: boolean;
}

@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(public jwtHelper: JwtHelperService, private router: Router, private loggerService : LoggerService) { }

  public isAuthenticated(): boolean {
    const token = localStorage.getItem("token");
    const user = localStorage.getItem("user");
    
    if (!token || !user) {
      console.log('AuthService: No token or user found');
      return false;
    }
    
    // For this API, we'll use a simple token validation
    // The token is a JWT but we'll check if it exists and user exists
    try {
      const isExpired = this.jwtHelper.isTokenExpired(token);
      if (isExpired) {
        console.log('AuthService: Token is expired, clearing storage');
        this.clearAuthData();
        return false;
      }
      
      console.log('AuthService: Token is valid');
      return true;
    } catch (error) {
      console.log('AuthService: Error validating token, treating as invalid');
      this.clearAuthData();
      return false;
    }
  }

  public clearAuthData(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  public getToken(): string | null {
    return localStorage.getItem('token');
  }

  public getUser(): User | null {
    const userItem = localStorage.getItem('user');
    return userItem ? JSON.parse(userItem) : null;
  }


  public isAllowed(page: any) {
    const userItem = localStorage.getItem("user");
    const user: User | null = userItem ? JSON.parse(userItem) : null;
    //if (!user?.role[page])
    this.router.navigate(["/main/dashboard"]);
  }
}

export function tokenGetter() {
  return localStorage.getItem("token");
}
export function getUser() {
  const userItem = localStorage.getItem("user");
  const userInfo: User | null = userItem ? JSON.parse(userItem) : null;
  return userInfo;
}
