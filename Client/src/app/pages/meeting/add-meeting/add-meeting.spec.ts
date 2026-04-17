import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMeeting } from './add-meeting';

describe('AddMeeting', () => {
  let component: AddMeeting;
  let fixture: ComponentFixture<AddMeeting>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddMeeting]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddMeeting);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
