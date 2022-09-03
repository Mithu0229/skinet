import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { HomeComponent } from './home/home.component';


const routes: Routes = [
  {path:'', component:HomeComponent,data:{breadcrumb:'Home'}},
  {path:'shop',loadChildren:()=>import('./shop/shop.module').then(map=>map.ShopModule),data:{breadcrumb:'Shop'}},
  {path:'basket',loadChildren:()=>import('./basket/basket.module').then(map=>map.BasketModule),data:{breadcrumb:'Basket'}},
  {path:'checkout',loadChildren:()=>import('./checkout/checkout.module').then(map=>map.CheckoutModule),data:{breadcrumb:'Checkout'}},

  {path:'not-found', component:NotFoundComponent,data:{breadcrumb:'not-found'}},
  {path:'server-error', component:ServerErrorComponent,data:{breadcrumb:'server-error'}},
  {path:'**',redirectTo:'not-found',pathMatch:'full'},
 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
