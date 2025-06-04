/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { Router } from "@angular/router";
import { CommonService } from "./common.service";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { User } from "./user.service";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class LoginService {
  data: any = {};
  constructor(
    private http: HttpClient,
    private common: CommonService,
    private router: Router
  ) { }

  login(username: string, password: string): Observable<any> {
    this.data.username = username;
    this.data.password = password;
    return this.http.post<any>(this.common.AuthUrl, this.data, { headers: this.common.headers });

  }
  //getClientInfo() {
   // return this.http.get(this.common.ClientInfoUrl);

  //}
}

