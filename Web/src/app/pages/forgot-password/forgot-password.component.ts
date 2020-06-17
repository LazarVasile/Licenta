import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ThrowStmt } from '@angular/compiler';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  public message;
  public url = "https://localhost:5001/api/users/forgotpassword";
  public displayMessage = "none";
  public error;
  public displayError = "none";
  constructor(public _http : HttpClient, public _router : Router) { }

  ngOnInit(): void {
  }

  sendEmail(value) {
    if (value == ""){
      this.error = "Nu ați introdus adresa de email!";
      this.displayError = "block";
    }
    else {
      var data = {"email" : value};
      this._http.put<any>(this.url, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}})
      .subscribe({ next: data => {
        if (data['response'] == "true"){
          this.message = "A fost trimis un link către pagina de resetare parolă pe adresa dumneavoastră de email!";
          this.displayMessage = "block";
        }
        else if (data['rseponse'] == "false") {
          this.displayError = "block";
          this.error = "Adresa de email nu este validă! Vă rugăm să incercați din nou!";
        }
      }})
    }
  }
}
