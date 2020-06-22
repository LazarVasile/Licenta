import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  public _urlLogin = "https://localhost:5001/api/users/login";
  public _urlRegister = "https://localhost:5001/api/users";

  constructor(public http: HttpClient, public _router : Router, public _location : Location) { }

  login(email, password) : Observable<String> {
    let data = { "email" : email, "password" : password, "type" : "web"};
    return this.http.post<String>(this._urlLogin, data, {headers:{'Accept' : 'application/json', 'Content-Type' : 'application/json'}});

  } 

  register(email, password, type) : Observable<any> {
    let data = {"email" : email,  "password" : password, "type" : type};
    return this.http.post<any>(this._urlRegister, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}});
  }

  loggedIn() {
    return !!localStorage.getItem('token');
  }

  getToken() {
    return localStorage.getItem('token');
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('loggedUser');
    localStorage.removeItem('type');
    localStorage.removeItem('id_user');
    this._router.navigate(['/login']);
  }

  refresh() {
    this._router.navigateByUrl('.', { skipLocationChange: true }).then(() => {
      this._router.navigate([decodeURI(this._location.path())]);
  }); 
}
}
