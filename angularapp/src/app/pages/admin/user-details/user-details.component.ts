import { Component } from '@angular/core';
import { User } from '../../../services/user.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent {

  disabled: boolean = true;
  User: User = {};
  UserType: any;
  constructor(private location: Location) { }

  ngOnInit() {
    this.User = JSON.parse(localStorage.getItem("adminUserEdit") || "{}") as User;

    if (this.User.isAdministrator) {
      this.UserType = "Administrator";
    } else if (this.User.isSuperVisor) {
      this.UserType = this.User.isDepartmentRestrictedSupervisor ? "Supervisor (Single Department)" : "Supervisor (All Departments)";

    } else if (this.User.isBackOffice) {
      this.UserType = "BackOffice";
    } else if (this.User.isAgent) {
      this.UserType = "Agent";
    }
    if (this.User.department_Name === "0") {
      this.User.department_Name = '';
    }
  }

  getDepartmentDisplay(): string {
    if (this.User.department_Name && this.User.department_Name !== "0") {
      return this.User.department_Name;
    }

    if (this.UserType === "Supervisor (All Departments)") {
      return "All Departments";
    }

    // BackOffice, Agent, Supervisor (Single Department)
    return "N/A";
  }

  showDepartment(): boolean {
    return this.UserType !== "Agent" &&
      !(this.UserType === "Supervisor" && this.User.isDepartmentRestrictedSupervisor);
  }

  onTryAgainClick() {
    this.location.back();
  }
}
