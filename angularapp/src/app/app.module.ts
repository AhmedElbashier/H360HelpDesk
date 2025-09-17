import { BrowserModule } from '@angular/platform-browser';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, ErrorHandler, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { ErrorInterceptor } from './error.interceptor';
import { JwtInterceptor } from './services/jwt.interceptor';
import { RouterModule } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { CheckboxModule } from 'primeng/checkbox';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { SplitButtonModule } from 'primeng/splitbutton';
import { MenuModule } from 'primeng/menu';
import { InputTextModule } from 'primeng/inputtext';
import { MenubarModule } from 'primeng/menubar';
import { StyleClassModule } from 'primeng/styleclass';
import { DividerModule } from 'primeng/divider';
import { TabViewModule } from 'primeng/tabview';
import { CommonModule, registerLocaleData } from '@angular/common';
import { TableModule } from 'primeng/table';
import { SliderModule } from 'primeng/slider';
import { InputSwitchModule } from 'primeng/inputswitch';
import { SidebarModule } from 'primeng/sidebar';
import { TimelineModule } from 'primeng/timeline';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { MultiSelectModule } from 'primeng/multiselect';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { InputMaskModule } from 'primeng/inputmask';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressBarModule } from 'primeng/progressbar';
import { FileUploadModule } from 'primeng/fileupload';
import { ToolbarModule } from 'primeng/toolbar';
import { RatingModule } from 'primeng/rating';
import { CardModule } from 'primeng/card';
import { SplitterModule } from 'primeng/splitter';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { PasswordModule } from 'primeng/password';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ChartModule } from 'primeng/chart';
import { TagModule } from 'primeng/tag';
import { CalendarModule } from 'primeng/calendar';
import { ChipModule } from 'primeng/chip';
import { MegaMenuModule } from 'primeng/megamenu';
import { MessageModule } from 'primeng/message';
import { DataViewModule } from 'primeng/dataview';
import { SkeletonModule } from 'primeng/skeleton';
import { ConfirmationService, MessageService } from 'primeng/api';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { MessagesModule } from 'primeng/messages';
import { FieldsetModule } from 'primeng/fieldset';
import { AvatarModule } from 'primeng/avatar';
import { StepsModule } from 'primeng/steps';
import { NzLayoutModule } from "ng-zorro-antd/layout"
import { NzFormModule } from "ng-zorro-antd/form"
import { NzInputModule } from "ng-zorro-antd/input"
import { NzSkeletonModule} from "ng-zorro-antd/skeleton"
import { NzSelectModule } from "ng-zorro-antd/select"
import { NzDividerModule } from "ng-zorro-antd/divider"
import { NzSwitchModule } from "ng-zorro-antd/switch"
import { NzCommentModule } from "ng-zorro-antd/comment"
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzAutocompleteModule } from "ng-zorro-antd/auto-complete"
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker'; // Import the module
import { MailOutline, AppstoreOutline, SettingOutline, MenuFoldOutline, MenuUnfoldOutline, HomeOutline, LogoutOutline, DownloadOutline, UserOutline } from '@ant-design/icons-angular/icons';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';



