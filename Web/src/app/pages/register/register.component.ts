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
  public checkProfessor = false;
  public checkStudent = false;
  public displayMessage = "none";
  public message = "";
  constructor(private _UserService : UserService, private _router : Router) { }


  ngOnInit(): void {
  }

  register(email, password, confirmPassword) {
    var type;
    if(this.checkProfessor == true && this.checkStudent == true){
      this.error = "Trebuie să alegeți o singură categorie de utilizator!";
      this.displayError = "block";
    }
    if (this.checkProfessor == true){
      type = "professor";
    }
    else if (this.checkStudent == true){
      type = "student";
    }
    else {
      type = "normal";
    }

    if (password != confirmPassword) {
      this.displayError = "block";
      this.error = "Parolele nu se potrivesc!";
    }
    else if (password.length < 8) {
      this.displayError = "block";
      this.error = "Parola este prea mică! Trebuie să conțină minim 8 caractere.";
    }
    else if (password.length > 16) {
      this.displayError = "block";
      this.error = "Parola este prea mare! Trebuie să conțină maxim 20 caractere!"
    } 
    else {
      var passwordMD5 = Md5.hashStr(password);
      console.log(passwordMD5);
      this._UserService.register(email, passwordMD5, type)
      .subscribe({next:data =>
      {
        if(data["response"] == "true"){
          // this.error = "V-ati atutentificat cu succes! Vezi fi redirectionat catre pagina de Login";
          // DelayNode(300);
          this.displayError = "none";
          this.message = "Te-ai înregistrat cu succes! Vei fi redirecționat către pagina de autentificare."
          this.displayMessage = "block";
          window.scroll(0, 0);

          this._router.navigate(['/login']);
        }
        else if (data["response"] == "false")
        {
          this.displayError = "block";
          this.error = "Adresa de email deja există!";
        }
      }})
    }
  }
}
