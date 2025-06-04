import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-supervisor-search-crm',
  templateUrl: './supervisor-search-crm.component.html',
  styleUrls: ['./supervisor-search-crm.component.css']
})
export class SupervisorSearchCrmComponent {
  policyID: any;
  phoneNumber: any;
  customer: any = {};
  customers: any[] = [];

  constructor(private router: Router, private http: HttpClient, private cdr: ChangeDetectorRef, private messageService: MessageService) { }
  products: any;
  nonCustomer() {
    this.router.navigateByUrl("main/supervisor/tickets/new");
  }
  searchCustomer() {

    this.callPolicyApi(this.phoneNumber).then(
      (res: any) => {
        console.log(res),
          this.customers = res.Policies;
      },
      (error) => {
        console.log(error),
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail:
              "An error occurred while fetching customer data",
            life: 3000,
          });
      }
    );
  }
  callPolicyApi(phoneNumber: string): Promise<any> {
    // Set the query parameters, including the phone number
    const apiBaseUrl = 'http://call-srv-app.burujinsurance.com:7000/api/getPolData';
    const username = 'call_center';
    const password = 'cc@p@s$WrD';
    const queryParams = `?P_LOOKUP_TYPE=1&P_MOBILE=${phoneNumber}`;

    // Set the headers with Basic Authentication
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Basic ${btoa(`${username}:${password}`)}`, // Encode username and password in base64
    });

    // Make the HTTP GET request with headers and query parameters
    return this.http
      .get<any>(`${apiBaseUrl}${queryParams}`, { headers })
      .toPromise();
  }

  recordSelect(customer: any) {
    this.cdr.detectChanges();
    const customerdetails = [customer];
    localStorage.setItem("CustomerDetails", JSON.stringify(customerdetails));
    this.router.navigateByUrl("main/supervisor/tickets/new");


  }
}

