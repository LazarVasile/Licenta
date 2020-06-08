import { Component, OnInit } from '@angular/core';
import { Router, ParamMap, ActivatedRoute } from '@angular/router';
import { ThrowStmt } from '@angular/compiler';
import { UserService } from 'src/app/services/user/user.service';
import { ProductService } from '../../services/products/product.service';
import { DatePipe } from '@angular/common';
import { HttpClient} from '@angular/common/http';
import { IMenu } from '../../services/menus/imenu';
import { IProduct } from 'src/app/services/products/products';
import { MAY } from '@angular/material/core';
import { resolve } from 'dns';
import { Menu } from 'src/app/classes/menu';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  public username;
  public typeUser;
  public pipe = new DatePipe('en-US'); // Use your own locale
  public formatDate = this.pipe.transform(Date.now(), "dd-MM-yyyy");
  public myProducts = [];
  public myMenu = [];
  private _url = "https://localhost:5001/api/usermenu/";
  public categories;
  public productsByCategory : {[category : string] : Array<IProduct>} = {};
  public buyProductsNumber : {[id : string] : number} = {};
  public buyProductsFinal: {[id : string] : number} = {};
  public buyProductsTotal : {[id : string] : number} = {}; 
  public error;
  public displayError = "none";
  public totalPrice = 0;
  public _urlCodes = "https://localhost:5001/api/codes";
  public code : number;
  public displayCode;

  constructor(private _router : Router, private _http: HttpClient,  private _route : ActivatedRoute, public _UserService : UserService, public _productService : ProductService) {
    this.categories = _productService.getCategories();
   }

  ngOnInit(): void {
    
    this.displayCode = "none";
    this.username = localStorage.getItem("loggedUser");
    this.typeUser = localStorage.getItem("type");
    this.initCategories();
    this.getProducts(this.formatDate);
  }



  getProducts(date) {
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
      // .toPromise()
      // .then((data : any) => {
      //    this.myProducts = data;
      //    for (let i = 0; i < this.myProducts.length; i++){
      //     this.productsByCategory[this.myProducts[i].category].push(this.myProducts[i]);
      //    }
      //   }
      // );
      // console.log(this.myProducts);
      // console.log(this.productsByCategory);
      // });
      
    // return promise;

  }

  initCategories() {

    for (let i = 0; i < this.categories.length; i++){
      this.productsByCategory[this.categories[i]] = [];
    }
    
  }


  plusProduct(product) {
    if (this.buyProductsTotal[product["_id"].toString()] == 0){
      this.error = "Produsul nu mai este disponibil! Va rugam sa alegeti altceva!";
      this.displayError = "block";
    }
    else {
    // console.log(this.buyProducts);
      this.totalPrice = Number((this.totalPrice + product.student_price).toFixed(2)); 
      this.buyProductsTotal[product["_id"].toString()] -= 1;  
      this.buyProductsNumber[product['_id'].toString()] += 1;
      // console.log(this.buyProductsNumber);
      // console.log(this.buyProductsTotal);
    }
  }



  minusProduct(product) {
    if (this.buyProductsNumber[product['_id'].toString()] > 0){
      this.totalPrice = Number((this.totalPrice - product.student_price).toFixed(2)); 
      this.buyProductsTotal[product['_id'].toString()] += 1;
      this.buyProductsNumber[product["_id"].toString()] -= 1;
    }
  }

  buyProducts() {
    for (let i  = 0; i < this.myProducts.length; i++){
      if (this.buyProductsNumber[this.myProducts[i]['_id'].toString()] > 0){
        this.buyProductsFinal[this.myProducts[i]['_id'].toString()] = this.buyProductsNumber[this.myProducts[i]['_id'].toString()];
      }
    }
    console.log(this.buyProductsFinal);
    
    this._http.post<any>(this._urlCodes, this.buyProductsFinal, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}})
    .subscribe({next:data =>{
      console.log(data);
      if (data["response"] == "true"){
        this.code = data['code'];
        this.displayCode = "block";
      }
    }});
  }


}

