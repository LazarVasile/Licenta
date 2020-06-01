import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCreateMenuComponent } from './admin-create-menu.component';

describe('AdminCreateMenuComponent', () => {
  let component: AdminCreateMenuComponent;
  let fixture: ComponentFixture<AdminCreateMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminCreateMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCreateMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
