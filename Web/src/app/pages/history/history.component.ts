import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Location} from '@angular/common';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {

  public my_date;
  public myHistory;
  constructor(public _UserService : UserService, public _http : HttpClient, public _router : Router, public _location : Location) { }

  ngOnInit(): void {
  }

  showHistory() {
    if(this.my_date == null){
      console.log("Alegeti data")
    }
    else {
      this._http.get("https://localhost:5001/api/history/" + this.my_date)
      .subscribe({next : data => {
        console.log(data);
        this.myHistory = data;
      }});
    }
  }
}
