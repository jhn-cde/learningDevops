import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface StudentCourseInterface{
  courseName: string,
  studentId: number
}

@Injectable({
  providedIn: 'root'
})
export class StudentCourseApiService {
  private url = '';
  constructor( private http: HttpClient ) { 
    this.url = `${environment.apiUrl}/StudentCourse`;
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
