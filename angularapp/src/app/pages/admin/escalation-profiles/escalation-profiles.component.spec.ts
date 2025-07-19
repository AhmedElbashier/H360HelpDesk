import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationProfilesComponent } from './escalation-profiles.component';

describe('EscalationProfilesComponent', () => {
  let component: EscalationProfilesComponent;
  let fixture: ComponentFixture<EscalationProfilesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EscalationProfilesComponent]
    });
    fixture = TestBed.createComponent(EscalationProfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
