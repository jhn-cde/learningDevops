import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface DictionaryInterface{
  id: number,
  name: string,
  value: string,
  fatherId?: number
}

@Injectable({
  providedIn: 'root'
})
export class DictionariesApiService {
  private url = '';
  constructor( private http: HttpClient ) {
    this.url = `${environment.apiUrl}/Dictionary`;
  }

  getDictionaries(): Observable<DictionaryInterface[]> {
    return this.http.get<DictionaryInterface[]>(this.url)
    .pipe(
      catchError((err) => {
        console.log('error', err);
        return [];
      })
    );
  }
  insertDictionary(dictionary: DictionaryInterface):Observable<DictionaryInterface>{
    const res = this.http.post<DictionaryInterface>(this.url, dictionary)
    .pipe(
      catchError((err) => {
        console.log('error!', err);
        return [];
      })
    );
    return res;
  }
}
