import { Injectable } from "@angular/core";
import { AuthService } from "./auth.service";
import { Router, CanActivate } from "@angular/router";

@Injectable({
  providedIn: "root"
})
export class AuthGuardService implements CanActivate {
  constructor(public auth: AuthService, public router: Router) { }
  canActivate(): boolean {
    const isAuthenticated = this.auth.isAuthenticated();
    console.log('AuthGuard: isAuthenticated =', isAuthenticated);
    
    if (!isAuthenticated) {
      console.log('AuthGuard: Redirecting to login');
      this.router.navigate(["login"]);
      return false;
    }
    else {
      console.log('AuthGuard: Allowing access');
      return true;
    }
  }
}
