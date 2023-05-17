import { TestBed } from '@angular/core/testing';

import { StudentCourseApiService } from './student-course.api.service';

describe('StudentCourseApiService', () => {
  let service: StudentCourseApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StudentCourseApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
