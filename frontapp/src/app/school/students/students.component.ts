import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { StudentInterface, StudentService } from 'src/app/services/student.service';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {
  students: StudentInterface[] = [];
  
  formGroup: FormGroup = this.fb.group({
    id: this.fb.control('', [Validators.required]),
    firstName: this.fb.control('', [Validators.required]),
    lastName: this.fb.control('', [Validators.required])
  });

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService
  ){}
  ngOnInit(): void {
    this.getStudents();
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
}
