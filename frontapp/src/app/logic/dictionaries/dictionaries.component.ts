import { Component, OnInit } from '@angular/core';
import { DictionaryInterface, DictionaryService } from 'src/app/services/dictionary.service';

@Component({
  selector: 'app-dictionaries',
  templateUrl: './dictionaries.component.html',
  styleUrls: ['./dictionaries.component.css']
})
export class DictionariesComponent implements OnInit {
  dictionaries: DictionaryInterface[] = [];
  constructor(
    private dictionaryService: DictionaryService
  ){}

  ngOnInit(): void {
    this.dictionaryService.getDictionaries()
    .subscribe((dics) => {
      this.dictionaries = dics;
    });
  }
}
