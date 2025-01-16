import { Routes } from '@angular/router';
import { BooksPageComponent } from './books-page/books-page.component';
import { AuthorsPageComponent } from './authors-page/authors-page.component';
import { UsersPageComponent } from './users-page/users-page.component';
import { HomePageComponent } from './home-page/home-page.component'; // Новый компонент

export const routes: Routes = [
  { path: '', component: HomePageComponent }, // Главная страница
  { path: 'api/Books', component: BooksPageComponent },
  { path: 'api/Authors', component: AuthorsPageComponent },
  { path: 'api/Users', component: UsersPageComponent },
];

