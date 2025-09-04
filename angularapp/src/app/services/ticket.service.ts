import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export interface Ticket {
  id?: string;
  ticketID?: string;
  CustomerID?: string;
  indice?: string;
  userID?: string;
  categoryID?: string;
  subCategoryID?: string;
  departmentID?: string;
  channelID?: string;
  startDate?: Date;
  resolvedDate?: Date | null;
  closedDate?: Date | null;
  assingedToUserID?: number | null;
  assingedToBackOfficeID?: number | null;
  subject?: string;
  body?: string;
  statusID?: number;
  priority?: string;
  escalationLevel?: string;
  updateByUser?: string;
  dueDate?: Date | null;
  slaDate?: Date | null;
  emailAlert?: boolean;
  flag?: boolean;
  mobile?: string;
  email?: string;
  customerName?: string;
  companyID?: string;
  smsAlert?: boolean;
  requestID?: string;
  departmentReply?: string;
  referenceType?: string;
  referenceNumber?: string;
}


export interface Comment {
  commentID?: number;
  ticketID?: string;
  userID?: string;
  body?: string;
  commentDate?: string;
  ticketFlag?: boolean;
  children?: Comment[];
}
export interface SubTicket {
  id?: number;
  parent_Id?: number;
  child_Id?: number;
}
@Injectable({
  providedIn: 'root'
})
export class TicketService {
  constructor(private http: HttpClient,
    private common: CommonService,
    private msg: MessageService,) { }

