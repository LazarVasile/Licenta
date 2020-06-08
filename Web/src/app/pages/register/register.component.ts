import { Component, OnInit } from '@angular/core';
import { Md5 } from 'ts-md5/dist/md5';
import { UserService } from 'src/app/services/user/user.service';
import { Router } from '@angular/router';
import { I18nSelectPipe } from '@angular/common';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public error = "";
  public displayError = "none";
  constructor(private _UserService : UserService, private _router : Router) { }


  ngOnInit(): void {
  }

  register(email, password, confirmPassword) {
    if (password != confirmPassword) {
      this.displayError = "block";
      this.error = "Password and confirm password doesn't match!";
    }
    else if (password.length < 8) {
      this.displayError = "block";
      this.error = "Password is too short, should have minimum 8 characters!";
    }
    else if (password.length > 16) {
      this.displayError = "block";
      this.error = "Password is too long, should have maximum 20 characters!"
    } 
    else {
      var passwordMD5 = Md5.hashStr(password);
      console.log(passwordMD5);
      this._UserService.register(email, passwordMD5)
      .subscribe({next:data =>
      {
        if(data["response"] == "true"){
          // this.error = "V-ati atutentificat cu succes! Vezi fi redirectionat catre pagina de Login";
          // DelayNode(300);
          this._router.navigate(['/login']);
        }
        else if (data["response"] == "false")
        {
          this.displayError = "block";
          this.error = "Email already exists! Please enter another email adress!";
          console.log("Inregistrarea nu a avut loc!");
        }
      }})
    }
  }
}
