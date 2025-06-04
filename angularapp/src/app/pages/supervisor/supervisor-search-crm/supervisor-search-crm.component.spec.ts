import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorSearchCrmComponent } from './supervisor-search-crm.component';

describe('SupervisorSearchCrmComponent', () => {
  let component: SupervisorSearchCrmComponent;
  let fixture: ComponentFixture<SupervisorSearchCrmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SupervisorSearchCrmComponent]
    });
    fixture = TestBed.createComponent(SupervisorSearchCrmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
