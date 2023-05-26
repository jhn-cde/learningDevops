import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'dictionary',
    loadChildren: () => 
      import('./dictionary/dictionary.module').then(
        (mod) => mod.DictionaryModule
      )
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{
    useHash: false
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
