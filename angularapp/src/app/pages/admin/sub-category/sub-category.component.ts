import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { SubCategory, Category, SettingsService, Department } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-sub-category',
  templateUrl: './sub-category.component.html',
  styleUrls: ['./sub-category.component.css']
})
export class SubCategoryComponent {

  subCategoriesDialog!: boolean;
  EditDialog!: boolean;
  subCategories!: SubCategory[];
  SubCategory: SubCategory = {};
  Departments!: Department[];
  Department: Department = {};
  selectedDepartment: Department = {};
  Categories!: Category[];
  Category: Category = {};
  selectedCategories!: SubCategory[];
  selectedCategory: Category = {};
  selectedSubCategories!: Category [];
  submitted!: boolean;
  Delete!: string;
  cols!: any[];
  exportColumns!: any[];
  @ViewChild('dt') dt!: Table;
  newSubCategory: any;
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
    this.settingsService.getDepartments().subscribe(
      (res) => {
        this.Departments = res;
        this.cdr.detectChanges();
      },
      (error) => {
        
      }
    )
    
  }
  onDepartmentChange() {
    this.settingsService.getCategoriesbyDepartment(this.selectedDepartment.departmentID).then(
      (res: any) => {
        this.Categories = res;
        this.cdr.detectChanges();
      },
      (error) => {
        
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
  addNew() {

    const currentDate = new Date();
    const currentFormattedDate = currentDate.toISOString();
    this.SubCategory.categoryID = this.selectedCategory.categoryID?.toString();
    this.SubCategory.description = this.newSubCategory;
    this.settingsService.addSubCategory(this.SubCategory).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Category added successfully', life: 3000 });
        this.getSelectedCategory();
        this.SubCategory = {};

      },
      (error) => {
        
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
  getSelectedCategory() {
    this.settingsService.getSubCategoriesbyMainCategory(this.selectedCategory.categoryID).then(
      (res: any) => {
        this.subCategories = res;
        this.cdr.detectChanges();
      },
      (error) => {
        
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
  editCategory(SubCategory: SubCategory) {
    this.SubCategory = { ...SubCategory };
    this.EditDialog = true;
  }
  deleteCategory(SubCategory:SubCategory) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Sub-Category  ' + SubCategory.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteSubCategory(SubCategory.subCategoryID).then(
          (res) => {
            this.messageService.add({ severity: 'error', summary: 'done ', detail: 'Sub-Category Deleted', life: 3000 });
            this.subCategories = this.subCategories.filter(val => val.subCategoryID !== SubCategory.subCategoryID);
            this.getSelectedCategory();
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
        this.getSelectedCategory();
      }
    });
    this.hideDialog();
  }
  editSubCategoriesD(SubCategory:SubCategory) {
    SubCategory.categoryID = this.selectedCategory.categoryID?.toString();
    this.settingsService.editSubCategory(SubCategory).then(
      (res) => {
        this.messageService.add({ severity: 'warn', summary: 'Done ', detail: 'Category name edited succesfully', life: 3000 });
        this.getSelectedCategory();
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
    this.EditDialog = false;
    this.getSelectedCategory();
    this.hideDialog();
  }
  hideDialog() {
    this.SubCategory = {};
    this.EditDialog = false;
  }
  exportExcel() {
    const worksheet = XLSX.utils.json_to_sheet(this.Categories);
    const workbook = {
      Sheets: { 'Categories List': worksheet },
      SheetNames: ['Sub-Categories List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Sub-Categories List.xlsx');
  }
  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }
}
