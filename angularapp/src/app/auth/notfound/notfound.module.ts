import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { NotfoundComponent } from './notfound.component';

@NgModule({
  imports: [
    CommonModule,
    ButtonModule
  ],
  declarations: [NotfoundComponent]
})
export class NotfoundModule {


}
