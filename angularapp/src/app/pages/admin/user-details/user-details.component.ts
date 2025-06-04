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
    }
    else if (this.User.isSuperVisor) {
      this.UserType = "Supervisor";
    }
    else if (this.User.isBackOffice) {
      this.UserType = "BackOffice";
    }
    else if (this.User.isAgent) {
      this.UserType = "Agent";
    }
  }

  onTryAgainClick() {
    this.location.back();
  }
}
