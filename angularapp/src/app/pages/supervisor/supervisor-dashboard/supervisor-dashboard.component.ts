import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { Ticket, TicketService } from '../../../services/ticket.service';
import { MegaMenuItem, MenuItem, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { LoggerService } from '../../../services/logger.service';
import { Table } from 'primeng/table';
import { Priority, SettingsService } from '../../../services/settings.service';
import { User } from '../../../services/user.service';

@Component({
  selector: 'app-supervisor-dashboard',
  templateUrl: './supervisor-dashboard.component.html',
  styleUrls: ['./supervisor-dashboard.component.css']
})
export class SupervisorDashboardComponent {

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

  constructor(private settingService: SettingsService, private logger: LoggerService, private router: Router, private cdr: ChangeDetectorRef, private ticketService: TicketService, private messageService: MessageService) {
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

    this.ticketService.getTickets().subscribe(
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
    const matchingPriority = this.priorities?.find((priority) => priority?.levelID === levelID);
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
    this.router.navigateByUrl("main/supervisor/tickets/crm");

  }
  getTotals(id: any) {
    this.ticketService.getSupervisorClosedTotal(id).then(
      (res: any) => {
        if (res === null) {
          this.closedTickets = "0";
        }
        this.closedTickets = res;
        this.ticketService.getSupervisorInProgressTotal(id).then(
          (res: any) => {
            if (res === null) {
              this.openedTickets = "0";
            }
            this.openedTickets = res;
            this.ticketService.getSupervisorNewTotal().then(
              (res: any) => {
                this.newTickets = res;
                this.ticketService.getSupervisorResolvedTotal(id).then(
                  (res: any) => {
                    this.resolvedTickets = res;
                    this.ticketService.getSupervisorAllTotal(id).then(
                      (res: any) => {
                        this.allTickets = res;
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
    const status = this.statuses?.find((d) => d?.statusID === statusID);
    return status ? status.description : 'N/A';
  }
  getTicketPriorityDescription(priorityID: any): any {
    const priority = this.priorities?.find((d) => d?.levelID === priorityID);
    return priority ? priority.description : 'N/A';
  }
  getTicketCategoryDescription(CategoryID: any): any {
    const Category = this.categories?.find((d) => d?.categoryID === CategoryID);
    return Category ? Category.description : 'N/A';
  }
  ticektDetails(ticket: Ticket) {
    localStorage.setItem("SupervisorTicketDetails", JSON.stringify(ticket));
    this.router.navigateByUrl("main/supervisor/tickets/details");

  }

  applyDateFilter() {
  }

  getNewTickets() {
    this.ticketService.getSupervisorNew().then(
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
    this.ticketService.getSupervisorResolved().then(
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
    this.ticketService.getSupervisorOpened().then(
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
    this.ticketService.getuserTicketsDashboard(this.user.user_Id).then(
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
    this.ticketService.getSupervisorClosed().then(
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

}
