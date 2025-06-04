import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Channel, SettingsService } from '../../../services/settings.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import { saveAs } from 'file-saver';
import * as XLSX from 'xlsx';
import { LoggerService } from '../../../services/logger.service';
import { Table } from 'primeng/table';


@Component({
  selector: 'app-channel',
  templateUrl: './channel.component.html',
  styleUrls: ['./channel.component.css']
})
export class ChannelComponent {
  ChannelsDialog!: boolean;
  ChannelsEditDialog!: boolean;
  Channels!: Channel[];
  Channel: Channel = {};
  selectedChannels!: Channel[];
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
    this.settingsService.getChannels().subscribe(
      (res: any) => {
        this.Channels = res;
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
    this.Channel = {};
    this.submitted = false;
    this.ChannelsDialog = true;
  }
  editChannel(Channels: Channel) {
    this.Channel = { ...Channels };
    this.ChannelsEditDialog = true;
  }
  deleteChannel(Channels: Channel) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Channel  ' + Channels.description + '؟',
      header: 'Confirm  ',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.settingsService.deleteChannel(Channels.channelID).then(
          (res) => {
            this.messageService.add({ severity: 'success', summary: 'done ', detail: 'Channel Deleted', life: 3000 });
            this.Channels = this.Channels.filter(val => val.channelID !== Channels.channelID);
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
    this.ChannelsDialog = false;
    this.ChannelsEditDialog = false;
    this.submitted = false;
  }
  editChannelsD(Channels: Channel) {
    this.settingsService.editChannel(Channels).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Channel name edited succesfully', life: 3000 });
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
    this.Channels = [...this.Channels];
    this.ChannelsEditDialog = false;
    this.getData();
  }
  saveChannels(Channels: Channel) {
    this.submitted = true;
    this.settingsService.addChannel(Channels).then(
      (res) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'New Channel added successfully', life: 3000 });
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
    this.Channels = [...this.Channels];
    this.ChannelsDialog = false;
    this.getData();
  }
  findIndexById(id: any): number {
    let index = -1;
    for (let i = 0; i < this.Channels.length; i++) {
      if (this.Channels[i].channelID == id) {
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
    const worksheet = XLSX.utils.json_to_sheet(this.Channels);
    const workbook = {
      Sheets: { 'Channels List': worksheet },
      SheetNames: ['Channels List'],
    };

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array',
    });

    this.saveAsExcelFile(excelBuffer, 'Channels List.xlsx');
  }

  saveAsExcelFile(excelBuffer: any, filename: string) {
    const data = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    FileSaver.saveAs(data, filename);
  }

  deleteSelectedChannels() {
    if (this.selectedChannels && this.selectedChannels.length > 0) {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected items?',
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          const selectedIds = this.selectedChannels.map(channel => String(channel.channelID)); // Convert to strings
          this.settingsService.deleteSelectedChannels(selectedIds).then(
            () => {
              this.selectedChannels = [];
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
