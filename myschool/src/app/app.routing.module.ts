import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'school',
    loadChildren: () => 
      import('./students/students.module').then(
        (mod) => mod.StudentsModule
      )
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