  getTickets(type: any = null): Observable<any[]> {
    return this.http.get<Ticket[]>(this.common.TicketUrl, { headers: this.common.headers });
  }
  getDeletedTickets(type: any = null): Observable<any[]> {
    return this.http.get<Ticket[]>(this.common.TicketUrl + "/DeletedTicket", { headers: this.common.headers });
  }
  getTicket(TicketId: any = null): Promise<any> {
    return this.http.get<Ticket>(this.common.TicketUrl + "/" + TicketId, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getlastTicketId(type: any = null): Observable<any[]> {
    return this.http.get<any[]>(this.common.TicketUrl + "/lastCreatedTicket", { headers: this.common.headers });
  }
  addTicket(Ticket: any): Promise<any> {
    return this.http.post<any>(this.common.TicketUrl, Ticket, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  editTicket(Ticket: Ticket): Promise<any> {
    return this.http.put<any>(this.common.TicketUrl + "/" + Ticket.id, Ticket, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteTicket(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.TicketUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedTicket(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.TicketUrl + "/DeleteSelected", options).toPromise();
  }
  uploadTicketWithFile(ticketData: any, file: any) {
    const formData = new FormData();
    // Match API [FromForm] model keys (case-insensitive but align for clarity)
    formData.append('Ticket', new Blob([JSON.stringify(ticketData)], { type: 'application/json' }));
    formData.append('File', file, file.name);
    return this.http.post<any>(this.common.TicketUrl + "/UploadHdTicketsWithFile", formData, { headers: this.common.headers });
  }

  getComments(type: any = null): Observable<any[]> {
    return this.http.get<Ticket[]>(this.common.TicketUrl, { headers: this.common.headers });
  }
  getRelatedComments(type: any = null): Observable<any[]> {
    return this.http.get<Ticket[]>(this.common.CommentUrl + "/RelatedTicketComments/" + type, { headers: this.common.headers });
  }
  getDeletedComments(type: any = null): Observable<any[]> {
    return this.http.get<Ticket[]>(this.common.CommentUrl + "/DeletedTicket", { headers: this.common.headers });
  }
  getComment(TicketId: any = null): Promise<any> {
    return this.http.get<Ticket>(this.common.CommentUrl + "/" + TicketId, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getlastCommentId(type: any = null): Observable<any[]> {
    return this.http.get<any[]>(this.common.CommentUrl + "/lastCreatedTicket", { headers: this.common.headers });
  }
  addComment(Ticket: any): Promise<any> {
    return this.http.post<any>(this.common.CommentUrl, Ticket, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteComment(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.CommentUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedComment(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.TicketUrl + "/DeleteSelected", options).toPromise();
  }


  getTotalTicketsCount(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/TotalCount/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTotalICommentsCount(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.CommentUrl + "/TotalCount/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTotalNew(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/TotalNew/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTotalOpened(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/TotalOpened/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTotalResolved(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/TotalResolved/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTotalReopen(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/TotalReOpened/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTotalClosed(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/TotalClosed/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTotalAssignedtoMe(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/TotalCount/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getNotifications(userID: any = null): Promise<any> {
    return this.http.get<any>(this.common.TicketUrl + "/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getRecentTickets(userID: any = null): Promise<any> {
    return this.http.get<Ticket>(this.common.TicketUrl + "/NewestOpened/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();

  }
  getuserTicketsDashboard(userID: any = null): Promise<any> {
    return this.http.get<Ticket>(this.common.TicketUrl + "/UserHdTickets/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  getSubRelatedTicket(type: any): Promise<any> {
    return this.http.get<SubTicket>(this.common.SubTicketUrl + "/" + type, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();

  }
  getTicketsByPhone(phone: string): Promise<any> {
    return this.http.get<any>(`${this.common.TicketUrl}/phone/${phone}`, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  closeTicket(Ticket: Ticket): Promise<any> {
    return this.http.put<any>(this.common.TicketUrl + "/TicketClose/" + Ticket.id, Ticket, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  resolveTicket(Ticket: Ticket): Promise<any> {
    return this.http.put<any>(this.common.TicketUrl + "/TicketResolve/" + Ticket.id, Ticket, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  reopenTicket(Ticket: Ticket): Promise<any> {
    return this.http.put<any>(this.common.TicketUrl + "/TicketReopen/" + Ticket.id, Ticket, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  reopenInProgressTicket(Ticket: Ticket): Promise<any> {
    return this.http.put<any>(this.common.TicketUrl + "/TicketReopenInProgress/" + Ticket.id, Ticket, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  takeoverTicket(Id: any, UserID: any): Promise<any> {
    const data = { Id, UserID }; // Create an object with Id and UserID
    return this.http.put<any>(this.common.TicketUrl + "/TicketTakeover", data, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  acceptTicket(Id: any, UserID: any): Promise<any> {
    const data = { Id, UserID }; // Create an object with Id and UserID
    return this.http.put<any>(this.common.TicketUrl + "/TicketAccept", data, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  backOfficeAcceptTicket(Id: any, UserID: any): Promise<any> {
    const data = { Id, UserID }; // Create an object with Id and UserID
    return this.http.put<any>(this.common.TicketUrl + "/BackOfficeTicketAccept", data, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  assignTicketToOtherAgent(Id: any, UserID: any): Promise<any> {
    const data = { Id, UserID }; // Create an object with Id and UserID
    console.log(data);
    return this.http.put<any>(this.common.TicketUrl + "/AsignToOtherAgent", data, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  getOpened(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/UserOpened/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getNew(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/UserNew/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getResolved(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/UserResolved/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getClosed(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/UserClosed/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getReopened(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/UserReopened/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }


  ////////////////////////////////////////// Back Office Requests ///////////////////////////

  getBackOfficeAll(departmentID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/BackOffice/DepartmentAll/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeUnAnswered(departmentID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/BackOffice/DepartmentUnAnswered/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeInProgress(departmentID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/BackOffice/DepartmentInProgress/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeClosed(departmentID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/BackOffice/DepartmentClosed/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeResolved(departmentID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/BackOffice/DepartmentResolved/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeAssignedToUser(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/BackOffice/DepartmentAssignedToUser/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  ////////////////////////////////////////// Back Office Requests Total ///////////////////////////


  getBackOfficeAllTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/BackOffice/DepartmentAllTotal/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeUnAnsweredTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/BackOffice/DepartmentUnAnsweredTotal/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeInProgressTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/BackOffice/DepartmentInProgressTotal/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeClosedTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/BackOffice/DepartmentClosedTotal/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackOfficeResolvedTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/BackOffice/DepartmentResolvedTotal/" + departmentID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getBackkOfficeAssignedToUserTotal(userID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/BackOffice/DepartmentAssingedToBackOfficeTotal/" + userID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }


  getSupervisorAllTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/Supervisor/Total" , {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorNewTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/Supervisor/TotalNew" , {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorInProgressTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/Supervisor/TotalOpen" , {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorClosedTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/Supervisor/TotalClosed", {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorResolvedTotal(departmentID: any = null): Promise<any> {
    return this.http.get<number>(this.common.TicketUrl + "/Supervisor/TotalResolved", {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }


  getSupervisorOpened(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/Supervisor/Open", {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorNew(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/Supervisor/New", {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorClosed(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/Supervisor/Closed", {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorResolved(userID: any = null): Promise<any> {
    return this.http.get<any[]>(this.common.TicketUrl + "/Supervisor/Resolved", {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSupervisorAllByDepartment(departmentID: any = null): Promise<any[] | undefined> {
    return this.http.get<any[]>(
      `${this.common.TicketUrl}/Supervisor/DepartmentAllTotal/${departmentID}`,
      {
        headers: this.common.headers,
        responseType: 'json'
      }
    ).toPromise();
  }



  getAssignedToUser(userId: number): Promise<any> {
    return this.http.get(`${this.common.TicketUrl}/AssignedTo/${userId}`).toPromise();
  }

}
