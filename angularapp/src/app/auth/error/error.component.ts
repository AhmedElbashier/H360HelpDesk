import { Component } from '@angular/core';
import { Location } from '@angular/common';
@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
})
export class ErrorComponent {

  constructor(private location: Location) { }

  onTryAgainClick() {
    this.location.back();
  }
}
