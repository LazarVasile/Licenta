import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Router } from '@angular/router';
import { IProduct } from './products';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private _urlProducts = "https://localhost:5001/api/products";
  public categories = ["Ciorbe si supe / Soups", "Garnituri / Side dishes", "Felul II", "Desert / Deserts", "Salate / Salads", "Paine / Bread", "Bauturi / Drinks"];

  constructor(private http : HttpClient, private _router : Router) { }

  addProduct(name, category, professor_price, student_price, weight, description) {
    let data = {"name" : name, "category" : category, "professor_price" : Number(professor_price), "student_price" : Number(student_price), "weight" : Number(weight), "description" : description};
    console.log(data);
    return this.http.post<any>(this._urlProducts, data, {headers:{'Accept' : 'application/json', 'Content-Type' : 'application/json'}});
  }

  getProducts() {
    return this.http.get<IProduct[]>(this._urlProducts);
  }

  getCategories() {
    return this.categories;
  }
}
