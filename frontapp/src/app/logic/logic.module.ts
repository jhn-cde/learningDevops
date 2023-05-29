import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DictionariesComponent } from './dictionaries/dictionaries.component';
import { HttpClientModule } from '@angular/common/http';
import { LogicRoutingModule } from './logic-routing.module';



@NgModule({
  declarations: [
    DictionariesComponent
  ],
  imports: [
    CommonModule,
    LogicRoutingModule,
    HttpClientModule
  ]
})
export class LogicModule { }
