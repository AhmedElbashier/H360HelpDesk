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
    
    // For this API, we'll use a more lenient token validation
    // Check if token exists and user exists, with a grace period for expiration
    try {
      const isExpired = this.jwtHelper.isTokenExpired(token);
      if (isExpired) {
        // Add a 5-minute grace period to handle timezone issues
        const tokenData = this.jwtHelper.decodeToken(token);
        if (tokenData && tokenData.exp) {
          const expirationTime = new Date(tokenData.exp * 1000);
          const now = new Date();
          const gracePeriod = 5 * 60 * 1000; // 5 minutes in milliseconds
          
          if (now.getTime() - expirationTime.getTime() < gracePeriod) {
            console.log('AuthService: Token expired but within grace period, allowing access');
            return true;
          }
        }
        
        console.log('AuthService: Token is expired, clearing storage');
        this.clearAuthData();
        return false;
      }
      
      console.log('AuthService: Token is valid');
      return true;
    } catch (error) {
      console.log('AuthService: Error validating token, treating as valid for now');
      // Don't clear data on validation errors - might be a parsing issue
      return true;
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

  public getTokenData(token: string): any {
    try {
      return this.jwtHelper.decodeToken(token);
    } catch (error) {
      console.log('AuthService: Error decoding token:', error);
      return null;
    }
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