import { JwtModule, JwtModuleOptions } from '@auth0/angular-jwt';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { en_US } from 'ng-zorro-antd/i18n';
import en from '@angular/common/locales/en';
import { AuthGuardService as AuthGuard, AuthGuardService } from "./services/auth.guard.service";
import { tokenGetter } from './services/auth.service';
import { NgxImageCompressService } from "ngx-image-compress";
import { GlobalErrorHandler } from './global.error.handler.module';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { ErrorComponent } from './auth/error/error.component';
import { AccessComponent } from './auth/access/access.component';
import { NotfoundComponent } from './auth/notfound/notfound.component';
import { HttpLoaderFactory } from './translations.module';
import { TranslationsModule } from './translations.module';
import { MainComponent } from './main/main.component';
import { AgentDashboardComponent } from './pages/agent/agent-dashboard/agent-dashboard.component';
import { ChannelComponent } from './pages/admin/channel/channel.component';
import { ReportsComponent } from './pages/agent/reports/reports.component';
import { AttachmentComponent } from './pages/admin/attachment/attachment.component';
import { DepartmentComponent } from './pages/admin/department/department.component';
import { CompanyComponent } from './pages/admin/company/company.component';
import { PriorityComponent } from './pages/admin/priority/priority.component';
import { StatusComponent } from './pages/admin/status/status.component';
import { SubCategoryComponent } from './pages/admin/sub-category/sub-category.component';
import { CategoryComponent } from './pages/admin/category/category.component';
import { TicketComponent } from './pages/admin/ticket/ticket.component';
import { UserDetailsComponent } from './pages/admin/user-details/user-details.component';
import { UserAddComponent } from './pages/admin/user-add/user-add.component';
import { UserEditComponent } from './pages/admin/user-edit/user-edit.component';
import { UserComponent } from './pages/admin/user/user.component';
import { TicketDetailsComponent } from './pages/agent/ticket-details/ticket-details.component';
import { RequestComponent } from './pages/admin/request/request.component';
import { TicketNewComponent } from './pages/agent/ticket-new/ticket-new.component';
import { SearchCrmComponent } from './pages/agent/search-crm/search-crm.component';
import { BackofficeDashboardComponent } from './pages/back-office/backoffice-dashboard/backoffice-dashboard.component';
import { BackofficeReportsComponent } from './pages/back-office/backoffice-reports/backoffice-reports.component';
import { BackofficeTicketDetailsComponent } from './pages/back-office/backoffice-ticket-details/backoffice-ticket-details.component';
import { EscalationsComponent } from './pages/admin/escalations/escalations.component';
import { SupervisorDashboardComponent } from './pages/supervisor/supervisor-dashboard/supervisor-dashboard.component';
import { SupervisorTicketNewComponent } from './pages/supervisor/supervisor-ticket-new/supervisor-ticket-new.component';
import { SupervisorSearchCrmComponent } from './pages/supervisor/supervisor-search-crm/supervisor-search-crm.component';
import { SupervisorTicketDetailsComponent } from './pages/supervisor/supervisor-ticket-details/supervisor-ticket-details.component';
import { SupervisorReportsComponent } from './pages/supervisor/supervisor-reports/supervisor-reports.component';
import { CommonService } from './services/common.service';
import { EscalationProfilesComponent } from './pages/admin/escalation-profiles/escalation-profiles.component';
import { EscalationLevelsComponent } from './pages/admin/escalation-levels/escalation-levels.component';
import { EscalationMappingsComponent } from './pages/admin/escalation-mappings/escalation-mappings.component';
import { AutoSearchCrmComponent } from './pages/agent/auto-search-crm/auto-search-crm.component';


registerLocaleData(en);
const JWT_Module_Options: JwtModuleOptions = {
  config: {
    tokenGetter: tokenGetter,
    //whitelistedDomains: ["localhost:8000"]
  }
};

