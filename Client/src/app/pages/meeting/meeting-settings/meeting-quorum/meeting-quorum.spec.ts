import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingQuorum } from './meeting-quorum';

describe('MeetingQuorum', () => {
  let component: MeetingQuorum;
  let fixture: ComponentFixture<MeetingQuorum>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeetingQuorum]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MeetingQuorum);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
