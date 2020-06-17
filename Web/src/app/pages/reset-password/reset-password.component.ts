import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Md5 } from 'ts-md5/dist/md5';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  public displayError = "none";
  public error;
  public url = "https://localhost:5001/api/users/resetpassword";
  public token;
  constructor(private activatedRoute: ActivatedRoute, public _http : HttpClient, public _router : Router) {
    this.activatedRoute.queryParams.subscribe(params => {
      this.token = params['token'].toString();
      console.log(this.token);
  });
   }

  ngOnInit(): void {
  }

  resetPassword(password, confirmPassword){
    if(password != confirmPassword){
      this.displayError = "block";
      this.error = "Password and confirm password doesn't match!";
    }
    else if(password.length > 16) {
      this.displayError = "block";
      this.error = "Password too long";
    }
    else if(password.length < 8) {
      this.displayError = "block";
      this.error = "Password too short"; 
    }
    else {
      var passwordMD5 = Md5.hashStr(password);
      console.log(this.token);
      var data = {"password" : passwordMD5, "token" : this.token};
      this._http.post<any>(this.url, data, {headers : {'Accept' : 'application/json;', 'Content-Type' : 'application/json; charset=utf-8  ', 'Authorization' : 'Bearer '+ this.token}})
      .subscribe( data => { 
        if(data["response"] == "true"){
          this._router.navigate(['/login']);
        }
        else {
          this.displayError = "block";
          this.error = "Ceva nu a mers. Încercați din nou!";
        }
      },
      error => {
        console.log(error);
        if (error.status == 401) {
          this.displayError = "block";
          this.error = "Link-ul de schimbare parolă a expirat. Incercați din nou!"
        }
      });
      
    }
  }

}
