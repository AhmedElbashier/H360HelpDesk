/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable no-cond-assign */
/* eslint-disable no-constant-condition */
/* eslint-disable no-dupe-else-if */
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Message, MessageService } from 'primeng/api';
import { AuthService } from '../services/auth.service';
import { ClientInfo, LoggerService } from '../services/logger.service';
import { LoginService } from '../services/login.service';
import { User, UserService } from '../services/user.service';

export interface Login {
  username: string;
  password: string;
}
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [`
        :host ::ng-deep .pi-eye,
        :host ::ng-deep .pi-eye-slash {
            transform:scale(1.6);
            margin-right: 1rem;
            color: var(--primary-color) !important;
        }
    `]
})
export class LoginComponent {
  constructor(
    private route: Router,
    private messageService: MessageService,
    private login: LoginService,
    private userService: UserService,
    private auth: AuthService,
    private loggerService: LoggerService
  ) { }
  users!: User[];
  username: any;
  password: any;
  station: any;
  showPassword: boolean = false;
  messages!: Message[];
  data: any = {};
  tokenData = "";
  userInfo!: User;
  clientInfo!: ClientInfo;

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  ngOnInit(): void {
    localStorage.clear();
    if (this.auth.isAuthenticated()) this.route.navigate(["main"]);
    this.clientInfo = {
      ipAddress: "",
      hostname: "",
    }
    //this.login.getClientInfo().subscribe(
    //  (res: any) => {
    //    this.clientInfo = res;
    //  },
    //  (error) => {
    //    this.loggerService.logError(error);
    //  }
    //);
  }
  Login() {
    //this.route.navigateByUrl('/main/agent/dashboard');

    this.login.login(this.username, this.password).subscribe(
      (token: any) => {

        this.tokenData = token["token"];
        this.userInfo = token["user"];
        this.userInfo.ipAddress = this.clientInfo.ipAddress;
        this.userInfo.hostName = this.clientInfo.hostname;
        this.userInfo.status = "Online";
        localStorage.setItem("token", this.tokenData);
        localStorage.setItem("user", JSON.stringify(this.userInfo));
        this.route.navigateByUrl('/main');
      },
      (error) => {
        if (error.status === 401) {
          this.route.navigateByUrl("access")
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
          this.route.navigateByUrl("error")
          this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Error occurred during login, Contact Vocalcom Support Team', life: 3000 });
          this.loggerService.logError(error);
          const userLog = localStorage.getItem('userLogs') || '';
          const updatedUserLog = `${userLog}\n'error'${error}`;
          localStorage.setItem('userLogs', updatedUserLog);
        }
      },
      () => { }
    );
  }
}
