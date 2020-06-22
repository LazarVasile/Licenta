import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Location} from '@angular/common';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import { ThrowStmt } from '@angular/compiler';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {

  public my_date;
  public myHistory;
  public token;
  public displayError = "none";
  public error;
  public _url = "https://localhost:5001/api/orders/history/";
  constructor(public _UserService : UserService, public _http : HttpClient, public _router : Router, public _location : Location) { }

  ngOnInit(): void {
    this.token = localStorage.getItem("token");
  }

  showHistory() {
    this.myHistory = null;
    if(this.my_date == null){
      this.displayError = "block";
      this.error = "Alegeti data!";
      window.scroll(0, 0);
    }
    else {
      console.log(this.my_date);
      this._http.get(this._url + this.my_date, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
      .subscribe(data => {
        this.myHistory = data;
      },
      error => {
        if (error.status == 401) {
          this._UserService.logout();
        }
      });
    }
  }
}
