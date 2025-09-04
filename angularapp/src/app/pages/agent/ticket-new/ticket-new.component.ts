import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { Category, Channel, Department, Priority, SettingsService, SubCategory, Request, FileAttachment } from '../../../services/settings.service';
import { LoggerService } from '../../../services/logger.service';
import { Router } from '@angular/router';
import { Ticket, TicketService } from '../../../services/ticket.service';
import { MessageService } from 'primeng/api';
import Editor from 'ckeditor5-custom-build/build/ckeditor';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { User } from '../../../services/user.service';
import { NzUploadChangeParam, NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { HttpHeaders } from '@angular/common/http';
import { Observable, Observer, Subscription } from 'rxjs';
import * as pako from 'pako';

@Component({
  selector: 'app-ticket-new',
  templateUrl: './ticket-new.component.html',
  styleUrls: ['./ticket-new.component.css']
})
export class TicketNewComponent {
  loading: boolean = false;


  departments: Department[] = [];
  categories: Category[] = [];
  subcategories: SubCategory[] = [];
  priorities: Priority[] = [];
  requests: Request[] = [];
  channels: Channel[] = [];

  selectedDepartment: Department = {};
  selectedCategory: Category = {};
  selectedSubcategory: SubCategory = {};

  selectedPriority: Priority = {};
  selectedRequest: Request = {};
  selectedChannel: Channel = {};
  newTicket: Ticket = {};
  user: User = {};
  fileAttachment: FileAttachment = {};
  name: any;
  email: any;
  phone: any;
  subject: any;
  smsAlertControl!: FormControl;
  emailAlertControl!: FormControl;
  body?: any = {};
  userform!: FormGroup;
  title = 'customEditor';
  customerDetails: any;
  public Editor = Editor;
  refNo: any;
  refType: any;
  defaultFileList: NzUploadFile[] = [];
  fileList2 = [...this.defaultFileList];
  constructor(private loggerService: LoggerService, private fb: FormBuilder, private settingService: SettingsService, private logger: LoggerService, private router: Router, private cdr: ChangeDetectorRef, private ticketService: TicketService, private messageService: MessageService) { }

  ngOnInit() {
    this.userform = this.fb.group({
      name: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      phone: new FormControl('', [Validators.required, Validators.minLength(6)]),
      smsAlert: new FormControl('Yes'),
      emailAlert: new FormControl('Yes'),
    });

    this.smsAlertControl = this.userform.get('smsAlert') as FormControl;
    this.emailAlertControl = this.userform.get('emailAlert') as FormControl;

    this.customerDetails = JSON.parse(localStorage.getItem("CustomerDetails") || "[]");

      if (this.customerDetails.length > 0) {
        const customer = this.customerDetails[0];
        const type = customer.type;

        if (type === 'P') {
          this.name = customer.INSURED_NAME;
          this.phone = customer.MBILE_NO;
          this.refNo = customer.POLICY_NUMBER;
          this.refType = 'Policy No';
        } else if (type === 'C') {
          this.name = customer.OWNER_NAME;
          this.phone = customer.OWNER_MOBILE_NO;
          this.refNo = customer.CLAIM_NO;
          this.refType = 'Claim No';
        }

      // Optionally patch the form
      this.userform.patchValue({
        name: this.name,
        phone: this.phone,
        email: this.email  
      });
    } else {
      this.customerDetails = [];
    }

    this.settingService.getPriorites().subscribe(
      (res: any) => {
        this.priorities = res;
        this.cdr.detectChanges();
      },
      (error) => {

        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );
    this.settingService.getDepartments().subscribe(
      (res: any) => {
        this.departments = res;
        this.cdr.detectChanges();
      },
      (error) => {

        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );
    this.settingService.getChannels().subscribe(
      (res: any) => {
        this.channels = res;
        this.cdr.detectChanges();
      },
      (error) => {

        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );
    this.settingService.getRequests().subscribe(
      (res: any) => {
        this.requests = res;
        this.cdr.detectChanges();
      },
      (error) => {

        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );
  }
  onDepartmentChange() {
    this.selectedSubcategory = {};
    this.selectedCategory = {};
    this.categories = [];
    this.subcategories = [];
    this.settingService.getCategoriesbyDepartment(this.selectedDepartment.departmentID).then(
      (res: any) => {
        this.categories = res;
        this.cdr.detectChanges();
      },
      (error) => {

        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );

  }
  onCategoryChange() {
    this.selectedSubcategory = {};
    this.subcategories = [];
    this.settingService.getSubCategoriesbyMainCategory(this.selectedCategory.categoryID).then(
      (res: any) => {
        this.subcategories = res;
        this.cdr.detectChanges();
      },
      (error) => {

        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );
  }




  stripHtmlTags(htmlString: string): string {
    const tmp = document.createElement('div');
    tmp.innerHTML = htmlString;
    return tmp.textContent || tmp.innerText || '';
  }
  createTicket() {

     this.user = JSON.parse(localStorage.getItem("user") || "{}") as User;
    const textWithoutTags = this.stripHtmlTags(this.body);
    if (textWithoutTags === "" || textWithoutTags === "undefined") {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail:
          "Please enter ticket description",
        life: 3000,
      });
    }
    else {
      this.newTicket.userID = this.user.user_Id;
      this.newTicket.assingedToBackOfficeID = 0;
      this.newTicket.assingedToUserID = 0;
      this.newTicket.categoryID = this.selectedCategory.categoryID;
      const subCategoryNote = this.selectedSubcategory ? this.selectedSubcategory.description : 'Not specified';
      const requestNote = this.selectedRequest ? this.selectedRequest.description : 'Not specified';
      this.newTicket.subCategoryID = this.selectedSubcategory.subCategoryID; // ✅ must be a number
      this.newTicket.requestID = this.selectedRequest?.requestID ?? 'Not specified';
      this.newTicket.departmentReply = `Subcategory: ${subCategoryNote}, Reason: ${requestNote}`;
      this.newTicket.departmentID = this.selectedDepartment.departmentID;
      this.newTicket.channelID = this.selectedChannel.channelID;
      this.newTicket.priority = this.selectedPriority.levelID;
      this.newTicket.escalationLevel = "0";
      this.newTicket.statusID = 1;
      this.newTicket.subject = this.subject;
      this.newTicket.body = textWithoutTags;
      this.newTicket.flag = true;
      this.newTicket.customerName = this.name;
      this.newTicket.email = this.email;
      this.newTicket.mobile = this.phone;
      this.newTicket.companyID = this.selectedDepartment.companyID;
      this.newTicket.updateByUser = this.user.user_Id;
      this.newTicket.departmentReply = "no reply";
      this.newTicket.referenceNumber = this.refNo?.trim() || 'Not specified - Non Customer Ticket';
      this.newTicket.referenceType = this.refType?.trim() || 'Not specified';
      const currentDate = new Date();
      this.newTicket.startDate = currentDate;
      this.newTicket.smsAlert = this.userform.value.smsAlert === 'Yes';
      this.newTicket.emailAlert = this.userform.value.emailAlert === 'Yes';

      if (this.checkIfAnyFieldIsEmpty(this.newTicket)) {
        console.error(this.newTicket);
        console.error('Error: One or more fields are empty.');
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "Please fill all fields",
          life: 3000,
        });
      } else {
        this.ticketService.addTicket(this.newTicket).then(
          (created: any) => {
            // Use API response directly (returns created ticket with id)
            this.newTicket.id = created?.id ?? created?.Id;
            if (this.fileList2 && this.fileList2.length > 0) {
              this.customRequest(this.fileList2);
            }
            localStorage.setItem("TicketDetails", JSON.stringify(this.newTicket));
            this.router.navigateByUrl("/main/agent/tickets/details");
          },
          (error) => {
            this.loggerService.logError(error);
            const userLog = localStorage.getItem('userLogs') || '';
            const updatedUserLog = `${userLog}\n'error'${error}`;
            localStorage.setItem('userLogs', updatedUserLog);

            // Handle duplicate RequestID (API returns 409 Conflict)
            if (error?.status === 409) {
              const dupId = error?.error?.id ?? '';
              this.messageService.add({
                severity: 'warn',
                summary: 'Duplicate Ticket',
                detail: dupId ? `A ticket with the same Request is already opened (ID: ${dupId}).` : 'A ticket with the same Request is already opened.',
                life: 5000,
              });
              return;
            }

            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: "An error occurred while creating the ticket",
              life: 3000,
            });
          }
        );
      }
    }
  }
  async customRequest(files: NzUploadFile[]): Promise<void[]> {
    const promises: Promise<void>[] = [];

    for (const file of files) {
      promises.push(this.processFile(file));
    }

    return Promise.all(promises);
  }

  private async processFile(file: NzUploadFile): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        const blob = await this.getFileBlob(file);

        const buffer = await this.blobToArrayBuffer(blob);

        // Convert ArrayBuffer to Uint8Array
        const uint8Array = new Uint8Array(buffer);

        const fileHash = await this.hashFileData(uint8Array);

        const base64Data = await this.arrayBufferToBase64(buffer);

        const fileHashString = this.arrayBufferToHex(fileHash);

        this.fileAttachment.ticketID = this.newTicket.id || undefined;
        this.fileAttachment.fileName = file.name;
        this.fileAttachment.fileSize = file['size']?.toString() || undefined;
        this.fileAttachment.fileHash = fileHashString;
        this.fileAttachment.fileData = base64Data;

        this.settingService.addFile(this.fileAttachment).then(
          (res) => {

          },
          (error) => {
            this.loggerService.logError(error);
            const userLog = localStorage.getItem('userLogs') || '';
            const updatedUserLog = `${userLog}\n'error'${error}`;
            localStorage.setItem('userLogs', updatedUserLog);
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail:
                "An error occurred while uploading attachments",
              life: 3000,
            });
          }
        )

        resolve();
      } catch (error) {
        console.error("Error processing file:", error);
        reject(error);
      }
    });
  }

  private async getFileBlob(file: NzUploadFile): Promise<Blob> {
    return new Promise<Blob>((resolve, reject) => {
      if (file.originFileObj) {
        resolve(file.originFileObj);
      } else {
        reject(new Error("File object not found."));
      }
    });
  }

  private async blobToArrayBuffer(blob: Blob): Promise<ArrayBuffer> {
    return new Promise<ArrayBuffer>((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = () => {
        if (reader.result instanceof ArrayBuffer) {
          resolve(reader.result);
        } else {
          reject(new Error("Failed to convert blob to ArrayBuffer."));
        }
      };

      reader.onerror = (event) => {
        reject(event.target?.error || new Error("File reading error."));
      };

      reader.readAsArrayBuffer(blob);
    });
  }

  private async hashFileData(data: Uint8Array): Promise<ArrayBuffer> {
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    return hashBuffer;
  }

  private arrayBufferToHex(buffer: ArrayBuffer): string {
    const hashArray = Array.from(new Uint8Array(buffer));
    return hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');
  }
  private arrayBufferToBase64(buffer: ArrayBuffer): string {
    const bytes = new Uint8Array(buffer);
    const binary = Array.from(bytes).map((byte) => String.fromCharCode(byte)).join('');
    return btoa(binary);
  }
checkIfAnyFieldIsEmpty(item: { [key: string]: any }): boolean {
  const optionalFields = ['subCategoryID', 'requestID', 'departmentReply'];

  for (const key in item) {
    if (item.hasOwnProperty(key) && !optionalFields.includes(key)) {
      const value = item[key];

      if (
        value === null ||
        value === undefined ||
        value === '' ||
        value === "[object Object]"
      ) {
        return true;
      }
    }
  }
  return false;
}




}
