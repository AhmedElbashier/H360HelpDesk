import { Component } from '@angular/core';
import { User, UserService } from '../../../services/user.service';
import { LoggerService } from '../../../services/logger.service';
import { Company, Department, SettingsService } from '../../../services/settings.service';
import { Location } from '@angular/common';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-add',
  templateUrl: './user-add.component.html',
  styleUrls: ['./user-add.component.css']
})
export class UserAddComponent {

  roles: any;
  selectedRole: any='';
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
  isDepartmentRestrictedSupervisor: boolean = false;

  constructor(private router: Router, private location: Location, private settingService: SettingsService, private userService: UserService, private logger: LoggerService, private messageService: MessageService) { }

  ngOnInit() {
    this.roles =
      [
        { id: 1, name: "Administrator" },
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
  }
  onTryAgainClick() {
    this.location.back();
  }
  submit(User: User) {
    const currentDate = new Date();

    // Assign roles
    if (this.selectedRole.name === "Administrator") {
      this.User.isAdministrator = true;
      this.User.isSuperVisor = false;
      this.User.isAgent = false;
      this.User.isBackOffice = false;
    } else if (this.selectedRole.name === "Supervisor") {
      this.User.isAdministrator = false;
      this.User.isSuperVisor = true;
      this.User.isAgent = false;
      this.User.isBackOffice = false;
    } else if (this.selectedRole.name === "BackOffice") {
      this.User.isAdministrator = false;
      this.User.isSuperVisor = false;
      this.User.isAgent = false;
      this.User.isBackOffice = true;
    } else if (this.selectedRole.name === "Agent") {
      this.User.isAdministrator = false;
      this.User.isSuperVisor = false;
      this.User.isAgent = true;
      this.User.isBackOffice = false;
    }

    // Add default values
    User.lastSeen = currentDate.toISOString();
    User.ipAddress = "Not defined";
    User.hostName = "Not defined";
    User.lastPasswordChange = currentDate.toISOString();
    User.darkMode = false;
    User.status = "Offline";
    User.deleted = false;
    User.isDepartmentRestrictedSupervisor = this.isDepartmentRestrictedSupervisor;

    // Conditionally assign department
    // ✅ Correct logic to assign department only if user is limited to one department
    if (this.isDepartmentRestrictedSupervisor && this.selectedDepartment?.departmentID) {
      User.department_Id = this.selectedDepartment.departmentID.toString();
      User.department_Name = this.selectedDepartment.description;
    } else {
      User.department_Id = null;
      User.department_Name = null;
    }

    // Optional validation
    if (this.User.isBackOffice && !this.selectedDepartment?.departmentID) {
      this.messageService.add({ severity: 'warn', summary: 'Field Required', detail: 'Please select a department', life: 3000 });
      return;
    }

    this.userService.addUser(User).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'User added successfully', life: 3000 });
        this.router.navigateByUrl('main/admin/users');
      },
      (error) => {
        if (error.status === 400) {
          this.messageService.add({ severity: 'warn', summary: 'Limit Reached', detail: 'The license users limit exceeded', life: 3000 });
        } else {
          this.logger.logError(error);
        }
      }
    );
  }
}
