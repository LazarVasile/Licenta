import { Component, OnInit } from '@angular/core';
import { Router, ParamMap, ActivatedRoute } from '@angular/router';
import { ThrowStmt } from '@angular/compiler';
import { UserService } from 'src/app/services/user/user.service';
import { ProductService } from '../../services/products/product.service';
import { DatePipe } from '@angular/common';
import { HttpClient} from '@angular/common/http';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  public username;
  public typeUser;
  public pipe = new DatePipe('en-US'); // Use your own locale
  public formatDate = this.pipe.transform(Date.now(), "yyyy-MM-dd")
  public myProducts = [];
  private _url = "https://localhost:5001/api/usermenu/";
  public categories = ["Ciorbe si supe / Soups", "Garnituri / Side dishes", "Felul II", "Desert / Deserts", "Salate / Salads", "Paine / Bread", "Bauturi / Drinks"];
  public productsByCategory = {"Ciorbe si supe / Soups" : [], "Garnituri / Side dishes" : [], "Felul II" : [], "Desert / Deserts" :[], "Salate / Salads" : [], "Paine / Bread" : [], "Bauturi / Drinks": []};
  
  constructor(private _router : Router, private _http: HttpClient,  private _route : ActivatedRoute, public _UserService : UserService, public _productService : ProductService) {
    // this.myDate = this._datePipe.transform(this.myDate, "yyyy-MM-dd");
   }

  ngOnInit(): void {
   this.username = localStorage.getItem("loggedUser");
   this.typeUser = localStorage.getItem("type");
  //  this.categories = this._productService.getCategories();
  console.log("dsaa");
   this.getProducts(this.formatDate);

  }



  getProducts(date) {
    // console.log(date);
    var url = this._url + date;
    // console.log(url);    
    this._http.get<any>(url)
    .subscribe(data =>
    {
      this.myProducts = data;
      for (let i = 0; i < this.myProducts.length; i++){
        this.productsByCategory[this.myProducts[i].category].push(this.myProducts[i]);
      }
      console.log(this.productsByCategory);
      // console.log("dskkk");
      // console.log(this.myProducts);
    },
    );

  }

}
