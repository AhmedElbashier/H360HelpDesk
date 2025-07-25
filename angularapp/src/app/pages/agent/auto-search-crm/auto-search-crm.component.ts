import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PolicyService } from '../../../services/policy.service';
import { TicketService } from '../../../services/ticket.service';
import { MessageService } from 'primeng/api';
import { ChangeDetectorRef } from '@angular/core';
import { User } from '../../../services/user.service'; 

@Component({
  selector: 'app-auto-search-crm',
  templateUrl: './auto-search-crm.component.html',
  styleUrls: ['./auto-search-crm.component.css']
})
export class AutoSearchCrmComponent {


  phoneNumber: string = '';
  radioValue: 'P' | 'C' = 'P';
  hasSearched = false;
  customers: any[] = [];
  customer: any = {};
  visible = false;
  size: 'large' = 'large';
  title!: any;
  tickets: any[] = [];
  user!: User;
  loadingCustomers = false;
  loadingTickets = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private policy: PolicyService,
    private ticketservice: TicketService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const storedUser = localStorage.getItem('user');
    if (!storedUser) {
      this.messageService.add({
        severity: 'error',
        summary: 'Unauthorized',
        detail: 'You must be logged in to access this page.',
        life: 3000,
      });
      this.router.navigate(['/login']);
      return;
    }

    this.user = JSON.parse(storedUser) as User;

    // Check if user is agent or allowed role (optional)
    if (!this.user.isAgent && !this.user.isSuperVisor && !this.user.isBackOffice) {
      this.messageService.add({
        severity: 'error',
        summary: 'Access Denied',
        detail: 'You do not have access to this page.',
        life: 3000,
      });
      this.router.navigate(['/access']);
      return;
    }

    this.route.paramMap.subscribe(params => {
      let phone = params.get('phone');
      if (phone) {
        // ✅ Remove leading 0 if present
        if (phone.startsWith('0')) {
          phone = phone.substring(1);
        }

        this.phoneNumber = phone;
        this.autoSearch();
      }
    });

  }

  autoSearch(): void {
    this.searchCustomer();
    this.searchTickets();
  }

  searchCustomer(): void {
    this.loadingCustomers = true;
    const apiCall = this.radioValue === 'P' ? this.policy.callPolicyApi(this.phoneNumber) : this.policy.callClaimApi(this.phoneNumber);
    apiCall.then((res: any) => {
      this.customers = this.radioValue === 'P' ? res?.Policies || [] : res?.Claims || [];
      this.hasSearched = true;
    }).catch(err => {
      console.error(err);
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Error fetching customer info',
        life: 3000,
      });
    }).finally(() => this.loadingCustomers = false);
  }

  searchTickets(): void {
    this.loadingTickets = true;

    this.ticketservice.getTicketsByPhone(this.phoneNumber)
      .then((res: any) => {
        this.tickets = res || [];
      })
      .catch(err => {
        console.error(err);
        this.tickets = []; // ✅ Ensure this is always an array

        this.messageService.add({
          severity: 'info',
          summary: 'No Tickets Found',
          detail: 'No previous tickets were found for this customer. You can open one.',
          life: 4000
        });
      })
      .finally(() => {
        this.loadingTickets = false;
      });
  }


  recordSelect(customer: any) {
    this.customer = customer;
    this.title = this.radioValue === 'P' ? 'Policy Details' : 'Claim Details';
    this.visible = true;
  }

  openTicket(customer: any) {
    if (!this.user.isAgent) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Not Allowed',
        detail: 'Only agents can create new tickets.',
        life: 3000,
      });
      return;
    }

    this.cdr.detectChanges();
    const customerdetails = [{ ...customer, type: this.radioValue }];
    localStorage.setItem("CustomerDetails", JSON.stringify(customerdetails));
    this.router.navigateByUrl("main/agent/tickets/new");
  }
  close(): void {
    this.visible = false;
  }

  get tableHeaders(): string[] {
    return this.radioValue === 'P' ? ['Policy No', 'Insured Name', 'Mobile', 'Status'] : ['Claim File No', 'Owner Name', 'Mobile', 'Date', 'Claim No', 'Claimant', 'Status'];
  }
}
