import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  public error;
  public url = "https://localhost:5001/api/users/forgotpassword";
  public displayError = "none";
  constructor(public _http : HttpClient, public _router : Router) { }

  ngOnInit(): void {
  }

  sendEmail(value) {
    console.log(value);
    var data = {"email" : value};
    this._http.put<any>(this.url, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}})
    .subscribe({ next: data => {
      if (data['response'] == "true"){
        this.displayError = "block";
        this.error = "An email was sent to your email adress!";
      }
      else if (data['rseponse'] == "false") {
        this.displayError = "block";
        this.error = "Email adress is invalid! Please try again!";
      }
    }})
  }
}
