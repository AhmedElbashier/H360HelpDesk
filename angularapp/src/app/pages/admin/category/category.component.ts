import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Category, Department, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { Table } from 'primeng/table';
@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent {


  CategoriesDialog!: boolean;
  CategoriesEditDialog!: boolean;
  Categories!: Category[];
  Category: Category = {};
  Departments!: Department[];
  Department: Department = {};
  selectedCategories!: Category[];
  selectedDepartment: Department = {};
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  exportColumns!: any[];
  departmentOptions: { label: string, value: string | null }[] = [];
  selectedDepartmentId: string | null = null;
  allCategories: Category[] = []; // to keep unfiltered list

  @ViewChild('dt') dt!: Table;
  constructor(private userService: UserService, private cdr: ChangeDetectorRef, private settingsService: SettingsService, private messageService: MessageService, private confirmationService: ConfirmationService, private router: Router) { }
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
    this.settingsService.getCategories().subscribe(
      (res: any) => {
        this.allCategories = res;
        this.Categories = [...this.allCategories];
        this.cdr.detectChanges();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'An error occurred while connection to the database',
          life: 3000,
        });
      }
    );

    this.settingsService.getDepartments().subscribe(
      (res: any) => {
        this.Departments = res;
        this.departmentOptions = [
          { label: 'All Departments', value: null },
          ...res.map((d: Department) => ({
            label: d.description,
            value: d.departmentID,
          }))
        ];
        this.cdr.detectChanges();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'An error occurred while connection to the database',
          life: 3000,
        });
      }
    );
  }
  filterByDepartment() {
    if (this.selectedDepartmentId) {
      this.Categories = this.allCategories.filter(
        (cat) => cat.departmentID === this.selectedDepartmentId
      );
    } else {
      this.Categories = [...this.allCategories]; // Show all
    }
  }


  getCategoryDepartmentName(departmentID: any): any {
    const department = this.Departments?.find((d) => d.departmentID === departmentID);
    return department ? department.description : 'N/A';
  }


  openNew() {
    this.submitted = false;
    this.CategoriesDialog = true;
  }
  editCategory(Categories: Category) {
    this.Category = { ...Categories };
    this.CategoriesEditDialog = true;
  }
  deleteCategory(Categories: Category) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Category  ' + Categories.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteCategory(Categories.categoryID).then(
          (res) => {
            this.messageService.add({ severity: 'error', summary: 'done ', detail: 'Category Deleted', life: 3000 });
            this.Categories = this.Categories.filter(val => val.categoryID !== Categories.categoryID);
            this.getData();
          },
          (error) => {
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
    this.hideDialog();

  }

  hideDialog() {
    this.CategoriesDialog = false;
    this.CategoriesEditDialog = false;
    this.submitted = false;
    this.Category = {};
  }
  editCategoriesD(Categories: Category) {
    const currentDate = new Date();
    Categories.departmentID = this.Department.departmentID;
    this.settingsService.editCategory(Categories).then(
      (res) => {
        this.messageService.add({ severity: 'warn', summary: 'Done ', detail: 'Category name edited succesfully', life: 3000 });
        this.getData();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Categories = [...this.Categories];
    this.CategoriesEditDialog = false;
    this.getData();
    this.hideDialog();

  }
  saveCategories(Categories: Category, Department: Department) {
    this.submitted = true;
    Categories.departmentID = this.Department.departmentID?.toString();
    this.settingsService.addCategory(Categories).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Category added successfully', life: 3000 });
        this.getData();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Failed',
          detail: 'System Error.!',
          life: 3000,
        });
      }
    );
    this.Categories = [...this.Categories];
    this.CategoriesDialog = false;
    this.getData();
    this.hideDialog();

  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Categories.length; i++) {
      if (this.Categories[i].categoryID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Categories);
    const workbook = {
      Sheets: { 'Categories List': worksheet },
      SheetNames: ['Categories List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Categories List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
  deleteSelectedCategories() {
    if (this.selectedCategories && this.selectedCategories.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedCategories.map(status => String(status.categoryID)); // Convert to strings
          this.settingsService.deleteSelectedPriorites(selectedIds).then(
            () => {
              this.selectedCategories = [];
              this.getData();
            },
            (error) => {
              console.error('Error deleting selected items:', error);
            }
          );
        }
      });
    }
  }

}
