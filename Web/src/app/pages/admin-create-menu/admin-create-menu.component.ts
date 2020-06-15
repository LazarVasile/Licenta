import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user/user.service';
import { ProductService } from '../../services/products/product.service';
import { IProduct } from '../../services/products/products';
import { timingSafeEqual } from 'crypto';
import { element } from 'protractor';
import { NgModel } from '@angular/forms';
import { ThrowStmt } from '@angular/compiler';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { HttpClient} from '@angular/common/http';
@Component({
  selector: 'app-admin-create-menu',
  templateUrl: './admin-create-menu.component.html',
  styleUrls: ['./admin-create-menu.component.css']
})
export class AdminCreateMenuComponent implements OnInit {

  public categories = ["Ciorbe si supe / Soups", "Garnituri / Side dishes", "Felul II", "Desert / Deserts", "Salate / Salads", "Paine / Bread", "Bauturi / Drinks"];
  // public productsOnCategories = new Array(any[]);
 
  public activeButton;
  public products;
  public productsToAdd = [];
  public indexes = [];
  public counter_categories = [];
  public list_ids = []
  private _urlMenus = "https://localhost:5001/api/menus";
  public date_menu;
  public list_cantities = [];
  public myProducts= {};
  public token;
  public error;
  public displayError = "none";

  constructor(private http: HttpClient, public _UserService : UserService, public _productService : ProductService, public _router: Router, public _location: Location) { }

  ngOnInit(): void {
    this.activeButton = true;
    this.token = localStorage.getItem("token");
    this._productService.getProducts().subscribe(data => {
      this.products = data;
      
    })

    for (let i = 0; i < this.categories.length; i++){
      this.counter_categories[this.categories[i]] = [1];
      this.productsToAdd[this.categories[i]] = [];
      this.indexes.push(i);
    }

  }

  addMoreProducts(category) {
    this.counter_categories[category].push(this.counter_categories[category].length + 1);
    this.indexes.push(this.indexes.length); 
  } 

  createMenu() {
    if (this.date_menu == null) {
      this.displayError = "block";
      this.error = "Va rugam sa selectati data!";
    }
    else {
      for (let i = 0; i < this.categories.length; i++)
      for (let j = 0; j < this.productsToAdd[this.categories[i]].length; j++){
        this.list_ids.push(this.productsToAdd[this.categories[i]][j])
      }
      
      let data = {"products" : this.myProducts, "dateMenu" : this.date_menu}
      this.http.post<any>(this._urlMenus, data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
      .subscribe(data =>
        {
          this._UserService.refresh()
    },
    error => {
      if (error.status == 401) {
        this._UserService.logout();
      }      
    })
    }
    
  }

  
  onChangeInput(event, product){
    this.myProducts[product.toString()] = +event.target.value;
  }

}
