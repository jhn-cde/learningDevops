import { RouterModule, Routes } from '@angular/router';
import { StudentsComponent } from './students/students.component';
import { StudentcoursesComponent } from './studentcourses/studentcourses.component';
import { NgModule } from '@angular/core';
import { DictionariesComponent } from './dictionaries/dictionaries.component';

const routes: Routes = [
  {
    path: '',
    component: StudentsComponent
  },
  {
    path: 'courses',
    component: StudentcoursesComponent
  },
  {
    path: 'dictionaries',
    component: DictionariesComponent
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class StudentsRoutingModule {}