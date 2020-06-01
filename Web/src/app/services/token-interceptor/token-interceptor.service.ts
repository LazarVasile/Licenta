import { Injectable, Injector } from '@angular/core';
import { HttpInterceptor } from '@angular/common/http'; 
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {

  constructor(private _injector: Injector) { }

  intercept(req, next) {
    let userService = this._injector.get(UserService);

    let tokenizedReq = req.clone({
        setHeaders: {
          Authorization:`Bearer ${userService.getToken()}`
        }
      })
      return next.handle(tokenizedReq);
  }
}   