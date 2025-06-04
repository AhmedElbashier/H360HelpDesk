//import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
//import { ChangeDetectorRef, Component } from '@angular/core';
//import { Router } from '@angular/router';
//import { MessageService } from 'primeng/api';
//import { PolicyService } from '../../../services/policy.service';

//@Component({
//  selector: 'app-search-crm',
//  templateUrl: './search-crm.component.html',
//  styleUrls: ['./search-crm.component.css']
//})
//export class SearchCrmComponent {

//  policyID: any;
//  phoneNumber: any;
//  customer: any = {};
//  customers: any[] = [];
//  radioValue: any;

//  constructor(private router: Router, private http: HttpClient, private cdr: ChangeDetectorRef, private messageService: MessageService, private policy: PolicyService) { }
//  products: any;
//  visible = false;
//  size: 'large' | 'default' = 'default';
//  title!: 'Policy Details'
//  nonCustomer() {
//    this.router.navigateByUrl("main/agent/tickets/new");
//  }
//  searchCustomer() {
//    console.log(this.radioValue)
//    if (this.radioValue === 'P') {
//      this.policy.callPolicyApi(this.phoneNumber).then(
//        (res: any) => {
//          console.log(res),
//            this.customers = res.Policies;
//        },
//        (error) => {
//          console.log(error),
//            this.messageService.add({
//              severity: 'error',
//              summary: 'Error',
//              detail:
//                "An error occurred while fetching customer data",
//              life: 3000,
//            });
//        }
//      );
//    }
//    else if (this.radioValue === 'C') {
//      this.policy.callClaimApi(this.phoneNumber).then(
//        (res: any) => {
//          console.log(res),
//            this.customers = res.Policies;
//        },
//        (error) => {
//          console.log(error),
//            this.messageService.add({
//              severity: 'error',
//              summary: 'Error',
//              detail:
//                "An error occurred while fetching customer data",
//              life: 3000,
//            });
//        }
//      );
//    }
//    //callPolicyApi(phoneNumber: string): Promise<any> {
//    //  // Set the query parameters, including the phone number
//    //  const apiBaseUrl = 'http://call-srv-app.burujinsurance.com:7000/api/getPolData';
//    //  const username = 'call_center';
//    //  const password = 'cc@p@s$WrD';
//    //  const queryParams = `?P_LOOKUP_TYPE=1&P_MOBILE=${phoneNumber}`;

//    //  // Set the headers with Basic Authentication
//    //  const headers = new HttpHeaders({
//    //    'Content-Type': 'application/json',
//    //    Authorization: `Basic ${btoa(`${username}:${password}`)}`, // Encode username and password in base64
//    //  });

//    //  // Make the HTTP GET request with headers and query parameters
//    //  return this.http
//    //    .get<any>(`${apiBaseUrl}${queryParams}`, { headers })
//    //    .toPromise();
//    //}
//  }

//recordSelect(customer: any) {
//  this.cdr.detectChanges();
//  const customerdetails = [customer];
//  console.log(customerdetails);
//  localStorage.setItem("CustomerDetails", JSON.stringify(customerdetails));
//  this.router.navigateByUrl("main/agent/tickets/new");


//}


//showDefault(): void {
//  this.size = 'default';
//  this.open();
//}

//showLarge(): void {
//  this.size = 'large';
//  this.open();
//}

//open(): void {
//  this.visible = true;
//}

//close(): void {
//  this.visible = false;
//}
//}




import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { PolicyService } from '../../../services/policy.service';

@Component({
  selector: 'app-search-crm',
  templateUrl: './search-crm.component.html',
  styleUrls: ['./search-crm.component.css']
})
export class SearchCrmComponent {
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
      this.router.navigateByUrl("main/agent/tickets/new");
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
        '',                 // Empty placeholder for Claim File No
        'Insured Name',     // INSURED_NAME
        'Mobile No',        // MBILE_NO
        'Policy Status',    // POLICY_STATUS
        '',                 // No Accident Date
        '',                 // No Claimant
        'Action'
      ];
    } else {
      return [
        '',                    // No Policy No
        'Claim File No',       // CLAIM_FILE_NO
        'Owner Name',          // OWNER_NAME
        'Mobile No',           // OWNER_MOBILE_NO
        'Claim Status',        // from REJECTION_REASON or logic
        'Accident Date',       // ACCIDENT_DATE
        'Claimant',            // APPLIER_NAME
        'Action'
      ];
    }
  }

  recordSelect(customer: any) {
    this.customer = customer;
    this.title = this.radioValue === 'P' ? 'Policy Details' : 'Claim Details';
    this.visible = true;
  }

showDefault(): void {
  this.size = 'large';
  this.open();
}

showLarge(): void {
  this.size = 'large';
  this.open();
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
      this.router.navigateByUrl("main/agent/tickets/new");
  }
}
