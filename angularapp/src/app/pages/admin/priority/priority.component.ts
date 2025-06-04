import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Priority, Department, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-priority',
  templateUrl: './priority.component.html',
  styleUrls: ['./priority.component.css']
})
export class PriorityComponent {

  PrioritiesDialog!: boolean;
  PrioritiesEditDialog!: boolean;
  ReceptientDialog!: boolean;
  DueDateDialog!: boolean;
  Priorities!: Priority[];
  Priority: Priority = {};
  Departments!: Department[];
  Department: Department = {};
  selectedPriorities!: Priority[];
  selectedDepartment: Department = {};
  proprtiesTypes!: any[];
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  exportColumns!: any[];
  selectedProprties: any;
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
    this.proprtiesTypes = [
      {id: "1", name: "Low" },
      {id: "2", name: "Medium" },
      {id: "3", name: "High" },
    ]
  }
  getData() {
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
  openNew() {
    this.submitted = false;
    this.PrioritiesDialog = true;
  }
  editPriority(Priorities: Priority) {
    this.Priority = { ...Priorities };
    this.PrioritiesEditDialog = true;
  }
  deletePriority(Priorities: Priority) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Priority  ' + Priorities.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deletePriority(Priorities.levelID).then(
          (res) => {
            this.messageService.add({ severity: 'error', summary: 'done ', detail: 'Priority Deleted', life: 3000 });
            this.Priorities = this.Priorities.filter(val => val.levelID !== Priorities.levelID);
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
    this.PrioritiesDialog = false;
    this.PrioritiesEditDialog = false;
    this.DueDateDialog = false;
    this.ReceptientDialog = false;
    this.submitted = false;
    this.Priority = {};
  }
  editEmails(Priorities: Priority) {
    this.Priority = { ...Priorities };
    this.ReceptientDialog = true;
  }
  editPrioritiesD(Priorities: Priority) {
    this.settingsService.editPriority(Priorities).then(
      (res) => {
        this.messageService.add({ severity: 'warn', summary: 'Done ', detail: 'Priority name edited succesfully', life: 3000 });
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
    this.Priorities = [...this.Priorities];
    this.PrioritiesEditDialog = false;
    this.getData();
    this.hideDialog();

  }
  savePriorities(Priorities: Priority) {
    this.submitted = true;
    Priorities.description = this.selectedProprties.name;
    this.settingsService.addPriority(Priorities).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Priority added successfully', life: 3000 });
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
    this.Priorities = [...this.Priorities];
    this.PrioritiesDialog = false;
    this.getData();
    this.hideDialog();

  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Priorities.length; i++) {
      if (this.Priorities[i].levelID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Priorities);
    const workbook = {
      Sheets: { 'Priorities List': worksheet },
      SheetNames: ['Priorities List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Priorities List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
  deleteSelectedPriorities() {
    if (this.selectedPriorities && this.selectedPriorities.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedPriorities.map(status => String(status.levelID)); // Convert to strings
          this.settingsService.deleteSelectedPriorites(selectedIds).then(
            () => {
              this.selectedPriorities = [];
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
  dueDate(Priority: Priority) {
    this.DueDateDialog = true;
  }
  setDueDate() {

  }
}
