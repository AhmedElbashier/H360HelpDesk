import { Component } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-notfound',
  templateUrl: './notfound.component.html',
})
export class NotfoundComponent {
  constructor(private location: Location) { }

  onTryAgainClick() {
    this.location.back();
  }
}
