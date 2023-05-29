import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { StudentInterface, StudentService } from 'src/app/services/student.service';
import { StudentCourseInterface, StudentcourseService } from 'src/app/services/studentcourse.service';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.css']
})
export class CoursesComponent implements OnInit{
  students: StudentInterface[] = [];
  courses: StudentCourseInterface[] = [];

  formGroupCourses: FormGroup = this.fb.group({
    studentId: this.fb.control('', [Validators.required]),
    courseName: this.fb.control('', [Validators.required])
  })

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private studentCourseService: StudentcourseService
  ){}

  ngOnInit(): void {
    this.getStudents();
    this.getStudentCourses();
  }
  
  get studentId(): FormControl {
    return this.formGroupCourses.get('studentId') as FormControl;
  }
  get courseName(): FormControl {
    return this.formGroupCourses.get('courseName') as FormControl;
  }

  getStudents(){
    this.studentService.getStudents()
    .subscribe((students) => {
      this.students = students;
      console.log(students)
    });
  }
  getStudentCourses(){
    this.studentCourseService.getStudentCourses()
    .subscribe((courses) => {
      this.courses = courses;
      console.log(courses)
    })
  }
  addStudentCourse(){
    if(this.students.findIndex(student => student.id === parseInt(this.studentId.value))<0){
      console.log('studentId does not exist');
      return;
    }

    const ans = this.studentCourseService.inserStudentCourse({
      "courseName": this.courseName.value,
      "studentId": parseInt(this.studentId.value)
    })
    .subscribe((course) => {
      console.log(course);
      this.getStudentCourses();
    })
  }
}
