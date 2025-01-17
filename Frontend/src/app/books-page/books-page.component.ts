import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-books-page',
  standalone: true,
  imports: [CommonModule, HttpClientModule], // Подключаем HttpClientModule
  templateUrl: './books-page.component.html',
  styleUrls: ['./books-page.component.css']
})
export class BooksPageComponent {
  books: any[] = [];
  isLoading: boolean = true;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('/api/Books').subscribe(
      (data) => {
        console.log('Полученные данные:', data);
        this.books = data;
        this.isLoading = false;
      },
      (error) => {
        console.error('Ошибка при загрузке данных:', error);
        this.isLoading = false;
      }
    );
  }
}
