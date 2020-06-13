import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _urlLogin = "https://localhost:5001/api/login";
  private _urlRegister = "https://localhost:5001/api/register";
  constructor(private http: HttpClient, private _router : Router) { }

  login(email, password) : Observable<String> {
    let data = { "email" : email, "password" : password};
    return this.http.post<String>(this._urlLogin, data, {headers:{'Accept' : 'application/json', 'Content-Type' : 'application/json'}});

  } 

  register(email, password) : Observable<any> {
    let data = {"email" : email,  "password" : password};
    return this.http.post<any>(this._urlRegister, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}});
  }

  loggedIn() {
    // return if token exist or not in the browser
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
}
