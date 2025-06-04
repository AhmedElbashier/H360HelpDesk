/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { CommonService } from "../services/common.service";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MessageService } from "primeng/api";
import { BehaviorSubject, EMPTY, Observable, filter, of, switchMap, timer } from "rxjs";
import { Role } from "./auth.service";

export interface AgentReport {
  departmentID?: any;
  categoryID?: any;
  statusID?: any;
  levelID?: any;
  userID?: any;
  startDate?: any;
  endDate?: any;
}

export interface SuperVisorReport {
  departmentID?: any;
  categoryID?: any;
  statusID?: any;
  levelID?: any;
  userID?: any;
  assignedToUserID?: any;
  startDate?: any;
  endDate?: any;
}

@Injectable({
  providedIn: "root"
})

export class ReportService {
  constructor(
    private http: HttpClient,
    private common: CommonService,
    private msg: MessageService,
  ) {}

  getAgentReport(reportData: any): Promise<any> {
    return this.http.post<any>(this.common.ReportUrl + "/agent", reportData, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeReport(reportData: any): Promise<any> {
    return this.http.post<any>(this.common.ReportUrl + "/backoffice", reportData, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorReport(reportData: any): Promise<any> {
    return this.http.post<any>(this.common.ReportUrl + "/supervisor", reportData, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  
}
