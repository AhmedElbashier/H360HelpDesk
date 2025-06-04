import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Status, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { LoggerService } from '../../../services/logger.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.css']
})
export class StatusComponent {
  StatusesDialog!: boolean;
  StatusesEditDialog!: boolean;
  Statuses!: Status[];
  Status: Status = {};
  selectedStatuses!: Status[];
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  exportColumns!: any[];
  @ViewChild('dt') dt!: Table;
  constructor(private logger: LoggerService, private userService: UserService, private cdr: ChangeDetectorRef, private settingsService: SettingsService, private messageService: MessageService, private confirmationService: ConfirmationService, private router: Router) { }
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
    this.settingsService.getStatuses().subscribe(
      (res: any) => {
        this.Statuses = res;
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
    this.Status = {};
    this.submitted = false;
    this.StatusesDialog = true;
  }
  editStatus(Statuses: Status) {
    this.Status = { ...Statuses };
    this.StatusesEditDialog = true;
  }
  deleteStatus(Statuses: Status) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Status  ' + Statuses.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteStatus(Statuses.statusID).then(
          (res) => {
            this.messageService.add({ severity: 'success', summary: 'done ', detail: 'Status Deleted', life: 3000 });
            this.Statuses = this.Statuses.filter(val => val.statusID !== Statuses.statusID);
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
    this.StatusesDialog = false;
    this.StatusesEditDialog = false;
    this.submitted = false;
  }
  editStatusesD(Statuses: Status) {
    this.settingsService.editStatus(Statuses).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Status name edited succesfully', life: 3000 });
        this.getData();
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Statuses = [...this.Statuses];
    this.StatusesEditDialog = false;
    this.getData();
  }
  saveStatuses(Statuses: Status) {
    this.submitted = true;
    this.settingsService.addStatus(Statuses).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Status added successfully', life: 3000 });
        this.getData();
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Statuses = [...this.Statuses];
    this.StatusesDialog = false;
    this.getData();
  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Statuses.length; i++) {
      if (this.Statuses[i].statusID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Statuses);
    const workbook = {
      Sheets: { 'Statuses List': worksheet },
      SheetNames: ['Statuses List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Statuses List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }

  deleteSelectedStatuses() {
    if (this.selectedStatuses && this.selectedStatuses.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedStatuses.map(status => String(status.statusID)); // Convert to strings
          this.settingsService.deleteSelectedStatuses(selectedIds).then(
            () => {
              this.selectedStatuses = [];
              this.getData();
            },
            (error) => {
              console.error('Error deleting selected Severities:', error);
            }
          );
        }
      });
    }
  }
}
