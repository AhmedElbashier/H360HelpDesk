import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Attachment, AttachmentService } from '../../../services/attachment.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { LoggerService } from '../../../services/logger.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-attachment',
  templateUrl: './attachment.component.html',
  styleUrls: ['./attachment.component.css']
})
export class AttachmentComponent {

  AttachmentsDialog!: boolean;
  Attachments!: Attachment[];
  Attachment!: Attachment;
  selectedAttachments!: Attachment[];
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  exportColumns!: any[];
  uploadDialog!: boolean;
  selectedFile: File | null = null;
@ViewChild('dt') dt!: Table;

  constructor(private logger: LoggerService, private userService: UserService, private cdr: ChangeDetectorRef, private attachmentService: AttachmentService, private messageService: MessageService, private confirmationService: ConfirmationService, private router: Router) { }
  applyFilterGlobal(event: any) {
    const searchTerm = (event.target as HTMLInputElement).value;
    if (this.dt) {
      this.dt.filterGlobal(searchTerm, 'contains');
    }
  }
  ngOnInit() {
    this.Delete = "Delete";
    this.getData();
  }
  getData() {
    this.attachmentService.getAttachments().subscribe(
      (res: any) => {
        this.Attachments = res;
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
  }
  openNew() {
    this.Attachment = {};
    this.uploadDialog = true;
  }
  deleteAttachment(Attachments: Attachment) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Folder  ' + Attachments.fileName + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.attachmentService.deleteAttachment(Attachments.id).then(
          (res) => {
            this.messageService.add({ severity: 'success', summary: 'done ', detail: 'Folder Deleted', life: 3000 });
            this.Attachments = this.Attachments.filter(val => val.id !== Attachments.id);
            this.getData();
          },
          (error) => {
            
            this.logger.logError(error);
            this.router.navigateByUrl("error");
            this.messageService.add({
              severity: 'error',
              summary: 'failed',
              detail: 'System Error! ',
              life: 3000,
            });
          }
        );
        this.getData();
      }
    });
  }

  hideDialog() {
    this.AttachmentsDialog = false;
    this.submitted = false;
  }
  findIndexById(id: string): number {
    let index = -1;
    for (let i = 0; i < this.Attachments.length; i++) {
      if (this.Attachments[i].id == id) {
        index = i;
        break;
      }
    }
    return index;
  }
  createId(): string {
    let id = '';
    var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    for (var i = 0; i < 5; i++) {
      id += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return id;
  }
  exportExcel() {
    const worksheet = XLSX.utils.json_to_sheet(this.Attachments);
    const workbook = {
      Sheets: { 'Attachments List': worksheet },
      SheetNames: ['Attachments List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Attachments List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }

  deleteSelectedAttachments() {
    if (this.selectedAttachments && this.selectedAttachments.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedAttachments.map(status => String(status.id)); // Convert to strings
          this.attachmentService.deleteSelectedAttachments(selectedIds).then(
            () => {
              this.selectedAttachments = [];
              this.getData();
            },
            (error) => {
              console.error('Error deleting selected items:', error);
            }
          );
        }
      });
    }
  }

  downloadAttachment(attachmentId: number, fileName: string) {
    this.attachmentService.downloadAttachment(attachmentId)
      .subscribe(
        (response: any) => {
          const blob = new Blob([response], { type: 'application/octet-stream' });
          saveAs(blob, fileName);
        },
        (error) => {
          console.error('Error downloading attachment:', error);
          
          this.logger.logError(error);
          this.router.navigateByUrl("error");
          // Handle error, show error message, etc.
        }
      );
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onSaveAttachments() {
    if (!this.selectedFile) {
      return; // No file selected, handle error if needed
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('owner_User_Id', '1'); // Replace with actual user ID
    formData.append('fileName', this.selectedFile.name);
    formData.append('size', this.selectedFile.size.toString());
    formData.append('ticket_Id', "Others");
    formData.append('created_at', new Date().toISOString());
    formData.append('updated_at', new Date().toISOString());
    formData.append('deleted', 'false');
    formData.append('id', '0');
    formData.append('url', `usersAttachments/${this.selectedFile.name}`);

    this.attachmentService.uploadFileWithAttachment(formData)
      .subscribe(
        async (response: any) => {
          if (response == '"File format not supported"' || response ==  400) {
            this.messageService.add({ severity: 'warn', summary: 'Error ', detail: 'The File:' + this.selectedFile?.name + "format not supported", life: 10000 });
          }
          else { await this.messageService.add({ severity: 'success', summary: 'done ', detail: 'The File:' + this.selectedFile?.name + "uploaded", life: 10000 }); }

          window.location.reload();
        },
        (error) => {

            
            this.logger.logError(error);
            this.router.navigateByUrl("error");
          }// Handle error, show error message, etc.
      );
  }
}
