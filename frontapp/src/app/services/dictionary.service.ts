import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, catchError } from 'rxjs';

export interface DictionaryInterface{
  Id: number,
  Name: string,
  Value: string,
  Childs: DictionaryInterface[]
}

@Injectable({
  providedIn: 'root'
})
export class DictionaryService {

  private url = '';
  constructor( private http: HttpClient ) {
    this.url = `/logicapi/Dictionary`;
  }

  getDictionaries(): Observable<DictionaryInterface[]> {
    return this.http.get<DictionaryInterface[]>(`${this.url}/GetAll`)
    .pipe(
      catchError((err) => {
        console.log('error', err);
        return [];
      })
    );
  }
}
