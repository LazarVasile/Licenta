import { Component, OnInit } from '@angular/core';
import { Md5 } from 'ts-md5/dist/md5';
import { UserService } from '../../services/user/user.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public response = "";
  public displayError = "none";
  public error = "";
  constructor(private _UserService : UserService, private _router : Router) { }

  ngOnInit(): void {
  }

  loginUser(username, password) {
    var passwordMD5 = Md5.hashStr(password);
    this._UserService.login(username, passwordMD5)
    .subscribe({next:data => 
      {  
        console.log(data);
        if(data['response'] == "true") {
          // console.log(data['response']);
          localStorage.setItem('token', data['token']);
          localStorage.setItem('loggedUser', username);
          localStorage.setItem('id_user', data["id_user"]);
          if (data['role'] == "user"){
            localStorage.setItem('type', data['type']);
            this._router.navigate(['/user']);
          }
          else if (data['role'] == "admin"){
            if(data['type'] == "admin")
            {
              this._router.navigate(['/admin-create-menu']);
            }
            else {
              this._router.navigate(['/admin']);
            }
          }
          // this.redirectToUser(username, passwordMD5);
        }
        else {
          this.error = "Nume sau parolă greșită!";
          this.displayError = "block";
          window.scroll(0,0);
        }
      },
    error:error => {
      this.error = "A intervenit o eroare. Încearcați din nou!";
      this.displayError = "block";
      window.scroll(0, 0);
    }
    })
  }

  redirectToUser(_username, _password) {
    this._router.navigate(['/user', {username : _username, password : _password}]);
  }


}
