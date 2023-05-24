import { TestBed } from '@angular/core/testing';

import { DictionariesApiService } from './dictionaries.api.service';

describe('DictionariesApiService', () => {
  let service: DictionariesApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DictionariesApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
