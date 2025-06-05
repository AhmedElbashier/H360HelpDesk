import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { PolicyService } from '../../../services/policy.service';

@Component({
  selector: 'app-supervisor-search-crm',
  templateUrl: './supervisor-search-crm.component.html',
  styleUrls: ['./supervisor-search-crm.component.css']
})
export class SupervisorSearchCrmComponent {
//  policyID: any;
//  phoneNumber: any;
//  customer: any = {};
//  customers: any[] = [];

//  constructor(private router: Router, private http: HttpClient, private cdr: ChangeDetectorRef, private messageService: MessageService) { }
//  products: any;
//  nonCustomer() {
//    this.router.navigateByUrl("main/supervisor/tickets/new");
//  }
//  searchCustomer() {

//    this.callPolicyApi(this.phoneNumber).then(
//      (res: any) => {
//        console.log(res),
//          this.customers = res.Policies;
//      },
//      (error) => {
//        console.log(error),
//          this.messageService.add({
//            severity: 'error',
//            summary: 'Error',
//            detail:
//              "An error occurred while fetching customer data",
//            life: 3000,
//          });
//      }
//    );
//  }
//  callPolicyApi(phoneNumber: string): Promise<any> {
//    // Set the query parameters, including the phone number
//    const apiBaseUrl = 'http://call-srv-app.burujinsurance.com:7000/api/getPolData';
//    const username = 'call_center';
//    const password = 'cc@p@s$WrD';
//    const queryParams = `?P_LOOKUP_TYPE=1&P_MOBILE=${phoneNumber}`;

//    // Set the headers with Basic Authentication
//    const headers = new HttpHeaders({
//      'Content-Type': 'application/json',
//      Authorization: `Basic ${btoa(`${username}:${password}`)}`, // Encode username and password in base64
//    });

//    // Make the HTTP GET request with headers and query parameters
//    return this.http
//      .get<any>(`${apiBaseUrl}${queryParams}`, { headers })
//      .toPromise();
//  }

//  recordSelect(customer: any) {
//    this.cdr.detectChanges();
//    const customerdetails = [customer];
//    localStorage.setItem("CustomerDetails", JSON.stringify(customerdetails));
//    this.router.navigateByUrl("main/supervisor/tickets/new");


//  }
//}


  constructor(private router: Router, private http: HttpClient, private cdr: ChangeDetectorRef, private messageService: MessageService, private policy: PolicyService) { }
  policyID: any;
  phoneNumber: any;
  customer: any = {};
  customers: any[] = [];
  radioValue: 'P' | 'C' = 'P'; // default to 'P'
  hasSearched: boolean = false;
  products: any;
  visible = false;
  //size: 'large' | 'default' = 'default';
  size: 'large' = 'large'; // Always large
  title!: any;

  nonCustomer() {
    localStorage.removeItem("CustomerDetails");
    this.router.navigateByUrl("main/supervisor/tickets/new");
  }

  searchCustomer() {
    this.hasSearched = false;
    this.customers = [];

    if (!this.phoneNumber) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Input Required',
        detail: 'Please enter a phone number.',
        life: 3000,
      });
      return;
    }

    const apiCall = this.radioValue === 'P' ? this.policy.callPolicyApi(this.phoneNumber) : this.policy.callClaimApi(this.phoneNumber);

    apiCall.then((res: any) => {
      // Choose the correct response field
      this.customers = this.radioValue === 'P' ? res?.Policies || [] : res?.Claims || [];
      this.hasSearched = true;
    }).catch(error => {
      console.log(error);
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'An error occurred while fetching customer data',
        life: 3000,
      });
    });
  }
  get tableHeaders(): string[] {
    if (this.radioValue === 'P') {
      return [
        'Policy No',        // POLICY_NUMBER
        'Insured Name',     // INSURED_NAME
        'Mobile No',        // MBILE_NO
        'Policy Status',    // POLICY_STATUS
      ];
    } else {
      return [
        'Claim File No',       // CLAIM_FILE_NO
        'Owner Name',          // OWNER_NAME
        'Mobile No',           // OWNER_MOBILE_NO
        'Date',        // from REJECTION_REASON or logic
        'Claim No',       // ACCIDENT_DATE
        'Claimant',            // APPLIER_NAME
        'Status',            // Status
      ];
    }
  }

  recordSelect(customer: any) {
    this.customer = customer;
    this.title = this.radioValue === 'P' ? 'Policy Details' : 'Claim Details';
    this.visible = true;
  }


  open(): void {
    this.visible = true;
  }

  close(): void {
    this.visible = false;
  }

  openTicket(customer: any) {
    this.cdr.detectChanges();
    const customerdetails = [{ ...customer, type: this.radioValue }];

    console.log(customerdetails);
    localStorage.setItem("CustomerDetails", JSON.stringify(customerdetails));
    this.router.navigateByUrl("main/supervisor/tickets/new");
  }
}


