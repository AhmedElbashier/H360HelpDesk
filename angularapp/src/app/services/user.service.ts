/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { CommonService } from "../services/common.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MessageService } from "primeng/api";
import { BehaviorSubject, EMPTY, Observable, filter, of, switchMap, timer } from "rxjs";
import { Role } from "./auth.service";

export interface User {
  id?: any;
  user_Id?: any;
  username?: any;
  email?: any;
  password?: any;
  isAdministrator?: any;
  isAgent?: any;
  isSuperVisor?: any;
  isBackOffice?: any;
  firstname?: any;
  lastname?: any;
  department_Id?: any;
  department_Name?: any;
  phone?: any;
  disabled?: any;
  lastSeen?: any;
  ipAddress?: any;
  hostName?: any;
  lastPasswordChange?: any;
  lastLogoutDate?: any;
  darkMode?: any;
  status?: any;
  deleted?: any;
}

@Injectable({
  providedIn: "root"
})

export class UserService {
  data: any;
  private lastInteractionTimestamp: number = Date.now();
  private userActivitySubject = new BehaviorSubject<string>("Online");

  constructor(
    private http: HttpClient,
    private common: CommonService,
    private msg: MessageService,
  ) {
    //timer(0, 60000) // Check every 1 minute
    //  .pipe(
    //    switchMap(() => {
    //      const userStatus = this.checkUserInactivity();

    //      var user = JSON.parse(localStorage.getItem("user") || "{}") as User;
    //      if (user != null) {
    //        return this.updateStatus(user.user_Id, userStatus);
    //      }
    //      else {
    //        return of(null);
    //      }
    //    }),
    //    filter(value => value !== null)
    //  )
    //  .subscribe();
}

  getAllUsers(type: any = null): Observable<any[]> {
    return this.http.get<User[]>(this.common.UserUrl, { headers: this.common.headers });
  }
  getUsers(type: any = null): Observable<any[]> {
    return this.http.get<User[]>(this.common.UserUrl, { headers: this.common.headers });
  }
  getAgentUsers(type: any = null): Observable<any[]> {
    return this.http.get<User[]>(this.common.UserUrl + "/Agents", { headers: this.common.headers });
  }
  getBackOfficeUsers(type: any = null): Observable<any[]> {
    return this.http.get<User[]>(this.common.UserUrl + "/BackOffice", { headers: this.common.headers });
  }
  getUserbyId(id: any = null): Observable<any> {
    return this.http.get<User>(this.common.UserUrl + "/" + id, { headers: this.common.headers })
  }
  getUser(username: any = null): Promise<any> {
    return this.http.get<User>(this.common.UserByNameUrl + "/" + username, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addUser(user: any): Promise<any> {
    return this.http.post<any>(this.common.UserUrl, user, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteUser(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.UserUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
 
  editUser(user: any): Promise<any> {
    return this.http.put<any>(this.common.UserUrl + "/" + user.id, user, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  resetUser(id: any, password:any): Promise<any> {
    return this.http.put<any>(this.common.UserUrl + "/reset-password/" + id+"/"+ password, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedUsers(selectedIds: string[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        ...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.UserUrl + "/deleteselected", options).toPromise();
  }

  getUserLastSeen(userID: number): Observable<any> {
    return this.http.put(`/api/v1/users/${userID}/lastseen`, {}, { headers: this.common.headers });
  }

  updateLastSeen(userID: number, lastSeen: Date): Observable<any> {
    const url = `${this.common.UserUrl}/${userID}/lastseen`;
    const body = { lastSeen: lastSeen.toISOString() };

    return this.http.put(url, body, { headers: this.common.headers });
  }
  updateLastLogout(userID: number, lastSeen: Date) {
    const url = `${this.common.UserUrl}/${userID}/lastlogoutdate`;
    const body = { lastSeen: lastSeen.toISOString() }; 

    return this.http.put(url, body, { headers: this.common.headers });
  }
  updateStatus(userID: Number, status: string) {
    const url = `${this.common.UserUrl}/${userID}/status`;
    const body = { status: status }; 
    var user = JSON.parse(localStorage.getItem("user") || "{}") as User;
    return this.http.put(url, body, { headers: this.common.headers });
  }
  triggerUserActivity(status: string) {
    this.lastInteractionTimestamp = Date.now();
    this.userActivitySubject.next(status);
  }
  checkUserInactivity(): string {
    const now = Date.now();
    const inactivityDuration = now - this.lastInteractionTimestamp;
    if (inactivityDuration >= 8 * 60 * 1000) {
      this.userActivitySubject.next("Offline");
    }
    else if (inactivityDuration >= 5 * 60 * 1000) {
      this.userActivitySubject.next("Away");
    }
    else {
      this.userActivitySubject.next("Online");
    }

    return this.userActivitySubject.getValue();
  }

  getCountUsers(type: any = null): Observable<any> {
    return this.http.get<any>(this.common.UserUrl + "/CountAllUsers");
  }
  getCountAdmins(type: any = null): Observable<any> {
    return this.http.get<any>(this.common.UserUrl + "/CountAdmins");
  }
  getCountAgents(type: any = null): Observable<any> {
    return this.http.get<any>(this.common.UserUrl + "/CountAgents");
  }
  getCountSupervisors(type: any = null): Observable<any> {
    return this.http.get<any>(this.common.UserUrl + "/CountBackOffices");
  }
  getCountBackOffice(type: any = null): Observable<any> {
    return this.http.get<any>(this.common.UserUrl + "/CountSupervisors");
  }
}
