import { Component, OnInit } from '@angular/core';
import { ThrowStmt } from '@angular/compiler';
import { UserService } from '../../services/user/user.service';
import { Http2ServerRequest } from 'http2';
import { HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-admin-adaugare-admin',
  templateUrl: './admin-adaugare-admin.component.html',
  styleUrls: ['./admin-adaugare-admin.component.css']
})
export class AdminAdaugareAdminComponent implements OnInit {

  public activeButton;
  public url = "https://localhost:5001/api/users";
  public token;
  public message;
  public displayMessage = "none"; 
  public error;
  public displayError = "none";

  constructor(public _http : HttpClient, public _UserService : UserService) { }

  ngOnInit(): void {
    this.activeButton = true;
    this.token = localStorage.getItem("token");
  }

  addAdmin(email){
    if (email == null || email == ""){
      this.displayError = "block";
      this.error = "Nu ați introdus adresa de email!"

    }
    else {
      var data = {"email" : email}
      
      this._http.put<any>(this.url, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}}).
      subscribe( data => {
        if(data["response"] == "true"){
          this.displayError = "none";
          this.message = "Admin adăugat cu succes!";
          this.displayMessage = "block";
        }
        else {
          this.error = "Eroare la adăugare admin! Încercați din nou!";
          this.displayError = "block";
        }
      },
      error => {
        if (error.status == 401){
          this._UserService.logout();
        }
      })
    }
    }
    
  }
  