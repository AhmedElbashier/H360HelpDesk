import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackofficeReportsComponent } from './backoffice-reports.component';

describe('BackofficeReportsComponent', () => {
  let component: BackofficeReportsComponent;
  let fixture: ComponentFixture<BackofficeReportsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BackofficeReportsComponent]
    });
    fixture = TestBed.createComponent(BackofficeReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
