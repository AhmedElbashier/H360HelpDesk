import { ChangeDetectorRef, Component } from '@angular/core';
import Editor from 'ckeditor5-custom-build/build/ckeditor';
import { Category, Department, Priority, SettingsService, Status } from '../../../services/settings.service';
import { Ticket, TicketService } from '../../../services/ticket.service';
import { LoggerService } from '../../../services/logger.service';
import { MessageService } from 'primeng/api';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { AgentReport, ReportService, SuperVisorReport } from '../../../services/report.service';
import { User, UserService } from '../../../services/user.service';
import { autosize, formatDate, stripHtml } from '../../../utils/export-helpers';

@Component({
  selector: 'app-supervisor-reports',
  templateUrl: './supervisor-reports.component.html',
  styleUrls: ['./supervisor-reports.component.css']
})
export class SupervisorReportsComponent {

  tickets: Ticket[] = [];
  priorities: Priority[] = []
  selectedPriority: Priority = {};
  statuses: Status[] = []
  selectedStatus: Status = {};
  departments: Department[] = []
  selectedDepartment!: Department;
  categories: Category[] = []
  selectedCategory: Category = {};
  selectedAgent: User = {};
  selectedBackOffice: User = {};
  isTableVisible = false;
  reportData: SuperVisorReport = {};
  startDate: any;
  endDate: any;
  user: User = {};
  agents: User[] = [];
  backOfficeUsers: User[] = [];
  users: User[] = [];
  constructor(private userService:UserService, private reportService: ReportService, private ticketService: TicketService, private settingService: SettingsService, private logger: LoggerService, private messageService: MessageService, private cdr: ChangeDetectorRef) { }


  ngOnInit() {

    this.user = JSON.parse(localStorage.getItem("user") || "{}") as User;
    this.getFilters();
  }

