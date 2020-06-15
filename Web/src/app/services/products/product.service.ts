import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Router } from '@angular/router';
import { IProduct } from './products';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  public _urlProducts = "https://localhost:5001/api/products";
  public categories = ["Ciorbe si supe / Soups", "Garnituri / Side dishes", "Felul II", "Desert / Deserts", "Salate / Salads", "Paine / Bread", "Bauturi / Drinks"];
  public token = localStorage.getItem("token");
  constructor(private http : HttpClient, private _router : Router) { }


  getProducts() {
    return this.http.get<IProduct[]>(this._urlProducts, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json', 'Authorization':'Bearer '+ this.token}});
  }

  getCategories() {
    return this.categories;
  }

  getUrlProducts(){
    console.log(this._urlProducts);
    return this._urlProducts;
  }
}
