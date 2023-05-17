import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { StudentApiService, StudentInterface } from './services/student.api.service';
import { StudentCourseApiService, StudentCourseInterface } from './services/student-course.api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  students: StudentInterface[] = [];
  courses: StudentCourseInterface[] = [];

  formGroup: FormGroup = this.fb.group({
    id: this.fb.control('', [Validators.required]),
    firstName: this.fb.control('', [Validators.required]),
    lastName: this.fb.control('', [Validators.required])
  });
  formGroupCourses: FormGroup = this.fb.group({
    studentId: this.fb.control('', [Validators.required]),
    courseName: this.fb.control('', [Validators.required])
  })

  constructor(
    private fb: FormBuilder,
    private studentService: StudentApiService,
    private studentCourseService: StudentCourseApiService
  ){}

  ngOnInit(): void {
    this.getStudents();
    this.getStudentCourses();
  }

  get id(): FormControl {
    return this.formGroup.get('id') as FormControl;
  }
  get firstName(): FormControl {
    return this.formGroup.get('firstName') as FormControl;
  }
  get lastName(): FormControl {
    return this.formGroup.get('lastName') as FormControl;
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
  addStudent(){
    const ans = this.studentService.inserStudent({
      "id": parseInt(this.id.value),
      "firstName": this.firstName.value,
      "lastName": this.lastName.value
    })
    .subscribe((student) => {
      console.log(student);
      this.getStudents();
    })
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