  getFilters() {
    this.userService.getAgentUsers().subscribe(
      (res) => {
        this.agents = res;
        const newAgent: User = {
          user_Id: '0',
          lastname: 'All'
        };
        this.agents.unshift(newAgent);
      },
      (error) => {
        this.logger.logError(error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );

    this.userService.getBackOfficeUsers().subscribe(
      (res) => {
        this.backOfficeUsers = res;
        const newAgent: User = {
          user_Id: '0',
          lastname: 'All'
        };
        this.backOfficeUsers.unshift(newAgent);
      },
      (error) => {
        this.logger.logError(error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );
    this.userService.getUsers().subscribe(
      (res) => {
        this.users = res;
      },
      (error) => {
        this.logger.logError(error);
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
      (res) => {
        this.departments = res;
        const newDepartment: Department = {
          departmentID: '0',
          description: 'All'
        };
        this.departments.unshift(newDepartment);
      },
      (error) => {
        this.logger.logError(error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );

    this.settingService.getStatuses().subscribe(
      (res) => {
        this.statuses = res;
        const newStatus: Status = {
          statusID: '0',
          description: 'All'
        };
        this.statuses.unshift(newStatus);
      },
      (error) => {
        this.logger.logError(error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    );
    this.settingService.getCategories().subscribe(
      (res: any) => {
        this.categories = res;
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

    this.settingService.getPriorites().subscribe(
      (res) => {
        this.priorities = res;
        const newPriority: Priority = {
          levelID: '0',
          description: 'All'
        };
        this.priorities.unshift(newPriority);
      },
      (error) => {
        this.logger.logError(error);
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
    this.settingService.getCategoriesbyDepartment(this.selectedDepartment.departmentID).then(
      (res: any) => {
        this.categories = res;
        const newCategory: Category = {
          categoryID: '0',
          description: 'All'
        };
        this.categories.unshift(newCategory);
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
  //exportExcel() {
  //  const worksheet = XLSX.utils.json_to_sheet(this.tickets);
  //  const workbook = {
  //    Sheets: { 'Tickets Report': worksheet },
  //    SheetNames: ['Tickets Report'],
  //  };

  //  const excelBuffer = XLSX.write(workbook, {
  //    bookType: 'xlsx',
  //    type: 'array',
  //  });

  //  this.saveAsExcelFile(excelBuffer, 'Tickets Report.xlsx');
  //}
  private mapTicketToRow(t: Ticket) {
    return {
      'id': t.id ?? '',
      'ticketID': t.ticketID ?? '',
      'CustomerID': t.CustomerID ?? '',
      'indice': t.indice ?? '',
      'userID': t.userID ?? '',
      'categoryID': t.categoryID ?? '',
      'subCategoryID': t.subCategoryID ?? '',
      'departmentID': t.departmentID ?? '',
      'channelID': t.channelID ?? '',
      'startDate': formatDate(t.startDate),
      'resolvedDate': formatDate(t.resolvedDate),
      'closedDate': formatDate(t.closedDate),
      'assignedToUserID': t.assingedToUserID ?? '',
      'assignedToBackOfficeID': t.assingedToBackOfficeID ?? '',
      'subject': stripHtml(t.subject),
      'body': stripHtml(t.body),
      'statusID': t.statusID ?? '',
      'priority': t.priority ?? '',
      'escalationLevel': t.escalationLevel ?? '',
      'updateByUser': t.updateByUser ?? '',
      'dueDate': formatDate(t.dueDate),
      'slaDate': formatDate(t.slaDate),
      'emailAlert': t.emailAlert ?? false,
      'flag': t.flag ?? false,
      'mobile': t.mobile ?? '',
      'email': t.email ?? '',
      'customerName': t.customerName ?? '',
      'companyID': t.companyID ?? '',
      'smsAlert': t.smsAlert ?? false,
      'requestID': t.requestID ?? '',
      'departmentReply': stripHtml(t.departmentReply),
      'referenceType': t.referenceType ?? '',
      'referenceNumber': t.referenceNumber ?? ''
    };
  }

  exportExcel() {
    // Export ALL rows currently loaded in the report table
    const source = this.tickets ?? [];
    const rows = source.map(t => this.mapTicketToRow(t));

    const ws = XLSX.utils.json_to_sheet(rows, { header: Object.keys(rows[0] || {}) });
    autosize(ws, rows);

    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Tickets Report');

    const buf = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    FileSaver.saveAs(new Blob([buf], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }), 'Tickets Report.xlsx');
  }


  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }

  generateReport() {

    this.reportData.userID = this.selectedAgent?.user_Id || "0";
    this.reportData.statusID = this.selectedStatus?.statusID || "0";
    this.reportData.departmentID = this.selectedDepartment?.departmentID || "0";
    this.reportData.categoryID = this.selectedCategory?.categoryID || "0";
    this.reportData.levelID = this.selectedPriority?.levelID || "0";
    this.reportData.assignedToUserID = this.selectedBackOffice?.user_Id || "0";
    this.reportData.startDate = this.startDate || new Date(new Date().getFullYear(), 0, 1);
    this.reportData.endDate = this.endDate || new Date(Date.now());
    console.log(this.reportData);
    this.reportService.getSupervisorReport(this.reportData).then(
      (res) => {
        console.log(res);
        this.tickets = res;
        this.isTableVisible = true;
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
    )
  }
  getTicketAssignedUserDescription(assignedToUserId: any): any {
    const user = this.users.find((d) => d.user_Id == assignedToUserId);
    return user ? user.lastname + " " + user.firstname : 'N/A';
  }
  getTicketStatusDescription(statusID: any): any {
    const status = this.statuses.find((d) => d.statusID === statusID);
    return status ? status.description : 'N/A';
  }
  getTicketPriorityDescription(priorityID: any): any {
    const priority = this.priorities.find((d) => d.levelID === priorityID);
    return priority ? priority.description : 'N/A';
  }
  getTicketDepartmentDescription(departmentID: any): any {
    const Department = this.departments.find((d) => d.departmentID === departmentID);
    return Department ? Department.description : 'N/A';
  }
  getTicketCategoryDescription(CategoryID: any): any {

    const Category = this.categories.find((d) => d.categoryID === CategoryID);
    return Category ? Category.description : 'N/A';
  }

}
