import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorReportsComponent } from './supervisor-reports.component';

describe('SupervisorReportsComponent', () => {
  let component: SupervisorReportsComponent;
  let fixture: ComponentFixture<SupervisorReportsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SupervisorReportsComponent]
    });
    fixture = TestBed.createComponent(SupervisorReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
