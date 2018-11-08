import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-processed-images',
  templateUrl: './processed-images.component.html',
  styleUrls: ['./processed-images.component.css']
})
export class ProcessedImagesComponent implements OnInit {
  public processedImages: string[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.getProcessedImages();
  }

  private getProcessedImages() {
    this.http.get<string[]>(this.baseUrl + 'api/ProcessedImages/GetProcessedImages').subscribe(
      processedImages => {
        this.processedImages = processedImages;
      },
      error => console.error(error)
    );
  }
}
