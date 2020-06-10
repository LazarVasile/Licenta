import { Component, OnInit, Inject } from '@angular/core';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import {Product, SellProduct, BuyProduct} from '../../classes/product';
import { DatePipe } from '@angular/common';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Route } from '@angular/compiler/src/core';

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
  public formatDate = this.pipe.transform(Date.now(), "dd-MM-yyyy")
  public myProducts = [];
  public sellProducts = [];
  public totalPrice = 0;
  public categories = {"Ciorbe si supe / Soups" : [], "Garnituri / Side dishes" : [], "Felul II" : [], "Desert / Deserts" :[], "Salate / Salads" : [], "Paine / Bread" : [], "Bauturi / Drinks": []};
  public urlCodes = "https://localhost:5001/api/codes/";
  public urlUserMenu = "https://localhost:5001/api/menus";
  public ticketProducts = [];
  public buyProducts = [];
  

  constructor(public _UserService : UserService, public _http: HttpClient, public _location : Location, public _router : Router) { }
  

  ngOnInit(): void {
    this.activeButton = true;
    this.username = localStorage.getItem("loggedUser");
    this.getProducts(this.formatDate);

  }

  getProducts(date) {
    // console.log(date);
    var url = this._url + date;
    // console.log(url);    
    this._http.get<any>(url)
    .subscribe({next: data =>
    {
      this.myProducts = data;
      console.log(this.myProducts);
    },
    })

  }
  


  addProduct(product) {
    console.log(product._id, product.student_price)
    let ok = 0;

    // this.totalPrice = this.totalPrice.toFixed(2);
    for(let i = 0; i < this.buyProducts.length; i++){
      // console.log(id,this.sellProducts[i].product.id);
      if (product._id == this.buyProducts[i]['id']){
        this.totalPrice = Number((this.totalPrice + product.student_price).toFixed(2)); 
        this.buyProducts[i]['quantity'] +=1;
        ok = 1;
        break
      }

    }
      if (ok == 0){
        var buyProduct : {[id : string] : any} = {};

        this.totalPrice = Number((this.totalPrice + product.student_price).toFixed(2)); 
        buyProduct['_id'] = 0;
        buyProduct['idProduct'] = product._id;
        buyProduct['code'] = 0;
        buyProduct['quantity'] = 1;
        this.buyProducts.push(buyProduct);

      }
  }

  GenerateTicket(value) {
    console.log("value: " + value);
    this._http.get<any>(this.urlCodes + value)
    .subscribe({next: dataGet => {
      this.buyProducts = dataGet;
      let price = 0;
      for (let i = 0; i < dataGet.length; i++) {
        console.log(dataGet[i])
        for (let j = 0; j < this.myProducts.length; j++){
          if (dataGet[i]['idProduct'] == this.myProducts[j]['_id']){
            price = price + (dataGet[i]['quantity'] * this.myProducts[j]["student_price"]);
            this.ticketProducts.push(this.myProducts[j]);
          } 
        }
      }
      this.totalPrice = price;
      console.log("produse tickete:");
      console.log(this.ticketProducts);
      this.myProducts = this.ticketProducts;
    }});    
  }

  refresh() {
    this._router.navigateByUrl('.', { skipLocationChange: true }).then(() => {
      this._router.navigate([decodeURI(this._location.path())]);
  }); 
  }

  BuyProducts(){ 
    console.log(this.buyProducts);
    this._http.put<any>(this.urlUserMenu, this.buyProducts, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}})
      .subscribe((response) => {
        
          this.refresh();
      
        });
  }

}
