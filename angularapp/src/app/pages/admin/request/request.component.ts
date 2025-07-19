import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Request, Department, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { Table } from 'primeng/table';
@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.css']
})
export class RequestComponent {



  RequestsDialog!: boolean;
  RequestsEditDialog!: boolean;
  Requests!: Request[];
  Request: Request = {};
  Departments!: Department[];
  Department: Department = {};
  selectedRequests!: Request[];
  selectedDepartment: Department = {};
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  allRequests: Request[] = []; // holds the unfiltered list
  selectedDepartmentId: string | null = null;
  departmentOptions: { label: string, value: string | null }[] = [];
  exportColumns!: any[];
  @ViewChild('dt') dt!: Table;
  constructor(private userService: UserService, private cdr: ChangeDetectorRef, private settingsService: SettingsService, private messageService: MessageService, private confirmationService: ConfirmationService, private router: Router) { }
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
    this.settingsService.getRequests().subscribe(
      (res: any) => {
        this.allRequests = res;
        this.Requests = [...this.allRequests];
        this.cdr.detectChanges();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'An error occurred while connecting to the database',
          life: 3000,
        });
      }
    );

    this.settingsService.getDepartments().subscribe(
      (res: any) => {
        this.Departments = res;
        this.departmentOptions = [
          { label: 'All Departments', value: null },
          ...res.map((d: Department) => ({
            label: d.description,
            value: d.departmentID,
          })),
        ];
        this.cdr.detectChanges();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'An error occurred while connecting to the database',
          life: 3000,
        });
      }
    );
  }
  filterByDepartment() {
    if (this.selectedDepartmentId) {
      this.Requests = this.allRequests.filter(
        (req) => req.departmentID === this.selectedDepartmentId
      );
    } else {
      this.Requests = [...this.allRequests]; // Show all
    }
  }

  getRequestDepartmentName(departmentID: any): any {
    const department = this.Departments?.find((d) => d.departmentID === departmentID);
    return department ? department.description : 'N/A';
  }


  openNew() {
    this.submitted = false;
    this.RequestsDialog = true;
  }
  editRequest(Requests: Request) {
    this.Request = { ...Requests };
    this.RequestsEditDialog = true;
  }
  deleteRequest(Requests: Request) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Request  ' + Requests.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteRequest(Requests.requestID).then(
          (res) => {
            this.messageService.add({ severity: 'error', summary: 'done ', detail: 'Request Deleted', life: 3000 });
            this.Requests = this.Requests.filter(val => val.requestID !== Requests.requestID);
            this.getData();
          },
          (error) => {
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
    this.hideDialog();

  }

  hideDialog() {
    this.RequestsDialog = false;
    this.RequestsEditDialog = false;
    this.submitted = false;
    this.Request = {};
  }
  editRequestsD(Requests: Request) {
    const currentDate = new Date();
    Requests.departmentID = this.Department.departmentID;
    this.settingsService.editRequest(Requests).then(
      (res) => {
        this.messageService.add({ severity: 'warn', summary: 'Done ', detail: 'Request name edited succesfully', life: 3000 });
        this.getData();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Requests = [...this.Requests];
    this.RequestsEditDialog = false;
    this.getData();
    this.hideDialog();

  }
  saveRequests(Requests: Request, Department: Department) {
    this.submitted = true;
    Requests.departmentID = this.Department.departmentID?.toString();
    this.settingsService.addRequest(Requests).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Request added successfully', life: 3000 });
        this.getData();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Requests = [...this.Requests];
    this.RequestsDialog = false;
    this.getData();
    this.hideDialog();

  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Requests.length; i++) {
      if (this.Requests[i].requestID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Requests);
    const workbook = {
      Sheets: { 'Requests List': worksheet },
      SheetNames: ['Requests List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Requests List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
  deleteSelectedRequests() {
    if (this.selectedRequests && this.selectedRequests.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedRequests.map(status => String(status.requestID)); // Convert to strings
          this.settingsService.deleteSelectedPriorites(selectedIds).then(
            () => {
              this.selectedRequests = [];
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
  getDepartmentNameById(DepartmentID?: string): string {
    if (DepartmentID === undefined) {
      return ''; // or any default value
    }
    const Department = this.Departments.find(c => c.departmentID === DepartmentID);
    return Department?.description || '';
  }
}
