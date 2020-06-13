import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { UserComponent } from './pages/user/user.component';
import { AdminComponent } from './pages/admin/admin.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { AdminAdaugareAdminComponent } from './pages/admin-adaugare-admin/admin-adaugare-admin.component';
import { AdminAdaugareProdusComponent } from './pages/admin-adaugare-produs/admin-adaugare-produs.component';
import { AdminCreateMenuComponent } from './pages/admin-create-menu/admin-create-menu.component';
import { UpdateProductComponent } from './pages/update-product/update-product.component';
import { ProductDetailsComponent } from './pages/product-details/product-details.component';
import { HistoryComponent } from './pages/history/history.component';
import { NavBarComponent } from './pages/nav-bar/nav-bar.component';
import { AuthGuard } from './services/auth/auth.guard';


const routes: Routes = [
  {path : '', redirectTo : "/login", pathMatch : "full" },
  {path : 'login', component: LoginComponent},
  {path : 'register', component : RegisterComponent},
  {path : 'user', component : UserComponent, canActivate: [AuthGuard]},
  {path : 'admin', component : AdminComponent},
  {path : 'admin-adaugare-admin', component : AdminAdaugareAdminComponent, canActivate: [AuthGuard]},
  {path : 'admin-create-menu', component : AdminCreateMenuComponent, canActivate: [AuthGuard]},
  {path : 'admin-adaugare-produs', component : AdminAdaugareProdusComponent, canActivate: [AuthGuard]},
  {path : 'product-details', component : ProductDetailsComponent, canActivate : [AuthGuard]},
  {path : 'update-product', component: UpdateProductComponent, canActivate : [AuthGuard]},
  {path : 'history', component : HistoryComponent, canActivate : [AuthGuard]},
  {path : 'nav-bar', component : NavBarComponent},
  {path: "**", component : PageNotFoundComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [LoginComponent,
                                  RegisterComponent,
                                  UserComponent,
                                  AdminComponent,
                                  PageNotFoundComponent,
                                  AdminAdaugareAdminComponent,
                                  AdminAdaugareProdusComponent,
                                  AdminCreateMenuComponent,
                                  ProductDetailsComponent,
                                  UpdateProductComponent,
                                  NavBarComponent
                                ];