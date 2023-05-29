import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, catchError } from 'rxjs';

export interface StudentInterface{
  id: number;
  firstName: string;
  lastName: string;
}

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private url = '';
  constructor( private http: HttpClient ) {
    this.url = `/schoolapi/Student`;
  }

  getStudents(): Observable<StudentInterface[]> {
    return this.http.get<StudentInterface[]>(this.url)
    .pipe(
      catchError((err) => {
        console.log('error', err);
        return [];
      })
    );
  }
  inserStudent(student: StudentInterface):Observable<StudentInterface>{
    const res = this.http.post<StudentInterface>(this.url, student)
    .pipe(
      catchError((err) => {
        console.log('error!', err);
        return [];
      })
    );
    return res;
  }
}
