import { Component, OnInit, ViewChild } from '@angular/core';
import { EscalationProfile, EscalationLevel, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
@Component({
  selector: 'app-escalation-profiles',
  templateUrl: './escalation-profiles.component.html',
  styleUrls: ['./escalation-profiles.component.css']
})
export class EscalationProfilesComponent implements OnInit {
  @ViewChild('dt') dt!: Table;

  profiles: EscalationProfile[] = [];
  selectedProfiles: EscalationProfile[] = [];
  profileDialog = false;
  submitted = false;

  levels: EscalationLevel[] = [];
  profile: EscalationProfile = {
    name: '',
    email: '',
    escalationLevelID: 0
  };

  selectedLevel: EscalationLevel = {
    levelID: 0,
    levelName: ''
  };

  constructor(
    private settingsService: SettingsService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.settingsService.getEscalationProfiles().subscribe(res => this.profiles = res);
    this.settingsService.getEscalationLevels().subscribe(res => this.levels = res);
  }

  applyFilterGlobal(event: any) {
    const searchTerm = (event.target as HTMLInputElement).value;
    this.dt.filterGlobal(searchTerm, 'contains');
  }

  openNew() {
    this.profile = {
      name: '',
      email: '',
      escalationLevelID: 0
    };

    this.selectedLevel = {
      levelID: 0,
      levelName: ''
    };

    this.profileDialog = true;
  }

  editProfile(profile: EscalationProfile) {
    this.profile = { ...profile };
    this.selectedLevel = this.levels.find(lvl => lvl.levelID === profile.escalationLevelID) || {
      levelID: 0,
      levelName: ''
    };
    this.profileDialog = true;
  }


  saveProfile() {
    if (!this.profile.name || !this.selectedLevel?.levelID || !this.profile.email) return;

    this.profile.escalationLevelID = this.selectedLevel.levelID;

    const saveAction = this.profile.profileID
      ? this.settingsService.editEscalationProfile(this.profile)
      : this.settingsService.addEscalationProfile(this.profile);

    saveAction.then(() => {
      this.messageService.add({ severity: 'success', summary: 'Done', detail: 'Profile saved successfully', life: 3000 });
      this.profileDialog = false;
      this.loadData();
    }).catch(() => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to save profile', life: 3000 });
    });
  }
  getLevelName(level: number): string {
    return this.levels.find(lvl => lvl.levelID === level)?.levelName || 'N/A';
  }

  confirmDeleteProfile(profile: EscalationProfile) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete Profile ID ${profile.profileID}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteEscalationProfile(profile.profileID!).then(() => {
          this.messageService.add({ severity: 'success', summary: 'Deleted', detail: 'Profile deleted', life: 3000 });
          this.loadData();
        });
      }
    });
  }

  deleteSelectedProfiles() {
    if (!this.selectedProfiles.length) return;
    this.selectedProfiles.forEach(profile => {
      this.settingsService.deleteEscalationProfile(profile.profileID!).then(() => this.loadData());
    });
    this.selectedProfiles = [];
  }

  hideDialog() {
    this.profileDialog = false;
  }

  exportExcel() {
    const worksheet = XLSX.utils.json_to_sheet(this.profiles);
    const workbook = { Sheets: { 'Profiles': worksheet }, SheetNames: ['Profiles'] };
    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blob, 'EscalationProfiles.xlsx');
  }
  getLevelNameById(id: number): string {
    const level = this.levels.find(l => l.levelID === id);
    return level ? level.levelName : 'N/A';
  }
}
