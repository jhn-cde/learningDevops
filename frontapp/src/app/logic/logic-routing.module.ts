import { RouterModule, Routes } from '@angular/router';
import { DictionariesComponent } from './dictionaries/dictionaries.component';
import { NgModule } from '@angular/core';
import { PageNotFoundComponent } from '../page-not-found/page-not-found.component';

const routes: Routes = [
  {
    path: '',
    component: DictionariesComponent
  },
  {
    path: '**',
    component: PageNotFoundComponent
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class LogicRoutingModule {}