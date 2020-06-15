import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Location} from '@angular/common';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  public myProducts;
  public myProductDetails = [];
  public searchText;
  public urlProducts = "https://localhost:5001/api/products";
  public token;

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
    }      
    );

  }
  
}
