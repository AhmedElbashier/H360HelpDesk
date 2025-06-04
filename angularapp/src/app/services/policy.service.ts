import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
@Injectable({
  providedIn: 'root'
})
export class PolicyService {
  // Ensure this URL is correct and points to your ASP.NET Core Web API
  //private apiUrl = 'https://localhost:7012/api/v1/proxy/getPolData'; // Replace with your actual API URL

  constructor(private http: HttpClient, private common: CommonService) { }

  callPolicyApi(phoneNumber: string): Promise<any> {
    return this.http.get<any>(this.common.ProxyUrl, { params: { P_LOOKUP_TYPE: '1', P_MOBILE: phoneNumber } })
      .toPromise()
      .catch(this.handleError);
  }

  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.error);
    return Promise.reject(error.error || error);
  }
}

