
import { ChangeDetectorRef, Component, TemplateRef, ViewChild } from '@angular/core';
import Editor from 'ckeditor5-custom-build/build/ckeditor';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { Ticket, TicketService, Comment } from '../../../services/ticket.service';
import { Category, Channel, Department, Priority, SettingsService, Status, SubCategory, Request, FileAttachment } from '../../../services/settings.service';
import { Router } from '@angular/router';
import { LoggerService } from '../../../services/logger.service';
import { MessageService } from 'primeng/api';
import { User, UserService } from '../../../services/user.service';
import { Observable, map } from 'rxjs';
interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}

interface SelectedAgent {
  id?: string;
  lastname?: string;
  firstname?: string;
  username?: string;
}

@Component({
  selector: 'app-ticket-details',
  templateUrl: './ticket-details.component.html',
  styleUrls: ['./ticket-details.component.css']
})
export class TicketDetailsComponent {
  title = 'customEditor';
  public Editor = Editor;
  @ViewChild('commentTemplateRef') commentTemplateRef!: TemplateRef<any>;
  departments: Department[] = [];
  categories: Category[] = [];
  subCategories: SubCategory[] = [];
  listSubCategories: SubCategory[] = [];
  agentNameToAgentMap: { [name: string]: SelectedAgent } = {};
  assignAgentDialog: boolean = false;
  statuses: Status[] = [];
  priorities: Priority[] = [];
  requests: Request[] = [];
  channels: Channel[] = [];
  ticket: Ticket = {};
  comment: Comment = {};
  agents: any[] = [];
  selectedAgent: User = {}
  selectedAgentFullName: string = ''; // Initialize as an empty string
  filteredAgentNames!: any[];
  comments: Comment[] = [];
  users: User[] = [];
  files: FileAttachment[] = [];
  user: User = {};
  fileAttachment: FileAttachment = {};
  selectedState: any;
  dropdownItems: any;
  skeleton: boolean = true;
  reply: any = '';
  defaultFileList: NzUploadFile[] = [];
  fileList2 = [...this.defaultFileList];
  data = {};
  inputValue?: string;
  base64Content: string | null = null;
  editFlag: string = "";
  selectedPriority: Priority = {};
  selectedChannel: Channel = {};
  selectedDepartment: Department = {};
  selectedCategory: Category = {};
  selectedSubCategory: SubCategory = {};
  selectedReason: Request[] =[];
  constructor(private userService: UserService, private settingService: SettingsService, private logger: LoggerService, private router: Router, private cdr: ChangeDetectorRef, private ticketService: TicketService, private messageService: MessageService) { }
  async ngOnInit() {

    this.user = JSON.parse(localStorage.getItem("user") || "{}") as User;
    this.ticket = JSON.parse(localStorage.getItem("TicketDetails") || "{}") as Ticket;
    await this.getFiles();
    await this.getComments();
    await this.getUsers();
    await this.getDepartments();
    await this.getCategories();
    await this.getSubCategories();
    await this.getStatuses();
    await this.getPriorities();
    await this.getRequests();
    await this.getChannels();
    this.skeleton = false;

  }
  private stripPTags(html: string): string {
    return html.replace(/<\/?p>/g, '');
  }
  getSeverity(status: any) {
    if (status === 1) {
      return 'success';
    }
    else if (status === 2) {
      return 'warning';

    }
    else if (status === 3) {
      return 'danger';
    }
    else {
      return 'unknown';
    }
  }


