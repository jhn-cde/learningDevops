import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentsModule } from './students/students.module';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => 
      import('./students/students.module').then(
        (mod) => mod.StudentsModule
      )
  },
  {
    path: 'students',
    redirectTo: ''
  }
]

@NgModule({
  imports: [
    RouterModule.forRoot(routes,{
      useHash: false
    }),
  ],
  exports: [RouterModule]
})

export class AppRoutingModule {}