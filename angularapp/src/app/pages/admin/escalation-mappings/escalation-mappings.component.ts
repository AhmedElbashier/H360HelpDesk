import { Component, OnInit, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';
import { ConfirmationService, MessageService } from 'primeng/api';
import {
  SettingsService,
  EscalationMapping,
  Department,
  Category,
  SubCategory,
  Priority,
  EscalationLevel
} from '../../../services/settings.service';

@Component({
  selector: 'app-escalation-mappings',
  templateUrl: './escalation-mappings.component.html',
  styleUrls: ['./escalation-mappings.component.css']
})
export class EscalationMappingsComponent implements OnInit {
  mappings: EscalationMapping[] = [];
  selectedMappings: EscalationMapping[] = [];
  mappingDialog = false;
  selectedMapping: EscalationMapping & {
    level1DelayDay?: number;
    level1DelayHour?: number;
    level1DelayMinute?: number;
    level2DelayDay?: number;
    level2DelayHour?: number;
    level2DelayMinute?: number;
    level3DelayDay?: number;
    level3DelayHour?: number;
    level3DelayMinute?: number;
  } = this.createEmptyMapping();

  departments: Department[] = [];
  categories: Category[] = [];
  subcategories: SubCategory[] = [];
  priorities: Priority[] = [];
  levels: EscalationLevel[] = [];
  filteredSubcategories: any[] = [];

  @ViewChild('dt') dt!: Table;

  constructor(
    private mappingService: SettingsService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit(): void {
    Promise.all([
      this.mappingService.getDepartments().toPromise().then(res => this.departments = res || []),
      this.mappingService.getCategories().toPromise().then(res => this.categories = res || []),
      this.mappingService.getPriorites().toPromise().then(res => this.priorities = res || []),
      this.mappingService.getEscalationLevels().toPromise().then(res => this.levels = res || []),
    ]).then(() => {
      this.loadMappings(); // only load after metadata is ready
    });
  }

  loadMappings() {
    this.mappingService.getEscalationMappings().subscribe({
      next: res => (this.mappings = res),
      error: () => this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Failed to load escalation mappings'
      })
    });
  }

  loadLevels() {
    this.mappingService.getEscalationLevels().subscribe(levels => {
      this.levels = levels;
    });
  }

  createEmptyMapping(): EscalationMapping {
    return {
      departmentID: 0,
      categoryID: 0,
      subcategoryID: 0,
      priorityID: 0,
      level1ProfileID: 0
    };
  }

  openNew() {
    this.selectedMapping = this.createEmptyMapping();
    this.mappingDialog = true;
  }

  onCategoryChange() {
    if (this.selectedMapping.categoryID) {
      this.mappingService.getSubCategoriesbyMainCategory(this.selectedMapping.categoryID).then(res => {
        this.subcategories = Array.isArray(res) ? res : [res];
      });
    } else {
      this.subcategories = [];
    }
  }

  sanitizeDelay(value: string | null | undefined): string | undefined {
    if (!value || value === '--:--' || value === '') return undefined;
    return value.length === 5 ? value + ':00' : value; // ensures hh:mm:ss
  }
  formatDelay(day?: number, hour?: number, minute?: number): string | undefined {
    if (day == null && hour == null && minute == null) return undefined;
    return `${String(day || 0).padStart(2, '0')}:${String(hour || 0).padStart(2, '0')}:${String(minute || 0).padStart(2, '0')}`;
  }


  parseDelay(value?: string): { day: number, hour: number, minute: number } {
    if (!value || !value.includes(':')) return { day: 0, hour: 0, minute: 0 };
    const [d, h, m] = value.split(':').map(Number);
    return { day: d || 0, hour: h || 0, minute: m || 0 };
  }
  saveMapping() {
    // Convert level selections before saving
    this.selectedMapping.level1ProfileID = this.selectedMapping.level1ProfileID;
    this.selectedMapping.level2ProfileID = this.selectedMapping.level2ProfileID;
    this.selectedMapping.level3ProfileID = this.selectedMapping.level3ProfileID;

    // Format delay values into HH:MM:SS
    this.selectedMapping.level1Delay = this.formatDelay(
      this.selectedMapping.level1DelayDay,
      this.selectedMapping.level1DelayHour,
      this.selectedMapping.level1DelayMinute
    );

    this.selectedMapping.level2Delay = this.formatDelay(
      this.selectedMapping.level2DelayDay,
      this.selectedMapping.level2DelayHour,
      this.selectedMapping.level2DelayMinute
    );

    this.selectedMapping.level3Delay = this.formatDelay(
      this.selectedMapping.level3DelayDay,
      this.selectedMapping.level3DelayHour,
      this.selectedMapping.level3DelayMinute
    );

    const isEdit = !!this.selectedMapping.mappingID && this.selectedMapping.mappingID > 0;
    const action = isEdit
      ? this.mappingService.editEscalationMapping(this.selectedMapping)
      : this.mappingService.addEscalationMapping(this.selectedMapping);

    action.then(() => {
      const msg = isEdit ? 'Updated' : 'Created';
      this.messageService.add({
        severity: 'success',
        summary: msg,
        detail: `Mapping ${msg.toLowerCase()} successfully`
      });
      this.loadMappings();
      this.mappingDialog = false;
    }).catch(() => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Failed to save mapping',
      });
    });
  }

  editMapping(mapping: EscalationMapping) {
    this.selectedMapping = { ...mapping };

    this.selectedMapping.level1ProfileID = mapping.level1ProfileID;
    this.selectedMapping.level2ProfileID = mapping.level2ProfileID;
    this.selectedMapping.level3ProfileID = mapping.level3ProfileID;

    // Delay parsing logic
    const delay1 = this.parseDelay(mapping.level1Delay);
    this.selectedMapping.level1DelayDay = delay1.day;
    this.selectedMapping.level1DelayHour = delay1.hour;
    this.selectedMapping.level1DelayMinute = delay1.minute;

    const delay2 = this.parseDelay(mapping.level2Delay);
    this.selectedMapping.level2DelayDay = delay2.day;
    this.selectedMapping.level2DelayHour = delay2.hour;
    this.selectedMapping.level2DelayMinute = delay2.minute;

    const delay3 = this.parseDelay(mapping.level3Delay);
    this.selectedMapping.level3DelayDay = delay3.day;
    this.selectedMapping.level3DelayHour = delay3.hour;
    this.selectedMapping.level3DelayMinute = delay3.minute;

    this.onCategoryChange();
    this.mappingDialog = true;
  }



  deleteMapping(mapping: EscalationMapping) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this mapping?',
      accept: () => {
        this.mappingService.deleteEscalationMapping(mapping.mappingID!).then(() => {
          this.messageService.add({ severity: 'success', summary: 'Deleted', detail: 'Mapping removed' });
          this.loadMappings();
        });
      }
    });
  }

  deleteSelectedMappings() {
    this.selectedMappings.forEach(m => {
      if (m.mappingID) this.mappingService.deleteEscalationMapping(m.mappingID).then(() => this.loadMappings());
    });
    this.selectedMappings = [];
  }

  getDepartmentName(id: number | string): string {
    const dep = this.departments.find(d => Number(d.departmentID) === Number(id));
    return dep?.description || 'N/A';
  }

  getCategoryName(id: number): string {
    return this.categories.find(c => Number(c.categoryID) === Number(id))?.description || 'N/A';
  }

  getSubcategoryName(id: number): string {
    return this.subcategories.find(sc => Number(sc.subCategoryID) === Number(id))?.description || 'N/A';
  }

  getPriorityName(id: number): string {
    return this.priorities.find(p => Number(p.levelID) === Number(id))?.description || 'N/A';
  }

  getProfileName(id: number): string {
    return this.levels.find(p => p.levelID == id)?.levelName || 'N/A';
  }

  hideDialog() {
    this.mappingDialog = false;
  }

  applyGlobalFilter(event: any) {
    this.dt.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
