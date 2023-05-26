import { SelectionModel } from '@angular/cdk/collections';
import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DictionariesApiService, DictionaryInterface } from 'src/app/services/dictionaries.api.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

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
