import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackofficeTicketDetailsComponent } from './backoffice-ticket-details.component';

describe('BackofficeTicketDetailsComponent', () => {
  let component: BackofficeTicketDetailsComponent;
  let fixture: ComponentFixture<BackofficeTicketDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BackofficeTicketDetailsComponent]
    });
    fixture = TestBed.createComponent(BackofficeTicketDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
