import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { Ticket, TicketService } from '../../../services/ticket.service';
import { MegaMenuItem, MenuItem, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { LoggerService } from '../../../services/logger.service';
import { Table } from 'primeng/table';
import { Priority, SettingsService } from '../../../services/settings.service';
import { User, UserService } from '../../../services/user.service';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
@Component({
  selector: 'app-backoffice-dashboard',
  templateUrl: './backoffice-dashboard.component.html',
  styleUrls: ['./backoffice-dashboard.component.css']
})
export class BackofficeDashboardComponent {

  assignedTickets: any;
  unAnsweredTickets: any;
  inPorgressTickets: any;
  resolvedTickets: any;
  closedTickets: any;
  allTickets: any;
  tickets!: Ticket[];
  newTicket: Ticket = {};
  user: User = {};
  visible: boolean = false;
  visible2: boolean = false;
  loading: boolean = false;
  disabled: boolean = true;
  items: MegaMenuItem[] = [];
  representatives!: any[];
  categories!: any[];
  statuses!: any[];
  priorities!: any[];
  priority!: any;
  selectedCategory: any;
  breakpoint: any = "xxl";
  value: any;
  buttonStyle = {
    'left': '20',
    'margin': '10px',
  };
  @ViewChild('dt') dt!: Table;
  @ViewChild('dt2') dt2!: Table;
  filteredData: any;
  startDate: any;
  endDate: any;
  filteredPriorities: any[] = [];
  selectedPriority: any;
  agents: any[] = [];
  backofficeUsers: any[] = [];

  selectedAgent: any = null;
  selectedBackoffice: any = null;

  constructor(private settingService: SettingsService, private logger: LoggerService, private router: Router, private cdr: ChangeDetectorRef, private ticketService: TicketService, private messageService: MessageService, private userService : UserService) {
  }

  ngOnInit() {
    this.user = JSON.parse(localStorage.getItem("user") || "{}") as User;
    this.representatives = [
      { name: 'Amy Elsner', image: 'amyelsner.png' },
      { name: 'Anna Fali', image: 'annafali.png' },
      { name: 'Asiya Javayant', image: 'asiyajavayant.png' },
      { name: 'Bernardo Dominic', image: 'bernardodominic.png' },
      { name: 'Elwin Sharvill', image: 'elwinsharvill.png' },
      { name: 'Ioni Bowcher', image: 'ionibowcher.png' },
      { name: 'Ivan Magalhaes', image: 'ivanmagalhaes.png' },
      { name: 'Onyama Limba', image: 'onyamalimba.png' },
      { name: 'Stephen Shaw', image: 'stephenshaw.png' },
      { name: 'Xuxue Feng', image: 'xuxuefeng.png' }
    ];
    this.userService.getBackOfficeUsers().subscribe(res => {
      this.backofficeUsers = res.map(user => ({
        ...user,
        id: user.user_Id,
        fullname: `${user.firstname} ${user.lastname}`
      }));
      this.cdr.detectChanges();
    });

    this.userService.getAgentUsers().subscribe(res => {
      this.agents = res.map(agent => ({
        ...agent,
        id: agent.user_Id,
        fullname: `${agent.firstname} ${agent.lastname}`
      }));
      this.cdr.detectChanges();
    });

    this.getTotals(this.user.department_Id, this.user.user_Id);
    this.settingService.getStatuses().subscribe(
      (res: any) => {
        this.statuses = res;
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
    this.settingService.getPriorites().subscribe(
      (res: any) => {
        this.priorities = res;
        this.cdr.detectChanges();
        this.filteredPriorities = this.priorities.slice();

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
    this.settingService.getCategories().subscribe(
      (res: any) => {
        this.categories = res;
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

    this.getAllTickets();
  }
  applyFilterGlobal(event: any) {
    const searchTerm = (event.target as HTMLInputElement).value;
    if (this.dt2) {
      this.dt2.filterGlobal(searchTerm, 'contains');
    }
  }
  applyFilterGlobal2(event: any) {
    const searchTerm = event.target.value.toLowerCase();

    // Filter priorities based on the user's input
    this.filteredPriorities = this.priorities.filter((priority) =>
      priority.description.toLowerCase().includes(searchTerm)
    );
  }
  getPriorityDescription(levelID: string): string {
    const matchingPriority = this.priorities.find((priority) => priority.levelID === levelID);
    return matchingPriority ? matchingPriority.description : '';
  }

  getSeverity(status: any) {
    if (status === 1) {
      return 'success';
    }
    else if (status === 2) {
      return 'warning';

    }
    else if (status === 3) {
      return 'danger';
    }
    else if (status === 4) {
      return 'seconday';
    }
    else {
      return 'unknown'; // You can choose an appropriate value for unhandled cases
    }
  }
  getPrioritySeverity(priority: any) {
    if (priority === 1) {
      return 'success';
    }
    else if (priority === 2) {
      return 'warning';

    }
    else if (priority === 3) {
      return 'danger';
    }
    else if (priority === 4) {
      return 'secondary';
    }
    else {
      return 'unknown'; // You can choose an appropriate value for unhandled cases
    }

  }
  //createTicket() {
  //  this.router.navigateByUrl("main/agent/tickets/new");
  //  this.router.navigateByUrl("main/agent/tickets/crm");
  //}

  getTotals(departmentID : any, userID: any) {
    this.ticketService.getBackOfficeClosedTotal(departmentID).then(
      (res: any) => {
        if (res === null) {
          this.closedTickets = "0";
        }
        this.closedTickets = res;
        this.ticketService.getBackOfficeUnAnsweredTotal(departmentID).then(
          (res: any) => {
            if (res === null) {
              this.unAnsweredTickets = "0";
            }
            this.unAnsweredTickets = res;
            this.ticketService.getBackOfficeInProgressTotal(departmentID).then(
              (res: any) => {
                if (res === null) {
                  this.inPorgressTickets = "0";
                }
                this.inPorgressTickets = res;
                this.ticketService.getBackOfficeResolvedTotal(departmentID).then(
                  (res: any) => {
                    this.resolvedTickets = res;
                    if (res === null) {
                      this.resolvedTickets = "0";
                    }
                    this.ticketService.getBackkOfficeAssignedToUserTotal(userID).then(
                      (res: any) => {
                        if (res === null) {
                          this.assignedTickets = "0";

                        }
                        this.assignedTickets = res;
                      });
                  });
              });
          });
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
    )
  }
  getTicketStatusDescription(statusID: any): any {
    const status = this.statuses.find((d) => d?.statusID === statusID);
    return status ? status.description : 'N/A';
  }
  getTicketPriorityDescription(priorityID: any): any {
    const priority = this.priorities.find((d) => d?.levelID === priorityID);
    return priority ? priority.description : 'N/A';
  }
  getTicketCategoryDescription(CategoryID: any): any {
    const Category = this.categories.find((d) => d?.categoryID === CategoryID);
    return Category ? Category.description : 'N/A';
  }
  ticektDetails(ticket: Ticket) {
    localStorage.setItem("BackOfficeTicketDetails", JSON.stringify(ticket));
    this.router.navigateByUrl("main/backoffice/tickets/details");

  }

  applyDateFilter() {
  }

  getNewTickets() {
    this.ticketService.getBackOfficeUnAnswered(this.user.department_Id).then(
      (res: any) => {
        this.tickets = res;
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
    )
  }
  getResolvedTickets() {
    this.ticketService.getBackOfficeResolved(this.user.department_Id).then(
      (res: any) => {
        this.tickets = res;
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
    )
  }
  getOpenTickets() {
    this.ticketService.getBackOfficeInProgress(this.user.department_Id).then(
      (res: any) => {
        this.tickets = res;
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
    )
  }
  getAssignedTickets() {
    this.ticketService.getBackOfficeAssignedToUser(this.user.user_Id).then(
      (res: any) => {
        this.tickets = res;
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
    )
  }
  getCloseTickets() {
    this.ticketService.getBackOfficeClosed(this.user.department_Id).then(
      (res: any) => {
        this.tickets = res;
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
    )
  }
  getAllTickets() {
    this.ticketService.getBackOfficeAll(this.user.department_Id).then(
      (res: any) => {
        this.tickets = res;
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
    )
  }
  filterTickets() {
    this.tickets = this.tickets.filter(ticket => {
      const priorityMatch = !this.selectedPriority || ticket.priority === this.selectedPriority.levelID;
      const dateMatch = !this.startDate || (ticket.startDate && new Date(ticket.startDate) >= new Date(this.startDate));
      const agentMatch = !this.selectedAgent || +(ticket?.assingedToUserID ?? 0) === +this.selectedAgent.id;
      const backofficeMatch = !this.selectedBackoffice || +(ticket?.assingedToBackOfficeID ?? 0) === +this.selectedBackoffice.id;
      return priorityMatch && dateMatch && agentMatch && backofficeMatch;
    });
    this.cdr.detectChanges();
  }


  resetFilters() {
    this.selectedPriority = null;
    this.selectedAgent = null;
    this.selectedBackoffice = null;
    this.startDate = null;
    this.getAllTickets();
  }


  exportExcel() {
    const exportData = this.tickets.map(ticket => ({
      'ID': ticket.id,
      'Subject': ticket.subject,
      'Priority': this.getTicketPriorityDescription(ticket.priority),
      'Status': this.getTicketStatusDescription(ticket.statusID),
      'Entry Date': ticket.startDate ? new Date(ticket.startDate).toLocaleString() : '',
      'Due Date': ticket.dueDate ? new Date(ticket.dueDate).toLocaleString() : '',
      'Category': this.getTicketCategoryDescription(ticket.categoryID)
    }));

    const worksheet = XLSX.utils.json_to_sheet(exportData);
    const workbook = {
      Sheets: { 'Tickets': worksheet },
      SheetNames: ['Tickets'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, `Tickets_${new Date().toISOString().split('T')[0]}.xlsx`);
  }

  saveAsExcelFile(buffer: any, fileName: string) {
    const blob = new Blob([buffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    });
    FileSaver.saveAs(blob, fileName);
  }
  getUserName(userID: number): string {
    const user = [...this.agents, ...this.backofficeUsers].find(u => +u.user_Id === +userID);
    return user ? `${user.firstname} ${user.lastname}` : 'N/A';
  }

}

