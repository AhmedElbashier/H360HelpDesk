import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchCrmComponent } from './search-crm.component';

describe('SearchCrmComponent', () => {
  let component: SearchCrmComponent;
  let fixture: ComponentFixture<SearchCrmComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SearchCrmComponent]
    });
    fixture = TestBed.createComponent(SearchCrmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
