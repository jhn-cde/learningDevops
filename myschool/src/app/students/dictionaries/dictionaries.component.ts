import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DictionariesApiService, DictionaryInterface } from 'src/app/services/dictionaries.api.service';

const ELEMENT_DATA: DictionaryInterface[] = [
  {id: 1, name: 'item 1', value: 'vk', fatherId: undefined},
  {id: 3, name: 'item 3', value: 'vk', fatherId: 1},
  {id: 4, name: 'item 4', value: 'vk', fatherId: 1},
  {id: 5, name: 'item 5', value: 'vk', fatherId: 3},
  {id: 2, name: 'item 2', value: 'vk', fatherId: undefined},
  {id: 6, name: 'item 6', value: 'vk', fatherId: 2}
];

@Component({
  selector: 'app-dictionaries',
  templateUrl: './dictionaries.component.html',
  styleUrls: ['./dictionaries.component.css']
})
export class DictionariesComponent implements OnInit{
  displayedColumns: string[] = ['select','id', 'name', 'value', 'fatherId'];
  dataSource: MatTableDataSource<DictionaryInterface> = new MatTableDataSource<DictionaryInterface>([]);
  selection = new SelectionModel<DictionaryInterface>(true, []);
  
  constructor(
    private dictionaryService: DictionariesApiService
  ){}

  ngOnInit(): void {
    this.dictionaryService.getDictionaries()
    .subscribe((dictionaries) => {
      this.dataSource.data = dictionaries;
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);
  }

  checkboxLabel(row?: DictionaryInterface): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
  }
}
