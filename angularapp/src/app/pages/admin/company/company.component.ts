import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Company, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { LoggerService } from '../../../services/logger.service';
import { Table } from 'primeng/table';


@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.css']
})
export class CompanyComponent {
  CompaniesDialog!: boolean;
  CompaniesEditDialog!: boolean;
  Companies!: Company[];
  Company: Company = {};
  selectedCompanies!: Company[];
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  exportColumns!: any[];
  @ViewChild('dt') dt!: Table;
  constructor(private logger: LoggerService, private userService: UserService, private cdr: ChangeDetectorRef, private settingsService: SettingsService, private messageService: MessageService, private confirmationService: ConfirmationService, private router: Router) { }
  applyFilterGlobal(event: any) {
    const searchTerm = (event.target as HTMLInputElement).value;
    if (this.dt) {
      this.dt.filterGlobal(searchTerm, 'contains');
    }
  }
  ngOnInit() {
    this.Delete = "Delete";
    this.getData();
  }
  getData() {
    this.settingsService.getCompanies().subscribe(
      (res: any) => {
        this.Companies = res;
        this.cdr.detectChanges();
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
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
    this.Company = {};
    this.submitted = false;
    this.CompaniesDialog = true;
  }
  editCompany(Companies: Company) {
    this.Company = { ...Companies };
    this.CompaniesEditDialog = true;
  }
  deleteCompany(Companies: Company) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Company  ' + Companies.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteCompany(Companies.companyID).then(
          (res) => {
            this.messageService.add({ severity: 'success', summary: 'done ', detail: 'Company Deleted', life: 3000 });
            this.Companies = this.Companies.filter(val => val.companyID !== Companies.companyID);
            this.getData();
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
    this.CompaniesDialog = false;
    this.CompaniesEditDialog = false;
    this.submitted = false;
  }
  editCompaniesD(Companies: Company) {
    this.settingsService.editCompany(Companies).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Company name edited succesfully', life: 3000 });
        this.getData();
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Companies = [...this.Companies];
    this.CompaniesEditDialog = false;
    this.getData();
  }
  saveCompanies(Companies: Company) {
    this.submitted = true;
    this.settingsService.addCompany(Companies).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Company added successfully', life: 3000 });
        this.getData();
      },
      (error) => {
        
        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Companies = [...this.Companies];
    this.CompaniesDialog = false;
    this.getData();
  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Companies.length; i++) {
      if (this.Companies[i].companyID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Companies);
    const workbook = {
      Sheets: { 'Companies List': worksheet },
      SheetNames: ['Companies List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Companies List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }

  deleteSelectedCompanies() {
    if (this.selectedCompanies && this.selectedCompanies.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedCompanies.map(status => String(status.companyID)); // Convert to strings
          this.settingsService.deleteSelectedCompanies(selectedIds).then(
            () => {
              this.selectedCompanies = [];
              this.getData();
            },
            (error) => {
              console.error('Error deleting selected Severities:', error);
            }
          );
        }
      });
    }
  }
}
