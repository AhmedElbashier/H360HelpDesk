import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Escalation, Department, SettingsService, Priority } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-escalations',
  templateUrl: './escalations.component.html',
  styleUrls: ['./escalations.component.css']
})
export class EscalationsComponent {
  EscalationsDialog!: boolean;
  EscalationsEditDialog!: boolean;
  Escalations!: Escalation[];
  Escalation: Escalation = {};
  Departments!: Department[];
  Department: Department = {};
  Priorities!: Priority[];
  Priority: Priority = {};
  selectedEscalations!: Escalation[];
  selectedPriority: Priority = {};
  selectedDepartment: Department = {};
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
    this.settingsService.getEscalations().subscribe(
      (res: any) => {
        this.Escalations = res;
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

    this.settingsService.getPriorites().subscribe(
      (res: any) => {
        this.Priorities = res;
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
  }
  getEscalationDepartmentName(departmentID: any): any {
    const department = this.Departments?.find((d) => d.departmentID === departmentID);
    return department ? department.description : 'N/A';
  }
  getEscalationPriorityName(levelID: any): any {
    const priority = this.Priorities?.find((d) => d.levelID === levelID);
    return priority ? priority.description : 'N/A';
  }
  openNew() {
    this.submitted = false;
    this.EscalationsDialog = true;
  }
  editEscalation(Escalations: Escalation) {
    this.Escalation = { ...Escalations };
    this.EscalationsEditDialog = true;
  }
  deleteEscalation(Escalations: Escalation) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Escalation  ' + Escalations.escalationID + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteEscalation(Escalations.escalationID).then(
          (res) => {
            this.messageService.add({ severity: 'error', summary: 'done ', detail: 'Escalation Deleted', life: 3000 });
            this.Escalations = this.Escalations.filter(val => val.escalationID !== Escalations.escalationID);
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
    this.EscalationsDialog = false;
    this.EscalationsEditDialog = false;
    this.submitted = false;
    this.Escalation = {};
  }
  editEscalationsD(Escalations: Escalation) {
    const currentDate = new Date();
    Escalations.departmentID = this.Department.departmentID;
    Escalations.levelID = this.Priority.levelID;
    this.settingsService.editEscalation(Escalations).then(
      (res) => {
        this.messageService.add({ severity: 'warn', summary: 'Done ', detail: 'Escalation name edited succesfully', life: 3000 });
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
    this.Escalations = [...this.Escalations];
    this.EscalationsEditDialog = false;
    this.getData();
    this.hideDialog();

  }
  saveEscalations(Escalations: Escalation, Department: Department) {
    this.submitted = true;
    Escalations.departmentID = this.Department.departmentID?.toString();
    Escalations.levelID = this.selectedPriority.levelID;
    this.settingsService.addEscalation(Escalations).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Escalation added successfully', life: 3000 });
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
    this.Escalations = [...this.Escalations];
    this.EscalationsDialog = false;
    this.getData();
    this.hideDialog();

  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Escalations.length; i++) {
      if (this.Escalations[i].escalationID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Escalations);
    const workbook = {
      Sheets: { 'Escalations List': worksheet },
      SheetNames: ['Escalations List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Escalations List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
  deleteSelectedEscalations() {
    if (this.selectedEscalations && this.selectedEscalations.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedEscalations.map(status => String(status.escalationID)); // Convert to strings
          this.settingsService.deleteSelectedPriorites(selectedIds).then(
            () => {
              this.selectedEscalations = [];
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
  getPriorityNameById(levelID?: string): string {
    if (levelID === undefined) {
      return ''; // or any default value
    }
    const Priority = this.Priorities.find(c => c.levelID === levelID);
    return Priority?.description || '';
  }
}
