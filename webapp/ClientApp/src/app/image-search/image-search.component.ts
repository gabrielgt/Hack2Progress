import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-image-search',
  templateUrl: './image-search.component.html',
  styleUrls: ['./image-search.component.css']
})
export class ImageSearchComponent implements OnInit {
  public images: string[];
  public selectedImages: number[];
  public searchTerm: string;

  private httpClient: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.selectedImages = [];
  }

  ngOnInit() {}

  public onKeyEnter() {
    this.onClickSearchButton();
  }

  public onClickSearchButton() {
    if (this.searchTerm === undefined || this.searchTerm === null || this.searchTerm === '') {
      return;
    }

    this.httpClient.get<string[]>(this.baseUrl + 'api/BingApiSearch/SearchImages?searchTerm=' + this.searchTerm).subscribe(
      result => {
        this.selectedImages = [];
        this.images = result;
      },
      error => console.error(error)
    );
  }

  public onClickSendButton() {
    this.httpClient
      .post<string[]>(
        this.baseUrl + 'api/BingApiSearch/SendSelectedImages',
        this.images.filter((image, index) => this.selectedImages.includes(index))
      )
      .subscribe(
        result => {
          this.selectedImages = [];
        },
        error => console.error(error)
      );
  }

  public onImageClick(imageIndex: number) {
    const index: number = this.selectedImages.indexOf(imageIndex);
    if (index >= 0) {
      this.selectedImages.splice(index, 1);
    } else {
      this.selectedImages.push(imageIndex);
    }
  }

  public isImageSelected(imageIndex: number) {
    return this.selectedImages.includes(imageIndex);
  }
}
