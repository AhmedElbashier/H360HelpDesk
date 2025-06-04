import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

export interface Channel {
  channelID?: string;
  description?: string;
}
export interface Company {
  companyID?: string;
  description?: string;
}
export interface Department {
  departmentID?: string;
  companyID?: string;
  description?: string;
}
export interface Request {
  requestID?: string;
  description?: string;
  departmentID?: string;
}
export interface Category {
  categoryID?: string;
  description?: string;
  departmentID?: string;
}
export interface SubCategory {
  subCategoryID?: string;
  categoryID?: string;
  description?: string;
}
export interface Status {
  statusID?: string;
  description?: string;
}
export interface Priority {
  levelID?: string;
  description?: string;

}
export interface EscalationTimer {
  timerID?: number;
  hours?: number;

}
export interface License {
  licenseFile: string;
  licenseInfo: {
    key: string;
    company: string;
    vendor: string;
    adminsLimit: number;
    agentsLimit: number;
    supervisorsLimit: number;
    backOfficeLimit: number;
    expirationDate: string;
  };
}

export interface Escalation {
  escalationID?: string;
  departmentID?: string;
  levelID?: string;
  days?: string;
  emails?: string;

}
export interface FileAttachment {
  fileID?: string;
  ticketID?: string;
  commentID?: string;
  userID?: string;
  fileName?: string;
  fileHash?: string;
  fileData?: string;
  fileSize?: string;
  days?: string;
  emails?: string;

}


export interface SmtpSettings {
  id?: string; 
  host?: string;
  port?: number;
  username?: string;
  password?: string; 
  useSsl?: boolean; 
  useDefaultCredentials?: boolean; 
  fromAddress?: string;
  displayName?: string; 
}

