import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Location } from '@angular/common';


@Component({
  selector: 'app-update-product',
  templateUrl: './update-product.component.html',
  styleUrls: ['./update-product.component.css']
})
export class UpdateProductComponent implements OnInit {

  public myProducts;
  public urlProducts = "https://localhost:5001/api/products";
  public error = "";
  public displayError = "none";
  constructor(public _UserService : UserService, public _http : HttpClient, public _router : Router, public _location : Location) { }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {

    this._http.get<any>(this.urlProducts)
    .subscribe({next : data => {
        this.myProducts = data;
    }});

  }

  updateProduct(id, professorPrice, studentPrice, weight, description ) {
    if(id == ""){
      this.displayError = "block";
      this.error = "Nu ati selectat niciun produs, incercati din nou!";
      
    }
    else {
      var data = {"id" : id, "professor_price" : professorPrice, "student_price" : studentPrice, "weight" : weight, "description" : description};
      console.log(data);
      this._http.put<any>(this.urlProducts,  data, {headers : {'Accept' : 'application/json', 'Content-Type' : 'application/json'}})
      .subscribe({next : data =>{
        console.log(data);
      }});
      this.refresh();

    }
    
  }

  refresh() {
    this._router.navigateByUrl('.', { skipLocationChange: true }).then(() => {
      this._router.navigate([decodeURI(this._location.path())]);
  }); 
}



}
