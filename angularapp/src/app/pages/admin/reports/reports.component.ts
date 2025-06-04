import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { Router } from '@angular/router';
import { User } from '../../../services/user.service';
import {
  Department,
  SettingsService,
  //Package
} from '../../../services/settings.service';
@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css'],
})
export class ReportsComponent implements OnInit {
  EmpMonthlyDialog!: boolean;
  EmpYearlyDialog!: boolean;
  DeptReportDialog!: boolean;
  DeptDetailedReportDialog!: boolean;
  contractDialog!: boolean;
  ownerDialog!: boolean;
  tenantDialog!: boolean;
  usersDialog!: boolean;
  electricWaterDialog!: boolean;
  trainingDialog!: boolean;
  packageDialog!: boolean;

  caseActive!: string;

  owners!: User[];
  owner!: User;
  buildings!: User[];
  building!: User;
  contractStatuses!: User[];
  contractStatus!: User;

  // USERS TEST

  users!: User[];
  user!: User;

  Emp!: any;
  Emps!: any[];
  Dept!: Department;
  Depts!: Department[];
  month!: any;
  year!: any;

  content: any;
  constructor(
    private settingService: SettingsService,
    private router: Router
  ) { }

  columns!: any[];
  sales!: any[];

  ngOnInit(): void { }

  EmpGet() {
    //this.settingService.getTaxOnVariableAllowances().subscribe((res: any) => {
    //  this.buildings = res;
    //});
  }

  DeptGet() {
    //this.settingService.getTaxOnVariableAllowances().subscribe((res: any) => {
    //  this.buildings = res;
    //});
  }

  EmpMonthlyReport() {
    this.EmpMonthlyDialog = true;
  }
  EmpYearlyReport() {
    this.EmpYearlyDialog = true;
  }
  adminReport() {
    this.router.navigate(['adminReport']);
  }
  academicReport() {
    // this.buildingAmountDialog = true;
    this.router.navigate(['academicReport']);
  }
  payedReport() {
    this.router.navigate(['adminReport']);
  }
  unpayedReport() {
    this.router.navigate(['academicReport']);
  }
  DeptReport() {
    this.DeptReportDialog = true;
  }
  DeptDetailedReport() {
    this.DeptDetailedReportDialog = true;
  }

  settingsReport() {
    this.router.navigate(['reportowner']);
  }

  summaryReport() {
    this.tenantDialog = true;
  }
  trainingReport() { }
  packageReport() { }
  trainingPayRollReport() {
    this.trainingDialog = true;
  }
  packagePayRollReport() {
    this.packageDialog = true;
  }
  trainingPayRollReportD() {
    this.trainingDialog = false;
  }
  packagePayRollReportD() {
    this.packageDialog = false;
  }
  electricWaterReport() {
    this.electricWaterDialog = true;
  }

  hideDialog() {
    this.EmpMonthlyDialog = false;
    this.EmpYearlyDialog = false;
    this.DeptReportDialog = false;
    this.DeptDetailedReportDialog = false;
    this.contractDialog = false;
    this.tenantDialog = false;
  }

  savePayment() {
    //this.settingService.getTaxOnVariableAllowances().subscribe((res: any) => {
    //  this.users = res;
    //});
    this.usersDialog = true;
  }

  buildingExpenseReportD() {
    localStorage.setItem('expenseReportStatus', this.user.id);
    this.router.navigate(['reportexpense']);
  }
  buildingAmountReportD() {
    localStorage.setItem('amountReportStatus', this.user.id);
    this.router.navigate(['reportamount']);
  }
  caseReportD() {
    localStorage.setItem('caseReportStatus', this.caseActive);
    this.router.navigate(['reportdetails']);
  }
  contractReportD() {
    //localStorage.setItem('contractReportStatus', this.contractStatus.name);
    this.router.navigate(['reportcontract']);
  }

  tenantReportD() {
    localStorage.setItem('tenantReportStatus', this.user.id);
    this.router.navigate(['reporttenant']);
  }

  electricWaterReportD() {
    localStorage.setItem('electricWaterReportStatus', this.user.id);
    this.router.navigate(['reportelectricWater']);
  }

  @ViewChild('content') dt: ElementRef | undefined;
  public onPrint(): void {
    this.router.navigate(['reportdetails']);
  }
}
