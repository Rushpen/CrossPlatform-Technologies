import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http'; // Импортируем HttpClient для запросов

@Component({
  selector: 'app-books-page',
  templateUrl: './books-page.component.html',
  styleUrls: ['./books-page.component.css']
})
export class BooksPageComponent implements OnInit {

  books: any[] = [];  // Массив для хранения книг
  isLoading: boolean = true;  // Флаг загрузки данных

  constructor(private http: HttpClient) { }  // Внедряем HttpClient

  ngOnInit(): void {
    // Запрашиваем данные с API при инициализации компонента
    this.http.get<any[]>('/api/Books').subscribe(
      (data) => {
        this.books = data;  // Сохраняем полученные данные
        this.isLoading = false;  // Останавливаем индикатор загрузки
      },
      (error) => {
        console.error('Error fetching books:', error);
        this.isLoading = false;  // Останавливаем индикатор загрузки в случае ошибки
      }
    );
  }
}
