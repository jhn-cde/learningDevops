import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

const routes: Routes = [
  {
    path: 'school',
    loadChildren: () => 
      import('./school/school.module').then(
        (mod) => mod.SchoolModule
      )
  },
  {
    path: 'logic',
    loadChildren: () => 
      import('./logic/logic.module').then(
        (mod) => mod.LogicModule
      )
  },
  {
    path: '',
    redirectTo: 'school',
    pathMatch: 'full'
  },
  {
    path: '**',
    component: PageNotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
