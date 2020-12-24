import { ProdutosListagemComponent } from './produtos-listagem/produtos-listagem.component';
import { ImportListagemComponent } from './import-listagem/import-listagem.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path:"",redirectTo:"lotes",pathMatch:"full"
  },
  {
    path:"lotes", component: ImportListagemComponent
  },
  {
    path:"produtos",
    children: [
      { path: '', component: ProdutosListagemComponent },
      { path: ':id', component: ProdutosListagemComponent },
    ]
  }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
