import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/products/product.service';
import { UserService } from '../../services/user/user.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
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

  constructor(public _productService : ProductService, public _UserService : UserService, private _router : Router, private _location : Location, private _http : HttpClient) { 
  }

  ngOnInit(): void {
    this.token = localStorage.getItem("token");
    this.activeButton = true;
    this.categories = this._productService.getCategories();
  }

  addProduct(name, category, professor_price, student_price, weight, description) {
    if(name == "" || category == "" || professor_price == "" || student_price == "" || weight == "" || description == ""){
      this.displayError = "block";
      this.error = "Nu ati completat toate campurile!";
    }
    else {
      console.log(this._urlProducts);
      let data = {"name" : name, "category" : category, "professorPrice" : Number(professor_price), "studentPrice" : Number(student_price), "weight" : Number(weight), "description" : description};
      this._http.post<any>(this._urlProducts, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
      .subscribe(data =>
        {
          console.log(data);
          if (data == "succes"){
            this.displayError = "block";
            this.error = "Produsul a fost adaugat cu succes!";
          }

          this._UserService.refresh();
        },
        error => {
        if (error.status == 401) {
          this._UserService.logout();
        }      
      });
    }     
  }
  

  
}
