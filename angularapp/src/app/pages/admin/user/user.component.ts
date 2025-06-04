import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { User, UserService } from '../../../services/user.service';
import { Router } from '@angular/router';
import { CommonService } from '../../../services/common.service';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';
import { LoggerService } from '../../../services/logger.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styles: [
    `
      :host ::ng-deep .p-dialog .user-image {
        width: 150px;
        margin: 0 auto 2rem auto;
        display: block;
      }
    `,
  ],
  styleUrls: ['./user.component.css'],
})
export class UserComponent {
  UsersDialog!: boolean;
  UsersEditDialog!: boolean;
  Users!: User[];
  User: User = {};
  selectedUsers!: User[];
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  countries!: string[];
  regions!: string[];
  exportColumns!: any[];
  UserType!: any;
  password: any = '';
  c_password: any = '';
  resetDialog: boolean = false;
  @ViewChild('dt') dt!: Table;
  constructor(private logger: LoggerService, private common: CommonService, private cdr: ChangeDetectorRef, private userService: UserService, private messageService: MessageService, private confirmationService: ConfirmationService, private router: Router) { }
  applyFilterGlobal(event: any) {
    const searchTerm = (event.target as HTMLInputElement).value;
    if (this.dt) {
      this.dt.filterGlobal(searchTerm, 'contains');
    }
  }
  ngOnInit() {
    this.Delete = "Delete";
    this.getData();
    this.countries = this.common.countries;
    this.regions = this.common.regions;
  }
  getData() {
    this.userService.getUsers().subscribe(
      (res: any) => {
        this.Users = res;
        this.cdr.detectChanges();
      },
      (error) => {
        
        this.logger.logError(error);
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
  openNew() {
    this.router.navigateByUrl('main/admin/user-add');
  }
  editUser(Users: User) {
    this.User = { ...Users };
    localStorage.setItem("adminUserEdit", JSON.stringify(this.User));
    this.router.navigateByUrl('main/admin/user-edit');

  }
  detailsUser(Users: User) {
    this.User = { ...Users };
    localStorage.setItem("adminUserEdit", JSON.stringify(this.User));
    this.router.navigateByUrl('main/admin/user-details');

  }
  deleteUser(Users: User) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the User  ' + Users.firstname + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.userService.deleteUser(Users.id).then(
          (res) => {
            this.messageService.add({ severity: 'success', summary: 'done ', detail: 'User Deleted', life: 3000 });
            this.Users = this.Users.filter(val => val.id !== Users.id);
          },
          (error) => {
            
            this.logger.logError(error);
            this.router.navigateByUrl("error");
            this.messageService.add({
              severity: 'error',
              summary: 'failed',
              detail: 'System Error! ',
              life: 3000,
            });
          }
        );
        this.getData();
      }
    });
  }

  hideDialog() {
    this.UsersDialog = false;
    this.UsersEditDialog = false;
    this.submitted = false;
    this.resetDialog = false;

  }
  findIndexById(id: string): number {
    let index = -1;
    for (let i = 0; i < this.Users.length; i++) {
      if (this.Users[i].id === id) {
        index = i;
        break;
      }
    }
    return index;
  }
  createId(): string {
    let id = '';
    var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    for (var i = 0; i < 5; i++) {
      id += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return id;
  }
  exportExcel() {
    const worksheet = XLSX.utils.json_to_sheet(this.Users);
    const workbook = {
      Sheets: { 'Users List': worksheet },
      SheetNames: ['Users List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Users List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
  deleteSelectedUsers() {
    if (this.selectedUsers && this.selectedUsers.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedUsers.map(status => status.id);
          this.userService.deleteSelectedUsers(selectedIds).then(
            () => {
              this.selectedUsers = [];
              this.getData();
            },
            (error) => {
              
              this.logger.logError(error);
              this.router.navigateByUrl("error");
            }
          );
        }
      });
    }
  }
  resetPasswordDialog(User: User) {
    this.User = {...User };
    this.resetDialog = true;
  }
  reset(User: User) {
    if (this.password !== this.c_password) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Password do not match', life: 3000 });

    }
    else {
      this.userService.resetUser(User.id, this.password).then(
        (res: any) => {
          this.messageService.add({ severity: 'success', summary: 'Done', detail: 'Password resetted succefully', life: 3000 });
          this.resetDialog = false;
        },
        (error: any) => {
          this.messageService.add({ severity: 'error', summary: 'failed', detail: 'an error occured while connection to database', life: 3000 });
        }
      );
    }
  }
}
