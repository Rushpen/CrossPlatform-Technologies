// home-page.component.ts
import { Component } from '@angular/core';
import { HeaderComponent } from '../header/header.component';
import { MenuComponent } from '../menu/menu.component';
import { FooterComponent } from '../footer/footer.component';
import { AuthComponent } from '../auth/auth.component';
import { MatDialog } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [HeaderComponent, MenuComponent, FooterComponent],
  template: `
    <app-header [userRole]="userRole"></app-header>
    <app-menu></app-menu>
    <img src="library.jpg" class="image1">
    <app-footer></app-footer>
  `,
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent {
  title = 'Lab2Frontend';
  userRole: string | null = null;
  constructor(public dialog: MatDialog) {}

  openAuthDialog(): void {
    const dialogRef = this.dialog.open(AuthComponent);
  
    dialogRef.afterClosed().subscribe((role: string | undefined) => {
      if (role) {
        this.userRole = role;  // Роль сохраняется в переменную
        console.log('Пользователь вошел как:', role);  // Для отладки
      } else {
        console.log('Пользователь не авторизован');
        this.userRole = null;  // Очистить роль при ошибке
      }
    });
  }
  
}
