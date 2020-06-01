import { Component, OnInit, Inject } from '@angular/core';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import {Product, SellProduct} from '../../classes/product';
import { DatePipe } from '@angular/common';


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
  public categories = {"Ciorbe si supe / Soups" : [], "Garnituri / Side dishes" : [], "Felul II" : [], "Desert / Deserts" :[], "Salate / Salads" : [], "Paine / Bread" : [], "Bauturi / Drinks": []};

  constructor(public _UserService : UserService, public _http: HttpClient) { }
  

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

    this.totalPrice = Number((this.totalPrice + product.student_price).toFixed(2)); 
    // this.totalPrice = this.totalPrice.toFixed(2);
    for(let i = 0; i < this.sellProducts.length; i++){
      // console.log(id,this.sellProducts[i].product.id);
      if (product._id == this.sellProducts[i].product._id){
        this.sellProducts[i].cantity +=1;
        ok = 1;
        break
      }

    }
      if (ok == 0){
        this.sellProducts.push(new SellProduct(product));
      }
    console.log(this.totalPrice);
  }

}
