import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorTicketNewComponent } from './supervisor-ticket-new.component';

describe('SupervisorTicketNewComponent', () => {
  let component: SupervisorTicketNewComponent;
  let fixture: ComponentFixture<SupervisorTicketNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SupervisorTicketNewComponent]
    });
    fixture = TestBed.createComponent(SupervisorTicketNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
