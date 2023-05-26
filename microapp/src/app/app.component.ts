import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DictionariesApiService, DictionaryInterface } from './services/dictionaries.api.service';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(){}
}
