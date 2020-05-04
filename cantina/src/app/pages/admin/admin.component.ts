import { Component, OnInit, Inject } from '@angular/core';
import { UserService } from '../../services/user/user.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  public activeButton;
  public username = "";

  constructor(public _UserService : UserService) { }
  

  ngOnInit(): void {
    this.activeButton = true;
    this.username = localStorage.getItem("loggedUser");

  }

}
