import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/products/product.service';
import { UserService } from '../../services/user/user.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-admin-adaugare-produs',
  templateUrl: './admin-adaugare-produs.component.html',
  styleUrls: ['./admin-adaugare-produs.component.css']
})
export class AdminAdaugareProdusComponent implements OnInit {

  public activeButton;
  public categories;
  // public name = "";
  // public category = "";
  // public professor_price;
  // public student_price;
  // public weight;
  // public description = "";
  public displayError = "";
  public message = "";

  constructor(private _productService : ProductService, public _UserService : UserService, private _router : Router, private _location : Location) { }

  ngOnInit(): void {
    this.activeButton = true;
    this.categories = this._productService.getCategories();
  }

  addProduct(name, category, professor_price, student_price, weight, description) {
    console.log(description);
      this._productService.addProduct(name, category, professor_price, student_price, weight, description)
      .subscribe({next : data =>
      {
        console.log(data);
        if (data == "succes"){
          this.message = "The product has been added successfully!";
        }
        else if (data == "error") {
          this.displayError = "Product already exists!";
        }
      },
      error:error => {
        return console.error('There was an error!', error);}
      })
      
      this.refresh();
  }
  
  refresh() {
      this._router.navigateByUrl('.', { skipLocationChange: true }).then(() => {
        this._router.navigate([decodeURI(this._location.path())]);
    }); 
  }
  
}
