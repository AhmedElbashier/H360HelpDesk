import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackofficeDashboardComponent } from './backoffice-dashboard.component';

describe('BackofficeDashboardComponent', () => {
  let component: BackofficeDashboardComponent;
  let fixture: ComponentFixture<BackofficeDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BackofficeDashboardComponent]
    });
    fixture = TestBed.createComponent(BackofficeDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
