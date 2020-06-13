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


  constructor(public _UserService : UserService, public _http: HttpClient, public _location : Location, public _router : Router, public _productService : ProductService) {
    this.categories = _productService.getCategories();

   }
  

  ngOnInit(): void {
    this.activeButton = true;
    this.username = localStorage.getItem("loggedUser");
    this.getProducts(this.formatDate);

  }

  getProducts(date) {
    this.totalPrice = 0;
    this.buyProductsNumber = {};
    this.buyProductsTotal = {};
    this.initCategories();
    // const promise = new Promise((resolve, reject) => {
      var url = this._url + date;
      this._http.get<any>(url)
      .subscribe( data => {
          this.myProducts = data;
          for (let i = 0; i < this.myProducts.length; i++){
            this.productsByCategory[this.myProducts[i].category].push(this.myProducts[i]);
          }
          console.log(this.myProducts);
          console.log(date);
          this._http.get<any>("https://localhost:5001/api/menus/" + date)
          .subscribe(data =>
            {
              this.myMenu = data;
              for (let i = 0; i < this.myProducts.length; i++){
                this.buyProductsNumber[this.myProducts[i]['_id'].toString()] = 0;
                let seachProduct = this.myMenu.find(x => x['productId'] == this.myProducts[i]['_id']);
                this.buyProductsTotal[this.myProducts[i]['_id'].toString()] = seachProduct['productCantity'];
              }
              console.log(this.buyProductsNumber);
              console.log(this.buyProductsTotal);
            },
          )
      }
      )
  }

  initCategories() {

    for (let i = 0; i < this.categories.length; i++){
      this.productsByCategory[this.categories[i]] = [];
    }
    
  }
  


 

  addProduct(product) {
    // if (this.buyProductsTotal[product["_id"].toString()] == 0){
    //   this.error = "Produsul nu mai este disponibil! Va rugam sa alegeti altceva!";
    //   this.displayError = "block";
    // }
    // else {
    // console.log(this.buyProducts);
      this.totalPrice = Number((this.totalPrice + product.student_price).toFixed(2)); 
      this.buyProductsTotal[product["_id"].toString()] -= 1;  
      this.buyProductsNumber[product['_id'].toString()] += 1;
      // console.log(this.buyProductsNumber);
      // console.log(this.buyProductsTotal);
    // }
  }



  minusProduct(product) {
    if (this.buyProductsNumber[product['_id'].toString()] > 0){
      this.totalPrice = Number((this.totalPrice - product.student_price).toFixed(2)); 
      this.buyProductsTotal[product['_id'].toString()] += 1;
      this.buyProductsNumber[product["_id"].toString()] -= 1;
    }
  }


  GeneratePersonalMenu(value) {
    this.buyProductsNumber = {}
    this.buyProductsTotal = {};
    this.buyProductsTotal = {};
    this.initCategories();
    console.log("value: " + value);
    this._http.get<any>(this.urlCodes + value)
    .subscribe({next: dataGet => {
      this.ok = true;
      console.log(dataGet);
      console.log(dataGet["totalPrice"]);
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

    }});    
  }

  refresh() {
    this._router.navigateByUrl('.', { skipLocationChange: true }).then(() => {
      this._router.navigate([decodeURI(this._location.path())]);
  }); 
  }

  BuyProducts(){ 
    console.log(this.buyProductsNumber);
    if (this.ok == false){
      for (let i  = 0; i < this.myProducts.length; i++){
        if (this.buyProductsNumber[this.myProducts[i]['_id'].toString()] > 0){
          this.buyProductsFinal[this.myProducts[i]['_id'].toString()] = this.buyProductsNumber[this.myProducts[i]['_id'].toString()];
        }
      }
      this.buyProductsFinal["total_price"] = this.totalPrice;
    }
    console.log(this.buyProductsFinal);
    this._http.put<any>(this.urlUserMenu, this.buyProductsFinal, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}})
      .subscribe((response) => {
        
          this.refresh();
      
        });
  }

}
