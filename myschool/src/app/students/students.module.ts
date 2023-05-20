import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { StudentsComponent } from './students/students.component';
import { StudentcoursesComponent } from './studentcourses/studentcourses.component';
import { StudentsRoutingModule } from './students.routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DictionariesComponent } from './dictionaries/dictionaries.component';

@NgModule({
  declarations: [
    StudentsComponent,
    StudentcoursesComponent,
    DictionariesComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    StudentsRoutingModule,

    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatListModule,
    MatDividerModule,
    MatTableModule,
    MatCheckboxModule
  ]
})
export class StudentsModule { }
