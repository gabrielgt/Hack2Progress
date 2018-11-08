import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessedImagesComponent } from './processed-images.component';

describe('ProcessedImagesComponent', () => {
  let component: ProcessedImagesComponent;
  let fixture: ComponentFixture<ProcessedImagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessedImagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessedImagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
