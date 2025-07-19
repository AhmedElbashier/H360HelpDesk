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
  selector: 'app-agent-dashboard',
  templateUrl: './agent-dashboard.component.html',
  styleUrls: ['./agent-dashboard.component.css']
})
export class AgentDashboardComponent {
  newTickets: any;
  openedTickets: any;
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
  selectedStatus: any = null;
  originalTickets: Ticket[] = []; // to preserve unfiltered list
  assignedToMeTickets: any = 0;
  agents: any[] = []; // Populate with { name: 'Ahmed', id: 1 }
  backOffices: any[] = []; // Populate with { name: 'Ahmed', id: 1 }
selectedAgent: any = null;

  constructor(private settingService: SettingsService, private logger: LoggerService, private router: Router, private cdr: ChangeDetectorRef, private ticketService: TicketService, private messageService: MessageService,private userService:UserService) {
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
    this.userService.getAgentUsers().subscribe((res: any[]) => {

      this.agents = res.map(agent => ({
        ...agent,
        id: agent.user_Id, // ✅ Needed for dropdown value binding
        fullname: `${agent.firstname} ${agent.lastname}`
      }));
     


      this.cdr.detectChanges();
    });
    this.userService.getBackOfficeUsers().subscribe((res: any[]) => {

      this.backOffices = res.map(agent => ({
        ...agent,
        id: agent.user_Id, // ✅ Needed for dropdown value binding
        fullname: `${agent.firstname} ${agent.lastname}`
      }));
      this.cdr.detectChanges();
    });

    this.getTotals(this.user.user_Id);
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

    this.ticketService.getTickets(this.user.user_Id).subscribe(
      (res: any) => {
        this.tickets = res;
        this.originalTickets = res; // store full list
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
      return 'seconday';
    }
    else {
      return 'unknown'; // You can choose an appropriate value for unhandled cases
    }

  }
  createTicket() {
    //this.router.navigateByUrl("main/agent/tickets/new");
    this.router.navigateByUrl("main/agent/tickets/crm");

  }
  getTotals(id:any) {
    this.ticketService.getTotalClosed(id).then(
      (res: any) => {
        if (res === null) {
          this.closedTickets = "0";
        }
        this.closedTickets = res;
        this.ticketService.getTotalOpened(id).then(
          (res: any) => {
            if (res === null) {
              this.openedTickets = "0";
            }
            this.openedTickets = res;
            this.ticketService.getTotalNew(id).then(
              (res: any) => {
                this.newTickets = res;
                this.ticketService.getTotalResolved(id).then(
                  (res: any) => {
                    this.resolvedTickets = res;
                    this.ticketService.getAssignedToUser(id).then(
                      (res: any) => {
                       this.assignedToMeTickets = res.length;

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
    const status = this.statuses.find((d) => d.statusID === statusID);
    return status ? status.description : 'N/A';
  }
  getTicketPriorityDescription(priorityID: any): any {
    const priority = this.priorities.find((d) => d.levelID === priorityID);
    return priority ? priority.description : 'N/A';
  }
  getTicketCategoryDescription(CategoryID: any): any {
    const Category = this.categories.find((d) => d.categoryID === CategoryID);
    return Category ? Category.description : 'N/A';
  }
  ticektDetails(ticket: Ticket) {
    localStorage.setItem("TicketDetails", JSON.stringify(ticket));
    this.router.navigateByUrl("main/agent/tickets/details");

  }

  applyDateFilter() {
  }

  getNewTickets() {
    this.ticketService.getNew(this.user.user_Id).then(
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
    this.ticketService.getResolved(this.user.user_Id).then(
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
    this.ticketService.getOpened(this.user.user_Id).then(
      (res: any) => {
        this.tickets = res;
        this.openedTickets = res.length;
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
    this.ticketService.getAssignedToUser(this.user.user_Id).then(
      (res: any) => {
        this.tickets = res;
        this.cdr.detectChanges();
      },
      (error) => {
        this.logger.logError(error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load assigned tickets' });
      }
    );
  }






  getCloseTickets() {
    this.ticketService.getClosed(this.user.user_Id).then(
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
    this.ticketService.getTickets(this.user.user_Id).subscribe(
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

  filterByPriority(priorityValue: any) {
    if (priorityValue) {
      this.tickets = this.tickets.filter(ticket => ticket.priority === priorityValue.levelID);
    } else {
      this.getAllTickets(); // Or reload original unfiltered list
    }
  }

  filterByStatus(statusValue: any) {
    if (statusValue) {
      this.tickets = this.tickets.filter(ticket => ticket.statusID === statusValue.statusID);
    } else {
      this.getAllTickets(); // Or reload original unfiltered list
    }
  }
  filterTickets() {
    this.tickets = this.originalTickets.filter(ticket => {
      const priorityMatch = !this.selectedPriority || ticket.priority === this.selectedPriority.levelID;
      const statusMatch = !this.selectedStatus || ticket.statusID === this.selectedStatus.statusID;
      const dateMatch = !this.startDate || (
        ticket.startDate && new Date(ticket.startDate) >= new Date(this.startDate as Date)
      );
      const agentMatch =
        !this.selectedAgent ||
        (ticket.assingedToUserID != null && +ticket.assingedToUserID === +this.selectedAgent.id);

      return priorityMatch && statusMatch && dateMatch && agentMatch;
    });

    this.cdr.detectChanges();
  }

  resetFilters() {
    this.selectedAgent = null;
    this.selectedPriority = null;
    this.selectedStatus = null;
    this.startDate = null;
    this.tickets = [...this.originalTickets];
    this.cdr.detectChanges();
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
    const user = [...this.agents, ...this.backOffices].find(u => +u.user_Id === +userID);
    return user ? `${user.firstname} ${user.lastname}` : 'N/A';
  }

}
