import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorTicketDetailsComponent } from './supervisor-ticket-details.component';

describe('SupervisorTicketDetailsComponent', () => {
  let component: SupervisorTicketDetailsComponent;
  let fixture: ComponentFixture<SupervisorTicketDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SupervisorTicketDetailsComponent]
    });
    fixture = TestBed.createComponent(SupervisorTicketDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
