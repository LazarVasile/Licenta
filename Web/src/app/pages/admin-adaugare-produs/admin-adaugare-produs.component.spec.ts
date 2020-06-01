import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAdaugareProdusComponent } from './admin-adaugare-produs.component';

describe('AdminAdaugareProdusComponent', () => {
  let component: AdminAdaugareProdusComponent;
  let fixture: ComponentFixture<AdminAdaugareProdusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminAdaugareProdusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAdaugareProdusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
