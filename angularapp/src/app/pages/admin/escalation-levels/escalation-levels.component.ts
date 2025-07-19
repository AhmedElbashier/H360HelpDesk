import { Component, OnInit, ViewChild } from '@angular/core';
import { EscalationLevel, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';


@Component({
  selector: 'app-escalation-levels',
  templateUrl: './escalation-levels.component.html',
  styleUrls: ['./escalation-levels.component.css']
})
export class EscalationLevelsComponent implements OnInit {
  @ViewChild('dt') dt!: Table;

  levels: EscalationLevel[] = [];
  level: EscalationLevel = {
    levelID: 0,
    levelName: ''
  };
  selectedLevels: EscalationLevel[] = [];
  levelDialog = false;
  submitted = false;

  constructor(
    private settingsService: SettingsService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.settingsService.getEscalationLevels().subscribe(res => this.levels = res);
  }

  applyFilterGlobal(event: any) {
    const searchTerm = (event.target as HTMLInputElement).value;
    this.dt.filterGlobal(searchTerm, 'contains');
  }

    openNew() {
      this.level = {
        levelName: '',
        profiles: [] // ✅ prevent 400 Bad Request
      };
      this.levelDialog = true;
    }

  editLevel(level: EscalationLevel) {
    this.level = { ...level };
    this.levelDialog = true;
  }

  saveLevel() {
    if (!this.level.profiles) {
      this.level.profiles = [];
    }

    const saveAction = this.level.levelID
      ? this.settingsService.editEscalationLevel(this.level)
      : this.settingsService.addEscalationLevel(this.level);

    saveAction.then(() => {
      this.messageService.add({ severity: 'success', summary: 'Done', detail: 'Level saved successfully', life: 3000 });
      this.levelDialog = false;
      this.loadData();
    }).catch(() => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to save level', life: 3000 });
    });
  }

  confirmDeleteLevel(level: EscalationLevel) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete Level ID ${level.levelID}?`,
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteEscalationLevel(level.levelID!)
          this.messageService.add({ severity: 'success', summary: 'Deleted', detail: 'Level deleted', life: 3000 });
          this.loadData();
      }
    });
  }

  deleteSelectedLevels() {
    const deletes = this.selectedLevels.map(level =>
      this.settingsService.deleteEscalationLevel(level.levelID!)
    );

    Promise.all(deletes).then(() => {
      this.messageService.add({ severity: 'success', summary: 'Deleted', detail: 'Selected levels deleted' });
      this.selectedLevels = [];
      this.loadData();
    });
  }

  hideDialog() {
    this.levelDialog = false;
  }

  exportExcel() {
    const worksheet = XLSX.utils.json_to_sheet(this.levels);
    const workbook = { Sheets: { 'Levels': worksheet }, SheetNames: ['Levels'] };
    const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(blob, 'EscalationLevels.xlsx');
  }


}
