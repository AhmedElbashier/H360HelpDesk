import { ConfirmationService, MessageService } from 'primeng/api';
import { Ticket, TicketService } from '../../../services/ticket.service';
import { Router } from '@angular/router';
import { CommonService } from '../../../services/common.service';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { LoggerService } from '../../../services/logger.service';
import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css']
})
export class TicketComponent {
  TicketsDialog!: boolean;
  TicketsEditDialog!: boolean;
  Tickets!: Ticket[];
  Ticket: Ticket = {};
  selectedTickets!: Ticket[];
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  countries!: string[];
  regions!: string[];
  exportColumns!: any[];
  TicketType!: any;
  @ViewChild('dt') dt!: Table;
  visible = false;
  disabled = true;

  constructor(private logger: LoggerService, private common: CommonService, private cdr: ChangeDetectorRef, private TicketService: TicketService, private messageService: MessageService, private confirmationService: ConfirmationService, private router: Router) { }
  applyFilterGlobal(event: any) {
    const searchTerm = (event.target as HTMLInputElement).value;
    if (this.dt) {
      this.dt.filterGlobal(searchTerm, 'contains');
    }
  }
  ngOnInit() {
    this.Delete = "Delete";
    this.getData();
    this.countries = this.common.countries;
    this.regions = this.common.regions;
  }
  getData() {
    this.TicketService.getTickets().subscribe(
      (res: any) => {
        this.Tickets = res;
        this.cdr.detectChanges();
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error")
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

  editTicket(Tickets: Ticket) {
    this.Ticket = { ...Tickets };
    localStorage.setItem("adminTicketEdit", JSON.stringify(this.Ticket));
    this.router.navigateByUrl('main/admin/settings/ticket-edit');

  }
  detailsTicket(Tickets: Ticket) {
    this.Ticket = { ...Tickets };
    localStorage.setItem("adminTicketEdit", JSON.stringify(this.Ticket));
    this.open();
    //this.router.navigateByUrl('main/admin/settings/ticket-details');

  }
  deleteTicket(Tickets: Ticket) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Ticket  ' + Tickets.ticketID + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.TicketService.deleteTicket(Tickets.id).then(
          (res) => {
            this.messageService.add({ severity: 'success', summary: 'done ', detail: 'Ticket Deleted', life: 3000 });
            this.Tickets = this.Tickets.filter(val => val.id !== Tickets.id);
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
    this.TicketsDialog = false;
    this.TicketsEditDialog = false;
    this.submitted = false;
  }
  findIndexById(id: string): number {
    let index = -1;
    for (let i = 0; i < this.Tickets.length; i++) {
      if (this.Tickets[i].id === id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Tickets);
    const workbook = {
      Sheets: { 'Tickets List': worksheet },
      SheetNames: ['Tickets List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Tickets List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
  deleteSelectedTickets() {
    if (this.selectedTickets && this.selectedTickets.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedTickets.map(status => status.id);
          this.TicketService.deleteSelectedTicket(selectedIds).then(
            () => {
              this.selectedTickets = [];
              this.getData();
            },
            (error) => {
              
              this.logger.logError(error);
              this.router.navigateByUrl("error");
            }
          );
        }
      });
    }
  }
  open(): void {
    this.visible = true;
  }

  close(): void {
    this.visible = false;
  }
}