  onCategoryChange() {
    this.settingService.getSubCategoriesbyMainCategory(this.selectedCategory.categoryID).then(
      (res: any) => {
        this.listSubCategories = res;
        this.cdr.detectChanges();
      },
      (error) => {
        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: "An error occurred while connecting to the database",
          life: 3000,
        });
      }
    )
  }
  getTicketAssignedUserDescription(assignedToUserId: any): any {
    const user = this.users.find((d) => d.user_Id == assignedToUserId);
    return user ? user.lastname + " " + user.firstname : 'N/A';
  }
  getTicketStatusDescription(statusID: any): any {
    const status = this.statuses.find((d) => d.statusID === statusID);
    return status ? status.description : 'N/A';
  }
  getTicketChannelDescription(channelID: any): any {
    const channel = this.channels.find((d) => d.channelID === channelID);
    return channel ? channel.description : 'N/A';
  }
  getTicketRequestDescription(requestID: any): any {
    const request = this.requests.find((d) => d.requestID === requestID);
    return request ? request.description : 'N/A';
  }
  getTicketPriorityDescription(priorityID: any): any {
    const priority = this.priorities.find((d) => d.levelID === priorityID);
    return priority ? priority.description : 'N/A';
  }
  getTicketDepartmentDescription(departmentID: any): any {
    const Department = this.departments.find((d) => d.departmentID === departmentID);
    return Department ? Department.description : 'N/A';
  }
  getTicketCategoryDescription(CategoryID: any): any {
    const Category = this.categories.find((d) => d.categoryID === CategoryID);
    return Category ? Category.description : 'N/A';
  }
  getTicketSubCategoryDescription(subCategoryID: any): any {
    const Subcategory = this.subCategories.find((d) => d.subCategoryID === subCategoryID);
    return Subcategory ? Subcategory.description : 'N/A';
  }
  getReasonDescription(requestID: any): any {
    const Reason = this.requests.find((d) => d.requestID === requestID);
    return Reason ? Reason.description : 'N/A';
  }
  getUsers() {
    this.userService.getUsers().subscribe(
      (res: any) => {
        this.users = res;
        this.cdr.detectChanges();
      },
      (error) => {
        this.logger.logError(error);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: "An error occurred while connecting to the database",
          life: 3000,
        });
      }
    );
  }
  getFiles() {
    this.settingService.getTicketFiles(this.ticket.id).then(
      (res) => {
        this.files = res;
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
    )
  }

  getComments() {
    //this.ticketService.getRelatedComments(this.ticket.id).subscribe((apiComments: Comment[]) => {

    //  //this.comments = this.createCommentHierarchy(apiComments);
    //}
    this.ticketService.getRelatedComments(this.ticket.id).subscribe(
      (res: any) => {
        this.comments = res;
      }
    );
  }
  getDepartments() {
    this.settingService.getDepartments().subscribe(
      (res: any) => {
        this.departments = res;
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
  getCategories() {
    this.settingService.getCategories().subscribe(
      (res: any) => {
        this.categories = res;
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
  getSubCategories() {
    this.settingService.getSubCategories().subscribe(
      (res: any) => {
        this.subCategories = res;
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
  getPriorities() {
    this.settingService.getPriorites().subscribe(
      (res: any) => {
        this.priorities = res;
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
  getRequests() {
    this.settingService.getRequests().subscribe(
      (res: any) => {
        this.requests = res;
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
  getChannels() {
    this.settingService.getChannels().subscribe(
      (res: any) => {
        this.channels = res;
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
  getStatuses() {
    this.settingService.getStatuses().subscribe(
      (res: any) => {
        this.statuses = res;
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
  stripHtmlTags(htmlString: string): string {
    const tmp = document.createElement('div');
    tmp.innerHTML = htmlString;
    return tmp.textContent || tmp.innerText || '';
  }

  async createComment() {
    const textWithoutTags = this.stripHtmlTags(this.reply);
    if (textWithoutTags === "" || textWithoutTags === "undefined") {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail:
          "Please enter your comment first",
        life: 3000,
      });
    }
    else if (textWithoutTags !== "") {
      this.user = JSON.parse(localStorage.getItem("user") || "{}") as User;
      this.comment.userID = this.user.user_Id;
      this.comment.ticketID = this.ticket.id;
      this.comment.body = textWithoutTags;
      await this.ticketService.addComment(this.comment).then(
        async (res: any) => {
          await this.ticketService.getlastCommentId().subscribe(
            (res: any) => {
              this.comment.commentID = res;
              if (this.fileList2 !== null) {
                this.customRequest(this.fileList2);
                this.getComments();
                this.reply = '';
                this.router.navigateByUrl("/main/agent/tickets/details");
              }
            });
        },
        (error) => {
          this.logger.logError(error);
          const userLog = localStorage.getItem('userLogs') || '';
          const updatedUserLog = `${userLog}\n'error'${error}`;
          localStorage.setItem('userLogs', updatedUserLog);
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
  }
  async customRequest(files: NzUploadFile[]): Promise<void[]> {
    const promises: Promise<void>[] = [];

    for (const file of files) {
      promises.push(this.processFile(file));
    }

    return Promise.all(promises);
  }

  private async processFile(file: NzUploadFile): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        const blob = await this.getFileBlob(file);

        const buffer = await this.blobToArrayBuffer(blob);

        const uint8Array = new Uint8Array(buffer);

        const fileHash = await this.hashFileData(uint8Array);

        const base64Data = await this.arrayBufferToBase64(buffer);

        const fileHashString = this.arrayBufferToHex(fileHash);

        this.fileAttachment.ticketID = this.ticket.id || undefined;
        this.fileAttachment.fileName = file.name;
        this.fileAttachment.fileSize = file['size']?.toString() || undefined;
        this.fileAttachment.fileHash = fileHashString;
        this.fileAttachment.fileData = base64Data;
        this.fileAttachment.commentID = this.comment.commentID?.toString() || undefined;

        this.settingService.addFile(this.fileAttachment).then(
          (res) => {

          },
          (error) => {
            this.logger.logError(error);
            const userLog = localStorage.getItem('userLogs') || '';
            const updatedUserLog = `${userLog}\n'error'${error}`;
            localStorage.setItem('userLogs', updatedUserLog);
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail:
                "An error occurred while uploading attachments",
              life: 3000,
            });
          }
        )

        resolve();
      } catch (error) {
        console.error("Error processing file:", error);
        reject(error);
      }
    });
  }

  private async getFileBlob(file: NzUploadFile): Promise<Blob> {
    return new Promise<Blob>((resolve, reject) => {
      if (file.originFileObj) {
        resolve(file.originFileObj);
      } else {
        reject(new Error("File object not found."));
      }
    });
  }

  private async blobToArrayBuffer(blob: Blob): Promise<ArrayBuffer> {
    return new Promise<ArrayBuffer>((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = () => {
        if (reader.result instanceof ArrayBuffer) {
          resolve(reader.result);
        } else {
          reject(new Error("Failed to convert blob to ArrayBuffer."));
        }
      };

      reader.onerror = (event) => {
        reject(event.target?.error || new Error("File reading error."));
      };

      reader.readAsArrayBuffer(blob);
    });
  }

  private async hashFileData(data: Uint8Array): Promise<ArrayBuffer> {
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    return hashBuffer;
  }

  private arrayBufferToHex(buffer: ArrayBuffer): string {
    const hashArray = Array.from(new Uint8Array(buffer));
    return hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');
  }
  private arrayBufferToBase64(buffer: ArrayBuffer): string {
    const bytes = new Uint8Array(buffer);
    const binary = Array.from(bytes).map((byte) => String.fromCharCode(byte)).join('');
    return btoa(binary);
  }
  createCommentHierarchy(apiComments: Comment[]): Comment[] {
    const commentMap = new Map<number, Comment>();
    const roots: Comment[] = [];

    apiComments.forEach((comment) => {
      commentMap.set(comment.commentID as number, { ...comment, children: [] });
    });

    apiComments.forEach((comment) => {
      if (comment.ticketID) {
        const parentComment = commentMap.get(comment.ticketID as unknown as number);
        if (parentComment) {
          parentComment.children?.push(comment);
        } else {
          roots.push(comment);
        }
      }
    });
    return roots;
  }


  takeoverTicket() {
    this.ticketService.takeoverTicket(this.ticket.id, this.user.user_Id).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'You are the ticket handler now.!', life: 3000 });
        this.ticket.statusID = 1;
        this.ticket.assingedToUserID = this.user.user_Id;
        this.ticket.updateByUser = this.user.user_Id;
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.cdr.detectChanges();
        this.router.navigateByUrl("/main/agent/tickets/details");

      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
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
  closeTicket() {
    this.ticket.statusID = 3;
    this.ticket.updateByUser = this.user.user_Id;

    this.ticketService.closeTicket(this.ticket).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Ticket Closed', life: 3000 });
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.router.navigateByUrl("/main/agent/tickets/details");

      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
        this.router.navigateByUrl("error");
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

  formatCommentDate(commentDate: any) {
    const formattedDate = new Date(commentDate).toLocaleString(); // Format as a string
    return formattedDate;
  }
  async createCommentAndClose() {
    const textWithoutTags = this.stripHtmlTags(this.reply);
    if (textWithoutTags === "" || textWithoutTags === "undefined") {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail:
          "Please enter your comment first",
        life: 3000,
      });
    }
    else if (textWithoutTags !== "") {
      this.user = JSON.parse(localStorage.getItem("user") || "{}") as User;
      this.comment.userID = this.user.user_Id;
      this.comment.ticketID = this.ticket.id;
      this.comment.body = textWithoutTags;
      await this.ticketService.addComment(this.comment).then(
        async (res: any) => {
          await this.ticketService.getlastCommentId().subscribe(
            (res: any) => {
              this.comment.commentID = res;
              if (this.fileList2 !== null) {
                this.customRequest(this.fileList2);
                this.getComments();
                this.reply = '';
                localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
                this.router.navigateByUrl("/main/agent/tickets/details");
              }
            });
          await this.ticketService.closeTicket(this.ticket).then(
            (res: any) => {
              this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Ticket Closed', life: 3000 });
              this.ticket.statusID = 3;
              this.getComments();
              localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
              this.router.navigateByUrl("/main/agent/tickets/details");

            },
            (error) => {
              this.logger.logError(error);
              const userLog = localStorage.getItem('userLogs') || '';
              const updatedUserLog = `${userLog}\n'error'${error}`;
              localStorage.setItem('userLogs', updatedUserLog);
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
      );
    }
  }


  searchAgents(event: AutoCompleteCompleteEvent) {
    const searchTerm = event.query.toLowerCase();
    this.filteredAgentNames = this.agents
      .filter(agent =>
        agent.lastname.toLowerCase().includes(searchTerm) ||
        agent.firstname.toLowerCase().includes(searchTerm) ||
        agent.username.toLowerCase().includes(searchTerm)
      )
      .map(agent => {
        const fullName = `${agent.firstname} ${agent.lastname} (${agent.username})`;
        return fullName;
      });
  }
  assignToOtherAgent() {
    this.userService.getBackOfficeUsers().pipe(
      map((res: any) => {
        const agents = res; // Assuming res contains the list of agents

        // Create the mapping of agent names to agent objects
        const agentNameToAgentMap: { [name: string]: SelectedAgent } = {};

        agents.forEach((agent: SelectedAgent) => {
          const fullName = `${agent.firstname} ${agent.lastname} (${agent.username})`;
          agentNameToAgentMap[fullName] = agent;
        });

        return {
          agents,
          agentNameToAgentMap,
        };
      })
    ).subscribe((data: any) => {
      // Assign the agents list
      this.agents = data.agents;

      this.agentNameToAgentMap = data.agentNameToAgentMap;
    },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      }
    )
    this.assignAgentDialog = true;
  }
  assignNewAgent() {
    this.selectedAgentFullName = this.selectedAgent.lastname + " " + this.selectedAgent.firstname;
    this.ticketService.assignTicketToOtherAgent(this.ticket.id, this.selectedAgent.user_Id).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done', detail: 'Ticket assigned to ' + this.selectedAgentFullName, life: 3000 });
        if (this.selectedAgent?.user_Id !== undefined) {
          this.ticket.assingedToUserID = parseInt(this.selectedAgent.user_Id, 10);
        } else {
          // Handle the case where this.selectedAgent?.id is undefined
          // Handle the case where this.selectedAgent?.id is undefined
        }
        this.assignAgentDialog = false;
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.router.navigateByUrl("/main/agent/tickets/details");
      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
        this.router.navigateByUrl("error");
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

  reOpenTicket() {
    this.ticketService.reopenTicket(this.ticket).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Ticket Re-opened', life: 3000 });
        this.ticket.statusID = 1;
        this.ticket.updateByUser = this.user.user_Id;

        this.getComments();
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.router.navigateByUrl("/main/agent/tickets/details");

      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
        this.router.navigateByUrl("error");
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail:
            "An error occurred while connection to the database",
          life: 3000,
        });
      });
  }

  hideDialog() {
    this.assignAgentDialog = false;

  }

  onInput(e: Event): void {
    const value = (e.target as HTMLInputElement).value;
    if (!value || value.indexOf('@') >= 0) {
    } else {
    }
  }
  download(FileID: any, FileName: any) {
    const fileId = 1; // Replace with the actual file ID you want to download
    this.settingService.downloadFile(FileID).subscribe(
      (base64Content: string) => {
        const binaryData = atob(base64Content);
        const bytes = new Uint8Array(binaryData.length);
        for (let i = 0; i < binaryData.length; i++) {
          bytes[i] = binaryData.charCodeAt(i);
        }
        const blob = new Blob([bytes], { type: 'application/octet-stream' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = FileName; // Set the filename here
        a.click();
        window.URL.revokeObjectURL(url);
      });
  }
  createDownloadLink(blob: Blob, FileName: any) {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = FileName; // Set the filename here
    a.click();
    window.URL.revokeObjectURL(url);
  }

  editFlagChange(flag: any) {
    if (flag === "priority") {
      this.editFlag = "priority";
    }
    else if (flag === "category") {
      this.editFlag = "category";
    }
    else if (flag === "channel") {
      this.editFlag = "channel";
    }
    else if (flag === "department") {
      this.editFlag = "department";
    }
  }
  saveNewPriority() {
    this.ticket.priority = this.selectedPriority.levelID;
    this.ticketService.editTicket(this.ticket).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Ticket details updated succesfully', life: 3000 });
        this.editFlag = "";
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.router.navigateByUrl("/main/agent/tickets/details");
      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
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
  saveNewDepartment() {
    this.ticket.departmentID = this.selectedDepartment.departmentID;
    this.ticket.updateByUser = this.user.user_Id;
    this.ticketService.editTicket(this.ticket).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Ticket details updated succesfully', life: 3000 });
        this.editFlag = "";
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.router.navigateByUrl("/main/agent/tickets/details");
      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
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
  saveNewCategory() {
    this.ticket.categoryID = this.selectedCategory.categoryID;
    this.ticket.updateByUser = this.user.user_Id;
    this.ticket.subCategoryID = this.selectedSubCategory.subCategoryID;
    this.ticketService.editTicket(this.ticket).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Ticket details updated succesfully', life: 3000 });
        this.editFlag = "";
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.router.navigateByUrl("/main/agent/tickets/details");
      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
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
  saveNewChannel() {
    this.ticket.channelID = this.selectedChannel.channelID;
    this.ticket.updateByUser = this.user.user_Id;
    this.ticketService.editTicket(this.ticket).then(
      (res: any) => {
        this.messageService.add({ severity: 'success', summary: 'Done ', detail: 'Ticket details updated succesfully', life: 3000 });
        this.editFlag = "";
        localStorage.setItem("TicketDetails", JSON.stringify(this.ticket));
        this.router.navigateByUrl("/main/agent/tickets/details");
      },
      (error) => {
        this.logger.logError(error);
        const userLog = localStorage.getItem('userLogs') || '';
        const updatedUserLog = `${userLog}\n'error'${error}`;
        localStorage.setItem('userLogs', updatedUserLog);
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
}
