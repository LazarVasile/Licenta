import { Component, OnInit } from '@angular/core';
import { ThrowStmt } from '@angular/compiler';
import { UserService } from '../../services/user/user.service';


@Component({
  selector: 'app-admin-adaugare-admin',
  templateUrl: './admin-adaugare-admin.component.html',
  styleUrls: ['./admin-adaugare-admin.component.css']
})
export class AdminAdaugareAdminComponent implements OnInit {

  public activeButton;

  constructor(public _UserService : UserService) { }

  ngOnInit(): void {
    this.activeButton = true;
  }

}
