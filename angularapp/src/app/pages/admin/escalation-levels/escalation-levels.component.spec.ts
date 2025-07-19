import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationLevelsComponent } from './escalation-levels.component';

describe('EscalationLevelsComponent', () => {
  let component: EscalationLevelsComponent;
  let fixture: ComponentFixture<EscalationLevelsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EscalationLevelsComponent]
    });
    fixture = TestBed.createComponent(EscalationLevelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
