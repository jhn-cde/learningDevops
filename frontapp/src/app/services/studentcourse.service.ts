import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, catchError } from 'rxjs';

export interface StudentCourseInterface{
  courseName: string,
  studentId: number
}

@Injectable({
  providedIn: 'root'
})
export class StudentcourseService {
  
  private url = '';
  constructor( private http: HttpClient ) { 
    this.url = `/schoolapi/StudentCourse`;
  }
  
  getStudentCourses(): Observable<StudentCourseInterface[]> {
    return this.http.get<StudentCourseInterface[]>(this.url)
    .pipe(
      catchError((err) => {
        console.log('error', err);
        return [];
      })
    );
  }
  inserStudentCourse(course: StudentCourseInterface):Observable<StudentCourseInterface>{
    const res = this.http.post<StudentCourseInterface>(this.url, course)
    .pipe(
      catchError((err) => {
        console.log('error!', err);
        return [];
      })
    );
    return res;
  }
}
