import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Company, Department, SettingsService } from '../../../services/settings.service';
import { User, UserService } from '../../../services/user.service';
import { LoggerService } from '../../../services/logger.service';
import { MessageService } from 'primeng/api';
import { Location } from '@angular/common';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent {

  roles: any;
  selectedRole: any;
  selectedCompany: any;
  selectedUserGroup: any;
  selectedLanguage: any;
  User: User = {};
  Companies!: Company[];
  Company: Company = {};
  Department: Department = {};
  selectedDepartment: Department = {};
  Departments!: Department[];
  Languages: any;
  constructor(private router: Router, private location: Location, private settingService: SettingsService, private userService: UserService, private logger: LoggerService, private messageService: MessageService) { }

  ngOnInit() {
    this.roles =
      [
        { id: 1, name: "Administartor" },
        { id: 2, name: "Supervisor" },
        { id: 3, name: "BackOffice" },
        { id: 4, name: "Agent" },
      ];
    this.Languages =
      [
        { id: 1, name: "English" },
        { id: 2, name: "Arabic" },
      ];
    this.settingService.getCompanies().subscribe(
      (res: any) => {
        this.Companies = res;
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
      }
    );
    this.settingService.getDepartments().subscribe(
      (res: any) => {
        this.Departments = res;
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
      }
    );
    this.User = JSON.parse(localStorage.getItem("adminUserEdit") || "{}") as User;
    if (this.User.isAdministrator) {
      this.selectedRole = "Administrator";
    }
    else if (this.User.isSuperVisor) {
      this.selectedRole = "Supervisor";
    }
    else if (this.User.isBackOffice) {
      this.selectedRole = "BackOffice";
    }
    else if (this.User.isAgent) {
      this.selectedRole = "Agent";
    }
  }
  onTryAgainClick() {
    this.location.back();
  }
  submit(User: User) {
    const currentDate = new Date();

    if (this.selectedRole == "Administrator") {
      this.User.isAdministrator = true;
      this.User.isSuperVisor = false;
      this.User.isAgent = false;
      this.User.isBackOffice = false;
    }
    else if (this.selectedRole == "Supervisor") {
      this.User.isAdministrator = false;
      this.User.isSuperVisor = true;
      this.User.isAgent = false;
      this.User.isBackOffice = false;
    }
    else if (this.selectedRole == "BackOffice") {
      this.User.isAdministrator = false;
      this.User.isSuperVisor = false;
      this.User.isAgent = false;
      this.User.isBackOffice = true;
    }
    else if (this.selectedRole == "Agent") {
      this.User.isAdministrator = false;
      this.User.isSuperVisor = false;
      this.User.isAgent = true;
      this.User.isBackOffice = false;
    }
    User.lastSeen = currentDate.toISOString();
    User.ipAddress = "Not defined";
    User.hostName = "Not defined";
    User.lastPasswordChange = currentDate.toISOString();
    User.darkMode = false;
    User.status = "Offline";
    User.deleted = false;
    User.department_Id = this.selectedDepartment.departmentID;
    User.department_Name = this.selectedDepartment.description;
    this.userService.editUser(User).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'done ', detail: 'User edited succesfully', life: 3000 });
        this.router.navigateByUrl('main/admin/users');
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
      }
    );

  }
}
