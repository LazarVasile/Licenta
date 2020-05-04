import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAdaugareAdminComponent } from './admin-adaugare-admin.component';

describe('AdminAdaugareAdminComponent', () => {
  let component: AdminAdaugareAdminComponent;
  let fixture: ComponentFixture<AdminAdaugareAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminAdaugareAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAdaugareAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
