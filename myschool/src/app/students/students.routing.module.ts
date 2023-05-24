import { RouterModule, Routes } from '@angular/router';
import { StudentsComponent } from './students/students.component';
import { StudentcoursesComponent } from './studentcourses/studentcourses.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: '',
    component: StudentsComponent
  },
  {
    path: 'courses',
    component: StudentcoursesComponent
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class StudentsRoutingModule {}