import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Location } from '@angular/common';


@Component({
  selector: 'app-update-product',
  templateUrl: './update-product.component.html',
  styleUrls: ['./update-product.component.css']
})
export class UpdateProductComponent implements OnInit {

  public myProducts;
  public urlProducts = "https://localhost:5001/api/products";
  public error = "";
  public displayError = "none";
  public token;
  public displayMessage = "none";
  public message = "";

  constructor(public _UserService : UserService, public _http : HttpClient, public _router : Router, public _location : Location) { }

  ngOnInit(): void {
    this.token = localStorage.getItem("token");
    this.getProducts();
  }

  getProducts() {
    this._http.get<any>(this.urlProducts, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
    .subscribe(data => {
        this.myProducts = data;
    },
    error => {
      if (error.status == 401) {
        this._UserService.logout();
      }
    });


  }

  updateProduct(id, professorPrice, studentPrice, weight, description ) {
    if(id == ""){
      this.displayMessage = "none";
      this.displayError = "block";
      this.error = "Nu ați selectat niciun produs, incercați din nou!";
      window.scroll(0, 0);
      
    }
    else {
      var data = {"id" : id, "professor_price" : professorPrice, "student_price" : studentPrice, "weight" : weight, "description" : description};
      this._http.put<any>(this.urlProducts,  data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
      .subscribe(data =>{
        this.displayError = "none";
        this.message = "Produsul a fost modificat cu succes!";
        this.displayMessage = "block";
        setTimeout(()=>{    //<<<---    using ()=> syntax
          this._UserService.refresh()     
        }, 3000)
      },
      error => {
        if (error.status == 401) {
          this._UserService.logout();
        }
      });


    }
    
  }

}
