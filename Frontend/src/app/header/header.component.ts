import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthComponent } from '../auth/auth.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-header',
  imports: [MatDialogModule, MatButtonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  title = 'OnlineLibrary';
  constructor(public dialog: MatDialog) {}

  openAuthDialog(): void {
    this.dialog.open(AuthComponent, {
      width: '400px',  // Можно настроить размер окна
    });
  }
  
}
