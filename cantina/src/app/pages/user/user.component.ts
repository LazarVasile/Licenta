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
  public categories;
  public pipe = new DatePipe('en-US'); // Use your own locale
  public formatDate = this.pipe.transform(Date.now(), "yyyy-MM-dd")
  private _url = "https://localhost:5001/api/usermenu";
  
  constructor(private _router : Router, private _http: HttpClient,  private _route : ActivatedRoute, public _UserService : UserService, public _productService : ProductService) {
    // this.myDate = this._datePipe.transform(this.myDate, "yyyy-MM-dd");
   }

  ngOnInit(): void {
   this.username = localStorage.getItem("loggedUser");
   this.typeUser = localStorage.getItem("type");
   this.categories = this._productService.getCategories();
   this.getProducts(this.formatDate);

  }

  getProducts(date) {
    var data = {"myDate" : date};
    this._http.post<any>(this._url, data, {headers:{'Accept' : 'application/json', 'Content-Type' : 'application/json'}})
    .subscribe({next:data =>
    {
      console.log(data);
    },
    })
  }

}
