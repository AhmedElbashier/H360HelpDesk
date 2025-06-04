import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccessRoutingModule } from './access-routing.module';
import { AccessComponent } from './access.component';
import { ButtonModule } from 'primeng/button';

@NgModule({
  imports: [
    CommonModule,
    AccessRoutingModule,
    ButtonModule
  ],
  declarations: [AccessComponent]
})
export class AccessModule {


}
