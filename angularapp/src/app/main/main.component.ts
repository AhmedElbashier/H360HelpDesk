/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
//import { fail } from 'assert';
import { MenuItem, MessageService } from 'primeng/api';
import { Role } from '../services/auth.service';
import { LoggerService } from '../services/logger.service';
import { LoginService } from '../services/login.service';
import { User, UserService } from '../services/user.service';
import { Subscription } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { SettingsService, SmtpSettings, EmailRequest, EscalationTimer, License } from '../services/settings.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {
  constructor(private settingService: SettingsService, private router: Router, private userService: UserService, private login: LoginService, private messageService: MessageService, private loggerService: LoggerService, private translate: TranslateService) {
  }
  user!: User;
  role!: Role;
  isCollapsed = true;
  adminDialog = false;
  smtpDialog = false;
  smtpTestDialog = false;
  escalationTimerDialog = false;
  licenseDialog = false;
  escalationTimers: EscalationTimer []= [];
  escalationTimer: EscalationTimer = {};
  smtps: SmtpSettings[] = [];
  smtp: SmtpSettings = {};
  emailRequest: EmailRequest = {};
  smtpNewFlag: boolean = false;
  Username!: string;
  Password!: string;
  userInfo!: User;
  testEmail!: string;
  updateSubscription!: Subscription;
  lastInteractionTimestamp = new Date().getTime();
  awayTimeout = 5 * 60 * 1000; // 5 minutes
  logoutTimeout = 8 * 60 * 1000; // 8 minutes
  items: MenuItem[] = []; 
  items2: MenuItem[] = [];
  Ticketitems: MenuItem[] = [];
  showAdditionalMenu = false; 
  darkMode = false;
  loading = false;
  license: License = {
    licenseFile: '',
    licenseInfo: {
      key: '',
      company: '',
      vendor: '',
      adminsLimit: 0,
      agentsLimit: 0,
      supervisorsLimit: 0,
      backOfficeLimit: 0,
      expirationDate: '',
    },
  };
  allLicenseUserLimit: any;
  allUsersCount: any;
  allAdminsCount: any;
  allAgentsCount: any;
  allSupervisorsCount: any;
  allBackOfficeCount: any;
  @ViewChild('op') notificationPanel: any; 
  @ViewChild('op2') profilePanel: any;

  async ngOnInit(): Promise<void> {
    this.checkAndClearLogs();
    this.SMTP();
    this.getLicense();
    this.getUsersCount();
    this.user = JSON.parse(localStorage.getItem("user") || "{}") as User;
    const currentDate = new Date();
    //await this.userService.updateLastSeen(this.user.user_Id, currentDate);
    //await this.userService.updateStatus(this.user.user_Id, "Online");
    this.items = [];
    this.items2 = [
      {
        label: this.user.firstname + " " + this.user.lastname,
        //command: () => this.openOverlayPanel("op"),
      },
      {
        label: '', icon: 'pi pi-fw pi-user',
        //command: () => this.openOverlayPanel("op2"),
      },
      {
        label: '', icon: 'pi pi-fw pi-power-off',
        routerLink: ['/login']
      },
    ]
    await this.AddMenuItemsByRoles();
    if (this.user.isAgent) {
      this.router.navigate(["/main/agent/tickets/dashboard"]);
    }
    else if (this.user.isSuperVisor) {
      this.router.navigate(["/main/supervisor/tickets/dashboard"]);
    }
    else if (this.user.isBackOffice) {
      this.router.navigate(["/main/backoffice/tickets/dashboard"]);
    }
  }
  toggleCollapsed(): void {
    this.isCollapsed = !this.isCollapsed;
  }
  async logout() {
    const currentDate = new Date();
    // this.userService.updateLastLogout(this.user.user_Id, currentDate);
    // this.userService.updateLastSeen(this.user.user_Id, currentDate);
    // this.userService.updateStatus(this.user.user_Id, "Offline");
    localStorage.clear();
    this.router.navigate(["login"]);
  }
  checkAndClearLogs(): void {
    const storedTimestamp = localStorage.getItem('userLogs');

    if (storedTimestamp) {
      const currentTime = new Date().getTime();
      const twoHours = 4 * 60 * 60 * 1000; // 2 hours in milliseconds

      if (currentTime - parseInt(storedTimestamp) > twoHours) {
        localStorage.removeItem('userLogs');
      }
    }
  }
  getLicense() {
    this.settingService.getLicense().subscribe(
      (res: any) => {
        this.license = res;
      },
      (error) => {
       
      }
    );
  }
  getUsersCount() {
    this.userService.getCountUsers().subscribe(
      (res: any) => {
        this.allUsersCount = res;
      }
    );
    this.userService.getCountAdmins().subscribe(
      (res: any) => {
        this.allAdminsCount = res;
      }
    );
    this.userService.getCountAgents().subscribe(
      (res: any) => {
        this.allAgentsCount = res;
      }
    );
    this.userService.getCountSupervisors().subscribe(
      (res: any) => {
        this.allSupervisorsCount = res;
      }
    );
    this.userService.getCountBackOffice().subscribe(
      (res: any) => {
        this.allBackOfficeCount = res;
      }
    );
  }
  checkAdminRole() {
    this.adminDialog = true;
  }
  AddMenuItemsByRoles() {

    if (this.user.isAgent) {
      this.items.push(
        {
          label: 'Tickets',
          icon: 'pi pi-fw pi-inbox',
          routerLink: ['/main/agent/tickets/dashboard']

        },
        //{
        //  label: 'Reports',
        //  icon: 'pi pi-fw pi-chart-bar',
        //  routerLink: ['/main/agent/reports']

        //},
      );
    }
    else if (this.user.isSuperVisor) {

      this.items.push(
        {
          label: 'Supervisor Dashboard',
          icon: 'pi pi-fw pi-shield',
          routerLink: ['/main/supervisor/tickets/dashboard']
        },
        {
          label: 'Reports',
          icon: 'pi pi-fw pi-file-pdf',
          routerLink: ['/main/supervisor/reports']
        }
      );
    }
    else if (this.user.isBackOffice) {
      this.items.push(
        {
          label: 'Tickets',
          icon: 'pi pi-fw pi-inbox',
          routerLink: ['/main/backoffice/tickets/dashboard']

        },
        //{
        //  label: 'Reports',
        //  icon: 'pi pi-fw pi-chart-bar',
        //  routerLink: ['/main/backoffice/reports']

        //},
      );

    }
    this.items.push(
    
      {
        label: 'Download User Logs',
        icon: 'pi pi-fw pi-hashtag',
        command: () => this.checkAdminRole()
      },
    );
  }
  confirmRole() {
    this.login.login(this.Username, this.Password).subscribe(
      (token: any) => {
        this.userInfo = token["user"];
        if (this.userInfo.isAdministrator === true) { this.downloadLogs(); }
        else {
          this.router.navigateByUrl("access");
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'You dont have privilge to download user logs - this process requires Admin account', life: 3000 });
        }
      },
      (error) => {
        if (error.status === 401) {
          this.router.navigateByUrl("access");
          this.messageService.add({ severity: 'warn', summary: 'Error', detail: 'Incorrect username or password', life: 3000 });
          this.loggerService.logError(error);
          const userLog = localStorage.getItem('userLogs') || '';
          const updatedUserLog = `${userLog}\n'error'${error}`;
          localStorage.setItem('userLogs', updatedUserLog);
        }
        else if (error.status === 400) {
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Please Enter username & passowrd', life: 3000 });
          this.loggerService.logError(error);
          const userLog = localStorage.getItem('userLogs') || '';
          const updatedUserLog = `${userLog}\n'error'${error}`;
          localStorage.setItem('userLogs', updatedUserLog);
        }
        else {
          this.router.navigateByUrl("error");
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Error occurred during login, Contact Vocalcom Support Team', life: 3000 });
          this.loggerService.logError(error);
          const userLog = localStorage.getItem('userLogs') || '';
          const updatedUserLog = `${userLog}\n'error'${error}`;
          localStorage.setItem('userLogs', updatedUserLog);
        }
      },
      () => {
      }
    );
  }
  hideDialog() {
    this.adminDialog = false;
    this.smtpDialog = false;
    this.smtpTestDialog = false;
    this.escalationTimerDialog = false;
    this.testEmail = "";
    this.smtp = {};
  }
  openSMTPDialog() {
    this.smtpDialog = true;
  }
  setCollapsed(value: boolean) {
    this.isCollapsed = value;
  }
  openEscalationTimerDialog() {
    this.settingService.getEscalationTimers().subscribe(
      (res: any) => {
        this.escalationTimers = res;
        this.escalationTimer = this.escalationTimers[0];
        this.escalationTimerDialog = true;

      },
      (error) =>
      {
        this.loggerService.logError(error);
        this.router.navigateByUrl("error")
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
  downloadLogs() {
    const logs = localStorage.getItem('userLogs');
    if (logs) {
      const dataUri = 'data:text/plain;charset=utf-8,' + encodeURIComponent(logs);

      const link = document.createElement('a');
      link.href = dataUri;
      link.download = 'user-logs.txt';
      link.style.display = 'none';
      document.body.appendChild(link);

      link.click();
      document.body.removeChild(link);
      this.hideDialog();
    }
  }
  language(lang: any) {
    if (lang == "ar")
      this.translate.use('ar');
    else if (lang == "en") {
      this.translate.use('en');
    }
  }
  openOverlayPanel(op: any) {
    if (op === "op")
      this.notificationPanel.toggle(event);
    else
      this.profilePanel.toggle(event);
  }
  async SMTP() {
    await this.settingService.getSmtpSettings().subscribe(
      (res:any) => {
        this.smtps = res;
        this.smtp = { ...this.smtps[0] };
      },
      (error) =>
      {
        this.loggerService.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
      }
    )
    
  }
  SMTPSave(smtp: SmtpSettings) {
      this.settingService.addSmtpSetting(this.smtp).then(
        (res: any) => {
          this.loggerService.logInfo(res);

          this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'SMTP server settings saved succesfully', life: 3000 });

        },
        (error) => {
          this.loggerService.logError(error);
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
  testEmailDialog() {
    this.smtpDialog = false;
    this.smtpTestDialog = true;
  }
  sendTestEmail() {
    // Initialize 'to' as an empty array if it's not already defined
    if (!this.emailRequest.to) {
      this.emailRequest.to = [];
    }

    // Push the test email address as a string to the 'to' array
    this.emailRequest.to.push(this.testEmail);

    this.emailRequest.subject = "SMTP Test Email";
    this.emailRequest.body = "This email sent from Vocalcom-H360 Help-desk for testing SMTP Server Connectivity. </br> Regards, </br> Development Team";

    this.settingService.testEmail(this.emailRequest).subscribe(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Test Success', life: 3000 });
        this.hideDialog();
      },
      (error) => {
        this.loggerService.logError(error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: "Test Failed",
          life: 3000,
        });
      }
    );
  }

  saveEscalationTimer() {
    this.settingService.editEscalationTimer(this.escalationTimer).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'done ', detail: 'Saved Successfully', life: 3000 });
        this.hideDialog();
      },
      (error) => {
        this.loggerService.logError(error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: "Failed",
          life: 3000,
        });
      }
    )
  }
  async openLicenseDialog() {
    this.allLicenseUserLimit = this.license.licenseInfo.adminsLimit + this.license.licenseInfo.agentsLimit + this.license.licenseInfo.supervisorsLimit + this.license.licenseInfo.backOfficeLimit;
    this.licenseDialog = true;
  }
}

