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
    if (!token) return false;
    return !this.jwtHelper.isTokenExpired(token);
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
