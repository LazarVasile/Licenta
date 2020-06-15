import { Component, OnInit, Inject } from '@angular/core';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import {Product, SellProduct, BuyProduct} from '../../classes/product';
import { DatePipe } from '@angular/common';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { IProduct } from 'src/app/services/products/products';
import { Route } from '@angular/compiler/src/core';
import { ProductService } from '../../services/products/product.service';
import { timeStamp, error } from 'console';
import { JitEmitterVisitor } from '@angular/compiler/src/output/output_jit';


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  public activeButton;
  public username = "";
  public pipe = new DatePipe('en-US'); // Use your own local
  private _url = "https://localhost:5001/api/usermenu/";
  public formatDate = this.pipe.transform(Date.now(), "yyyy-MM-dd")
  public myProducts = [];
  public sellProducts = [];
  public totalPrice = 0;
  public urlCodes = "https://localhost:5001/api/codes/";
  public urlUserMenu = "https://localhost:5001/api/menus";
  public productsByCategory : {[category : string] : Array<IProduct>} = {};
  public ticketProducts = [];
  public categories;
  public buyProductsNumber : {[id : string] : number} = {};
  public buyProductsFinal: {[id : string] : number} = {};
  public buyProductsTotal : {[id : string] : number} = {}; 
  public buyProducts;
  public ok = false;
  public myMenu = [];
  public token;
  public error;
  public displayError = "none";


  constructor(public _UserService : UserService, public _http: HttpClient, public _location : Location, public _router : Router, public _productService : ProductService) {
    this.categories = _productService.getCategories();

   }
  
  ngOnInit(): void {
    this.activeButton = true;
    this.username = localStorage.getItem("loggedUser");
    this.token = localStorage.getItem("token");
    this.getProducts(this.formatDate);

  }

  getProducts(date) {
    this.totalPrice = 0;
    this.buyProductsNumber = {};
    this.buyProductsTotal = {};
    this.initCategories();
      var url = this._url + date;
      this._http.get<any>(url, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
      .subscribe( data => {
          this.myProducts = data;
          for (let i = 0; i < this.myProducts.length; i++){
            this.productsByCategory[this.myProducts[i].category].push(this.myProducts[i]);
          }
          this._http.get<any>("https://localhost:5001/api/menus/" + date, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
          .subscribe(data =>
            {
              console.log(data);
              if (data != null){

                this.myMenu = data;

                for (let key in this.myMenu["productsIdAndAmounts"]){
                  this.buyProductsNumber[key.toString()] = 0;
                  this.buyProductsTotal[key.toString()] = this.myMenu["productsIdAndAmounts"][key];
                }
                }
                },
                error => {
              if (error.status == 401) {
                this._UserService.logout();
              }      
            }
          )},
      error => {
        if (error.status == 401) {
          this._UserService.logout();
        }      
      }
      )}

  initCategories() {
    for (let i = 0; i < this.categories.length; i++){
      this.productsByCategory[this.categories[i]] = [];
    }
  }

  addProduct(product) {
    if (this.buyProductsTotal[product["_id"].toString()] == 0){
      this.error = "Produsul nu mai este disponibil! Va rugam sa alegeti altceva!";
      this.displayError = "block";
    }
    else {
      this.totalPrice = Number((this.totalPrice + product.studentPrice).toFixed(2)); 
      this.buyProductsTotal[product["_id"].toString()] -= 1;  
      this.buyProductsNumber[product['_id'].toString()] += 1;
    }
  }



  minusProduct(product) {
    if (this.buyProductsNumber[product['_id'].toString()] > 0){
      this.totalPrice = Number((this.totalPrice - product.studentPrice).toFixed(2)); 
      this.buyProductsTotal[product['_id'].toString()] += 1;
      this.buyProductsNumber[product["_id"].toString()] -= 1;
    }
  }


  GeneratePersonalMenu(value) {
    this.buyProductsNumber = {}
    this.buyProductsTotal = {};
    this.buyProductsTotal = {};
    this.initCategories();
    this._http.get<any>(this.urlCodes + value, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}})
    .subscribe(dataGet => {
      console.log(dataGet);
      this.ok = true;
      for (let key in dataGet["idProductsAndAmounts"]) {
        for (let j = 0; j < this.myProducts.length; j++){
          if (key == this.myProducts[j]['_id']){
            this.buyProductsTotal[this.myProducts[j]] = dataGet["idProductsAndAmounts"][key];
            this.buyProductsFinal[this.myProducts[j]['_id'].toString()] =  dataGet["idProductsAndAmounts"][key];
            this.ticketProducts.push(this.myProducts[j]);
          } 
        }
      }
      this.buyProductsFinal["total_price"] = dataGet["totalPrice"];
      this.totalPrice = dataGet["totalPrice"];
      this.myProducts = this.ticketProducts;
      for (let i = 0; i < this.myProducts.length; i++){
        this.productsByCategory[this.myProducts[i].category].push(this.myProducts[i]);
      }

    },
    error => {
      if (error.status == 401) {
        this._UserService.logout();
      }      
    }
    )    
  }

  refresh() {
    this._router.navigateByUrl('.', { skipLocationChange: true }).then(() => {
      this._router.navigate([decodeURI(this._location.path())]);
  }); 
  }

  BuyProducts(){ 
    if(this.totalPrice != 0){
      if (this.ok == false){
        for (let i  = 0; i < this.myProducts.length; i++){
          if (this.buyProductsNumber[this.myProducts[i]['_id'].toString()] > 0){
            this.buyProductsFinal[this.myProducts[i]['_id'].toString()] = this.buyProductsNumber[this.myProducts[i]['_id'].toString()];
          }
        }
        this.buyProductsFinal["total_price"] = this.totalPrice;
      }
    
    this._http.put<any>(this.urlUserMenu, this.buyProductsFinal, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}, observe : "response"})
      .subscribe(response => {
          console.log(response);
          if(response.status == 401){
            this._UserService.logout;
          }
          this.refresh();
      
        },
        error => {
          if (error.status == 401) {
            this._UserService.logout();
          }      
        });
    }
    else {
      this.displayError = "block";
      this.error = "Nu ati ales nimic. Incercati din nou!";
    }
  }

}
