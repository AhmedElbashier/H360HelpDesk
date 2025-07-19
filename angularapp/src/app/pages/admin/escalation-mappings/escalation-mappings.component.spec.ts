import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalationMappingsComponent } from './escalation-mappings.component';

describe('EscalationMappingsComponent', () => {
  let component: EscalationMappingsComponent;
  let fixture: ComponentFixture<EscalationMappingsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EscalationMappingsComponent]
    });
    fixture = TestBed.createComponent(EscalationMappingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
