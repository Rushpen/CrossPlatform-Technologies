import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthComponent } from '../auth/auth.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [MatDialogModule, MatButtonModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  @Input() userRole: string | null = null;
  title = 'OnlineLibrary';
  constructor(public dialog: MatDialog) {}

  openAuthDialog(): void {
    const dialogRef = this.dialog.open(AuthComponent, {
      width: '400px',  // Размер окна
    });

    dialogRef.afterClosed().subscribe((role: string | null) => {
      if (role) {
        this.userRole = role;  // Обновляем роль пользователя
      }
    });
  }
  
}
