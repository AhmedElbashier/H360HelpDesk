import { Component } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-access',
  templateUrl: './access.component.html',
})
export class AccessComponent {
  constructor(private location: Location) { }

  onTryAgainClick() {
    this.location.back();
  }
}