export interface EmailRequest {
  to?: string[]; // Recipient email address
  subject?: string; // Email subject
  body?: string; // Email body content
  cc?: string[]; // Optional array of CC email addresses
  bcc?: string[];

}

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor(
    private http: HttpClient,
    private common: CommonService,
    private msg: MessageService,
  ) { }

  getFiles(type: any = null): Observable<any[]> {
    return this.http.get<File[]>(this.common.FileUrl, { headers: this.common.headers });
  }
  getFile(FileID: any = null): Promise<any> {
    return this.http.get<File>(this.common.FileUrl + "/" + FileID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getTicketFiles(TicketID: any = null): Promise<any> {
    return this.http.get<File[]>(this.common.FileUrl + "/Ticket/" + TicketID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addFile(FileAttach: any): Promise<any> {
    return this.http.post<any>(this.common.FileUrl, FileAttach, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteFile(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.FileUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editFile(FileAttach: FileAttachment): Promise<any> {
    return this.http.put<any>(this.common.FileUrl + "/" + FileAttach.fileID, FileAttach, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedFiles(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.FileUrl + "/deleteselected", options).toPromise();
  }
  downloadFile(fileId: number): Observable<any> {
    return this.http.get<string>(`${this.common.FileUrl}/DownloadFile/${fileId}`);
  }


  getChannels(type: any = null): Observable<any[]> {
    return this.http.get<Channel[]>(this.common.ChannelUrl, { headers: this.common.headers });
  }
  getChannel(ChannelID: any = null): Promise<any> {
    return this.http.get<Channel>(this.common.ChannelUrl + "/" + ChannelID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addChannel(Channel: any): Promise<any> {
    return this.http.post<any>(this.common.ChannelUrl, Channel, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteChannel(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.ChannelUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editChannel(Channel: Channel): Promise<any> {
    return this.http.put<any>(this.common.ChannelUrl + "/" + Channel.channelID, Channel, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedChannels(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.CompanyUrl + "/deleteselected", options).toPromise();
  }

  getCompanies(type: any = null): Observable<any[]> {
    return this.http.get<Company[]>(this.common.CompanyUrl, { headers: this.common.headers });
  }
  getCompany(CompanyID: any = null): Promise<any> {
    return this.http.get<Company>(this.common.CompanyUrl + "/" + CompanyID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addCompany(Company: any): Promise<any> {
    return this.http.post<any>(this.common.CompanyUrl, Company, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteCompany(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.CompanyUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editCompany(Company: Company): Promise<any> {
    return this.http.put<any>(this.common.CompanyUrl + "/" + Company.companyID, Company, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedCompanies(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.CompanyUrl + "/deleteselected", options).toPromise();
  }

  getDepartments(type: any = null): Observable<any[]> {
    return this.http.get<Department[]>(this.common.DepartmentUrl, { headers: this.common.headers });
  }
  getDepartment(Id: any = null): Promise<any> {
    return this.http.get<Department>(this.common.DepartmentUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addDepartment(Department: Department): Promise<Department> {
    return this.http.post<any>(this.common.DepartmentUrl, Department, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteDepartment(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.DepartmentUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editDepartment(Department: Department): Promise<any> {
    return this.http.put<any>(this.common.DepartmentUrl + "/" + Department.departmentID, Department, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedDepartments(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.DepartmentUrl + "/deleteselected", options).toPromise();
  }
  getRequests(type: any = null): Observable<any[]> {
    return this.http.get<Category[]>(this.common.RequestUrl, { headers: this.common.headers });
  }
  getRequest(RequestID: any = null): Promise<any> {
    return this.http.get<Request>(this.common.RequestUrl + "/" + RequestID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  geRequestsbyDepartment(RequestID: any = null): Promise<any> {
    return this.http.get<Request>(this.common.RequestUrl + "/bydepartment/" + RequestID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addRequest(Request: any): Promise<any> {
    return this.http.post<any>(this.common.RequestUrl, Request, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteRequest(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.RequestUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  editRequest(Request: Request): Promise<any> {
    return this.http.put<any>(this.common.RequestUrl + "/" + Request.requestID, Request, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedRequests(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.RequestUrl + "/deleteselected", options).toPromise();
  }

  getCategories(type: any = null): Observable<any[]> {
    return this.http.get<Category[]>(this.common.CategoryUrl, { headers: this.common.headers });
  }
  getCategory(CategoryID: any = null): Promise<any> {
    return this.http.get<Category>(this.common.CategoryUrl + "/" + CategoryID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getCategoriesbyDepartment(CategoryID: any = null): Promise<any> {
    return this.http.get<Category>(this.common.CategoryUrl + "/bydepartment/" + CategoryID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addCategory(Category: any): Promise<any> {
    return this.http.post<any>(this.common.CategoryUrl, Category, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteCategory(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.CategoryUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  editCategory(Category: Category): Promise<any> {
    return this.http.put<any>(this.common.CategoryUrl + "/" + Category.categoryID, Category, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedCategories(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.CategoryUrl + "/deleteselected", options).toPromise();
  }

  getSubCategories(type: any = null): Observable<any[]> {
    return this.http.get<SubCategory[]>(this.common.SubCategoryUrl, { headers: this.common.headers });
  }
  getSubCategory(Id: any = null): Promise<any> {
    return this.http.get<SubCategory>(this.common.SubCategoryUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  getSubCategoriesbyMainCategory(Id: any = null): Promise<any> {
    return this.http.get<SubCategory>(this.common.SubCategoryUrl + "/bymaincategory/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addSubCategory(SubCategory: any): Promise<any> {
    return this.http.post<any>(this.common.SubCategoryUrl, SubCategory, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSubCategory(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.SubCategoryUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editSubCategory(SubCategory: SubCategory): Promise<any> {
    return this.http.put<any>(this.common.SubCategoryUrl + "/" + SubCategory.subCategoryID, SubCategory, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedSubCategories(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.SubCategoryUrl + "/deleteselected", options).toPromise();
  }
  getStatuses(type: any = null): Observable<any[]> {
    const extendedHeaders = this.common.headers.append('Authorization', 'Basic ' + btoa('admin:V0c4lc0m'));

    return this.http.get<Status[]>(this.common.StatusUrl, { headers:extendedHeaders });
  }
  getStatus(Id: any = null): Promise<any> {
    return this.http.get<Status>(this.common.StatusUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addStatus(Status: any): Promise<any> {
    return this.http.post<any>(this.common.StatusUrl, Status, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteStatus(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.StatusUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editStatus(Status: Status): Promise<any> {
    return this.http.put<any>(this.common.StatusUrl + "/" + Status.statusID, Status, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedStatuses(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.StatusUrl + "/deleteselected", options).toPromise();
  }

  getPriorites(type: any = null): Observable<any[]> {
    return this.http.get<Priority[]>(this.common.PriorityUrl, { headers: this.common.headers });
  }
  getPriority(Id: any = null): Promise<any> {
    return this.http.get<Priority>(this.common.PriorityUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addPriority(Priority: any): Promise<any> {
    return this.http.post<any>(this.common.PriorityUrl, Priority, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deletePriority(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.PriorityUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editPriority(Priority: Priority): Promise<any> {
    return this.http.put<any>(this.common.PriorityUrl + "/" + Priority.levelID, Priority, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedPriorites(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.PriorityUrl + "/deleteselected", options).toPromise();
  }

  getEscalations(type: any = null): Observable<any[]> {
    return this.http.get<Category[]>(this.common.EscalationUrl, { headers: this.common.headers });
  }
  getEscalation(EscalationID: any = null): Promise<any> {
    return this.http.get<Escalation>(this.common.EscalationUrl + "/" + EscalationID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  geEscalationsbyDepartment(EscalationID: any = null): Promise<any> {
    return this.http.get<Escalation>(this.common.EscalationUrl + "/bydepartment/" + EscalationID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addEscalation(Escalation: any): Promise<any> {
    return this.http.post<any>(this.common.EscalationUrl, Escalation, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteEscalation(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.EscalationUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  editEscalation(Escalation: Escalation): Promise<any> {
    return this.http.put<any>(this.common.EscalationUrl + "/" + Escalation.escalationID, Escalation, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedEscalations(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.EscalationUrl + "/deleteselected", options).toPromise();
  }
  getSmtpSettings(type: any = null): Observable<any[]> {
    return this.http.get<SmtpSettings[]>(this.common.SmtpSettingUrl, { headers: this.common.headers });
  }
  getSmtpSetting(SmtpSettingID: any = null): Promise<any> {
    return this.http.get<SmtpSettings>(this.common.SmtpSettingUrl + "/" + SmtpSettingID, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addSmtpSetting(SmtpSettings: SmtpSettings): Promise<any> {
    return this.http.post<any>(this.common.SmtpSettingUrl, SmtpSettings, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSmtpSetting(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.SmtpSettingUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  editSmtpSetting(SmtpSettings: SmtpSettings): Promise<any> {
    return this.http.put<any>(this.common.SmtpSettingUrl + "/" + SmtpSettings.id, SmtpSettings, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedSmtpSettings(selectedIds: any[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        ...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.SmtpSettingUrl + "/deleteselected", options).toPromise();
  }
  testEmail(emailRequest: EmailRequest): Observable<any> {
    const apiUrl = `${this.common.SmtpSettingUrl}/test`;

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      // Add any other headers as needed
    });

    return this.http.post(apiUrl, emailRequest, { headers });
  }


  getEscalationTimers(type: any = null): Observable<any[]> {
    return this.http.get<EscalationTimer[]>(this.common.EscalationTimerUrl, { headers: this.common.headers });
  }
  addEscalationTimer(EscalationTimer: any): Promise<any> {
    return this.http.post<any>(this.common.EscalationTimerUrl, EscalationTimer, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteEscalationTimer(Id: any): Promise<any> {
    return this.http.delete<any>(this.common.EscalationTimerUrl + "/" + Id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  editEscalationTimer(EscalationTimer: EscalationTimer): Promise<any> {
    return this.http.put<any>(this.common.EscalationTimerUrl + "/" + EscalationTimer.timerID, EscalationTimer, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  getLicense(type: any = null): Observable<any> {
    return this.http.get<any>(this.common.LicenseUrl+"/details");
  }
}
