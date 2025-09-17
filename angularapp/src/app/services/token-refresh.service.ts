import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from './common.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TokenRefreshService {
  private refreshInProgress = false;

  constructor(
    private http: HttpClient,
    private common: CommonService,
    private auth: AuthService
  ) {}

  async refreshTokenIfNeeded(): Promise<boolean> {
    if (this.refreshInProgress) {
      return false;
    }

    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');

    if (!token || !user) {
      return false;
    }

    try {
      // Check if token is close to expiration (within 10 minutes)
      const tokenData = this.auth.getTokenData(token);
      if (tokenData && tokenData.exp) {
        const expirationTime = new Date(tokenData.exp * 1000);
        const now = new Date();
        const timeUntilExpiry = expirationTime.getTime() - now.getTime();
        const refreshThreshold = 10 * 60 * 1000; // 10 minutes

        if (timeUntilExpiry < refreshThreshold && timeUntilExpiry > 0) {
          console.log('TokenRefreshService: Token is close to expiration, refreshing...');
          return await this.refreshToken();
        }
      }
    } catch (error) {
      console.log('TokenRefreshService: Error checking token expiration:', error);
    }

    return false;
  }

  private async refreshToken(): Promise<boolean> {
    if (this.refreshInProgress) {
      return false;
    }

    this.refreshInProgress = true;

    try {
      const user = JSON.parse(localStorage.getItem('user') || '{}');
      
      // For now, we'll just re-login with stored credentials
      // In a real app, you'd have a refresh token endpoint
      const response = await this.http.post<any>(this.common.AuthUrl, {
        username: user.username || 'admin',
        password: 'admin' // This should be stored securely or use refresh token
      }, { headers: this.common.headers }).toPromise();

      if (response && response.token) {
        localStorage.setItem('token', response.token);
        localStorage.setItem('user', JSON.stringify(response.user));
        console.log('TokenRefreshService: Token refreshed successfully');
        return true;
      }
    } catch (error) {
      console.log('TokenRefreshService: Failed to refresh token:', error);
    } finally {
      this.refreshInProgress = false;
    }

    return false;
  }
}