@NgModule({
  declarations: [
    AppComponent,
    ErrorComponent,
    AccessComponent,
    NotfoundComponent,

    LoginComponent,
    MainComponent,
    TicketComponent,
    CategoryComponent,
    SubCategoryComponent,
    StatusComponent,
    PriorityComponent,
    CompanyComponent,
    DepartmentComponent,
    AttachmentComponent,
    ReportsComponent,
    UserComponent,
    UserEditComponent,
    UserAddComponent,
    UserDetailsComponent,
    ChannelComponent,
    RequestComponent,
    EscalationsComponent,
    EscalationProfilesComponent,
    EscalationLevelsComponent,
    EscalationMappingsComponent,

    AgentDashboardComponent,
    TicketDetailsComponent,
    ReportsComponent,
    TicketNewComponent,
    SearchCrmComponent,
    AutoSearchCrmComponent,


    BackofficeDashboardComponent,
    BackofficeReportsComponent,
    BackofficeTicketDetailsComponent,



    SupervisorDashboardComponent,
    SupervisorReportsComponent,
    SupervisorSearchCrmComponent,
    SupervisorTicketNewComponent,
    SupervisorTicketDetailsComponent,

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    RippleModule,
    RouterModule,
    InputTextModule,
    SplitterModule,
    CardModule,
    DividerModule,
    MenubarModule,
    CommonModule,
    BrowserAnimationsModule,
    CalendarModule,
    SliderModule,
    DialogModule,
    MultiSelectModule,
    MenuModule,
    ContextMenuModule,
    DropdownModule,
    ToastModule,
    ProgressBarModule,
    FileUploadModule,
    ToolbarModule,
    TagModule,
    RatingModule,
    FormsModule,
    RadioButtonModule,
    FieldsetModule,
    InputNumberModule,
    AutoCompleteModule,
    ConfirmDialogModule,
    InputTextareaModule,
    TabViewModule,
    PasswordModule,
    ChartModule,
    DataViewModule,
    ChipModule,
    CheckboxModule,
    StyleClassModule,
    ButtonModule,
    SplitButtonModule,
    TimelineModule,
    NzMenuModule,
    MessagesModule,
    NzToolTipModule,
    OverlayPanelModule,
    ReactiveFormsModule,
    ProgressSpinnerModule,
    MessageModule,
    SkeletonModule,
    NzLayoutModule,
    AvatarModule,
    NzFormModule,
    NzSelectModule,
    NzDrawerModule,
    NzSpaceModule,
    NzDatePickerModule,
    NzSwitchModule,
    NzSkeletonModule,
    TableModule,
    InputMaskModule,
    SidebarModule,
    MegaMenuModule,
    InputSwitchModule,
    NzCardModule,
    StepsModule,
    CKEditorModule,
    NzUploadModule,
    NzAutocompleteModule,
    NzCommentModule,
    NzAvatarModule,
    NzRadioModule,
    NzDividerModule,
    NzButtonModule,
    NzInputModule,
    NzCheckboxModule,
    JwtModule.forRoot(JWT_Module_Options),
    NzIconModule.forRoot([MailOutline, AppstoreOutline, SettingOutline, MenuFoldOutline, MenuUnfoldOutline, HomeOutline, LogoutOutline, DownloadOutline, UserOutline]),
    LoggerModule.forRoot({ level: NgxLoggerLevel.TRACE }), // Adjust the log level as needed
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    RouterModule.forRoot([
      { path: '', pathMatch: 'full', redirectTo: "/login" },
      { path: 'login', component: LoginComponent },
      { path: 'error', component: ErrorComponent },
      { path: 'access', component: AccessComponent },
      { path: 'notfound', component: NotfoundComponent },
      //{ path: '**', redirectTo: '/notfound' },
      {
        canActivate: [AuthGuard],
        path: 'main', component: MainComponent, children: [
          { path: 'admin/users', component: UserComponent },
          { path: 'admin/user-edit', component: UserEditComponent },
          { path: 'admin/user-add', component: UserAddComponent },
          { path: 'admin/user-details', component: UserDetailsComponent },
          { path: 'admin/tickets', component: TicketComponent },
          { path: 'admin/categories', component: CategoryComponent },
          { path: 'admin/subcategories', component: SubCategoryComponent },
          { path: 'admin/status', component: StatusComponent },
          { path: 'admin/priority', component: PriorityComponent },
          { path: 'admin/companies', component: CompanyComponent },
          { path: 'admin/departments', component: DepartmentComponent },
          { path: 'admin/attachments', component: AttachmentComponent },
          { path: 'admin/reports', component: ReportsComponent },
          { path: 'admin/channels', component: ChannelComponent },
          { path: 'admin/requests', component: RequestComponent },
          { path: 'admin/escalations', component: EscalationsComponent },
          { path: 'admin/escalations/profiles', component: EscalationProfilesComponent },
          { path: 'admin/escalations/levels', component: EscalationLevelsComponent },
          { path: 'admin/escalations/mappings', component: EscalationMappingsComponent },


          { path: 'agent/tickets/dashboard', component: AgentDashboardComponent },
          { path: 'agent/tickets/new', component: TicketNewComponent },
          { path: 'agent/tickets/crm', component: SearchCrmComponent },
          { path: 'agent/tickets/details', component: TicketDetailsComponent },
          { path: 'agent/reports', component: ReportsComponent },
          { path: 'agent/tickets/crm/search/:phone',component: AutoSearchCrmComponent},


          { path: 'backoffice/tickets/dashboard', component: BackofficeDashboardComponent },
          { path: 'backoffice/tickets/details', component: BackofficeTicketDetailsComponent },
          { path: 'backoffice/reports', component: BackofficeReportsComponent },




          { path: 'supervisor/tickets/dashboard', component: SupervisorDashboardComponent },
          { path: 'supervisor/tickets/new', component: SupervisorTicketNewComponent },
          { path: 'supervisor/tickets/crm', component: SupervisorSearchCrmComponent },
          { path: 'supervisor/tickets/details', component: SupervisorTicketDetailsComponent },
          { path: 'supervisor/reports', component: SupervisorReportsComponent },
        ]
      },
    ],{ useHash: true }), 
  ],
  providers: [
    { provide: NZ_I18N, useValue: en_US },

    //{ provide: ErrorHandler, useClass: GlobalErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    // { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }, // Disabled - API uses Basic Auth
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    MessageService, ConfirmationService, AuthGuardService, NgxImageCompressService
  ],
  bootstrap: [AppComponent ]
})
export class AppModule { }
