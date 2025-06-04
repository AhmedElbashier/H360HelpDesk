
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { CommonService } from './common.service';
import { Observable } from 'rxjs';


export interface Attachment {
  id?: any;
  issueID?: any;
  fileName?: any;
  file?: any;
  url?: any;
  size?: any;
  ownerUserId?: any;
  deleted?: any;
  created_at?: any;
  updated_at?: any;
}


@Injectable({
  providedIn: 'root'
})
export class AttachmentService {

  data: any;
  constructor(
    private http: HttpClient,
    private common: CommonService,
    private msg: MessageService,
  ) { }

  getAttachments(type: any = null): Observable<any[]> {
    return this.http.get<Attachment[]>(this.common.AttachmentUrl);
  }
  getDeletedAttachments(type: any = null): Observable<any[]> {
    return this.http.get<Attachment[]>(this.common.AttachmentUrl + "/DeletedFiles", { headers: this.common.headers });
  }
  getRelatedAttachments(type: number): Observable<any[]> {
    return this.http.get<Attachment[]>(this.common.AttachmentUrl + "/GetRelated/" + type, {
      headers: this.common.headers,
      responseType: 'json'
    });
  }
  getAttachmentId(type: any = null): Observable<any> {
    return this.http.get<Attachment>(this.common.AttachmentUrl + "/" + type, {
      headers: this.common.headers,
      responseType: 'json'
    });
  }
  getAttachmentIdPromise(type: any = null): Promise<any> {
    return this.http.get<any>(this.common.AttachmentUrl + "/" + type, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  addAttachment(Attachment: any): Promise<any> {
    return this.http.post<any>(this.common.AttachmentUrl, Attachment, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteAttachment(id: any): Promise<any> {
    return this.http.delete<any>(this.common.AttachmentUrl + "/" + id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deletePermenantAttachment(id: any): Promise<any> {
    return this.http.delete<any>(this.common.AttachmentUrl + "/DeletePermenant/" + id, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  editAttachment(Attachment: any): Promise<any> {
    return this.http.put<any>(this.common.AttachmentUrl + "/" + Attachment.id, Attachment, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }
  deleteSelectedAttachments(selectedIds: string[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        ...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.AttachmentUrl + "/DeleteSelected", options).toPromise();
  }
  deletePermenantSelectedAttachments(selectedIds: string[]): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        ...this.common.headers,
        'Content-Type': 'application/json'
      }),
      body: selectedIds
    };
    return this.http.delete<any>(this.common.AttachmentUrl + "/DeleteSelectedPermenant", options).toPromise();
  }

  undeleteAttachment(AttachmentId: any): Promise<any> {
    return this.http.put<any>(this.common.AttachmentUrl + "/undelete/" + AttachmentId, {
      headers: this.common.headers,
      responseType: 'json'
    }).toPromise();
  }

  uploadFileWithAttachment(AttachmentData: FormData): Observable<any> {
    return this.http.post<any>(`${this.common.AttachmentUrl}/upload`, AttachmentData, {
      headers: this.common.headers,
      responseType: 'json'
    });
  }
  downloadAttachment(Id: number): Observable<Blob> {
    const url = `${this.common.AttachmentUrl}/download/${Id}`;
    return this.http.get(url, { responseType: 'blob' });
  }

}
