import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoSearchCrmComponent } from './auto-search-crm.component';

describe('AutoSearchCrmComponent', () => {
  let component: AutoSearchCrmComponent;
  let fixture: ComponentFixture<AutoSearchCrmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AutoSearchCrmComponent]
    });
    fixture = TestBed.createComponent(AutoSearchCrmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
