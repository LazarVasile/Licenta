import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/products/product.service';
import { UserService } from '../../services/user/user.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ThrowStmt } from '@angular/compiler';
@Component({
  selector: 'app-admin-adaugare-produs',
  templateUrl: './admin-adaugare-produs.component.html',
  styleUrls: ['./admin-adaugare-produs.component.css']
})
export class AdminAdaugareProdusComponent implements OnInit {

  public activeButton;
  public categories;
  public displayError = "none";
  public error;
  public message = "";
  public token;
  public _urlProducts = "https://localhost:5001/api/products";
  public displayMessage = "none";

  constructor(public _productService : ProductService, public _UserService : UserService, private _router : Router, private _location : Location, private _http : HttpClient) { 
    this.categories = this._productService.getCategories();
  }

  ngOnInit(): void {
    this.token = localStorage.getItem("token");
    this.activeButton = true;
  }

  addProduct(name, category, professor_price, student_price, weight, description) {
    if(name == "" || category == "" || professor_price == "" || student_price == "" || weight == ""){
      this.displayMessage = "none";
      this.displayError = "block";
      this.error = "Nu ați completat toate câmpurile!";
      window.scroll(0, 0);
    }
    else {
      console.log(this._urlProducts);
      let data = {"name" : name, "category" : category, "professorPrice" : Number(professor_price), "studentPrice" : Number(student_price), "weight" : Number(weight), "description" : description};
      this._http.post<any>(this._urlProducts, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
      .subscribe(data =>
        {
          console.log(data);
          if (data == "succes"){
            this.displayError = "none";
            this.displayMessage = "block";
            this.message = "Produsul a fost adăugat cu succes!";
            window.scroll(0, 0);
            setTimeout(()=>{    //<<<---    using ()=> syntax
              this._UserService.refresh()     
            }, 3000)
          }
          else {
            this.displayMessage = "none";
            this.error = "A intervenit o eroare. Încercați din nou!";
            this.displayError = "block";
            window.scroll(0, 0);

          }
        },
        error => {
        if (error.status == 401) {
          this._UserService.logout();
        }      
      });
    }     
  }
  

  
}
