import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Company, Department, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrls: ['./department.component.css']
})
export class DepartmentComponent {
  DepartmentsDialog!: boolean;
  DepartmentsEditDialog!: boolean;
  Departments!: Department[];
  Department: Department = {};
  Companies!: Company [];
  Company: Company = {};
  selectedDepartments!: Department[];
  selectedCompany: Company = {};
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
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
    this.settingsService.getDepartments().subscribe(
      (res: any) => {
        this.Departments = res;
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
    this.settingsService.getCompanies().subscribe(
      (res: any) => {
        this.Companies = res;
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
  openNew() {
    this.submitted = false;
    this.DepartmentsDialog = true;
  }
  editDepartment(Departments: Department) {
    this.Department = { ...Departments };
    this.DepartmentsEditDialog = true;
  }
  deleteDepartment(Departments: Department) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Department  ' + Departments.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteDepartment(Departments.departmentID).then(
          (res) => {
            this.messageService.add({ severity: 'error', summary: 'done ', detail: 'Department Deleted', life: 3000 });
            this.Departments = this.Departments.filter(val => val.departmentID !== Departments.departmentID);
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
    this.DepartmentsDialog = false;
    this.DepartmentsEditDialog = false;
    this.submitted = false;
    this.Department = {};
  }
  editDepartmentsD(Departments: Department) {
    Departments.companyID = this.Company.companyID?.toString();
    this.settingsService.editDepartment(Departments).then(
      (res) => {
        this.messageService.add({ severity: 'warn', summary: 'Done ', detail: 'Department name edited succesfully', life: 3000 });
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
    this.Departments = [...this.Departments];
    this.DepartmentsEditDialog = false;
    this.getData();
    this.hideDialog();

  }
  saveDepartments(Departments: Department, Company: Company) {
    this.submitted = true;
    Departments.companyID = this.Company.companyID?.toString();
    this.settingsService.addDepartment(Departments).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Department added successfully', life: 3000 });
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
    this.Departments = [...this.Departments];
    this.DepartmentsDialog = false;
    this.getData();
    this.hideDialog();

  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Departments.length; i++) {
      if (this.Departments[i].departmentID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Departments);
    const workbook = {
      Sheets: { 'Departments List': worksheet },
      SheetNames: ['Departments List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Departments List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
  deleteSelectedDepartments() {
    if (this.selectedDepartments && this.selectedDepartments.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedDepartments.map(status => String(status.departmentID)); // Convert to strings
          this.settingsService.deleteSelectedDepartments(selectedIds).then(
            () => {
              this.selectedDepartments = [];
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
getCompanyNameById(companyId?: string): string {
    if (companyId === undefined) {
        return ''; // or any default value
    }
    const company = this.Companies.find(c => c.companyID === companyId);
    return company?.description || '';
}

}
