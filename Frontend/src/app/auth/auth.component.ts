import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common'; 

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    HttpClientModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatDialogModule,
  ],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent {
  authForm: FormGroup;
  role: string | null = null; 
  isAuthError: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<AuthComponent>,
    private http: HttpClient,
    private fb: FormBuilder
  ) {
    this.authForm = this.fb.group({
      login: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.authForm.valid) {
      const loginData = this.authForm.value;
      console.log('Данные формы:', loginData);
  
      this.http.post('http://localhost:4200/api/authorization/login', loginData).subscribe({
        next: (response: any) => {
          console.log('Успех:', response);
  
          if (response.token) {
            const token = response.token;
            console.log('Токен:', token);
  
            const decodedToken = this.decodeToken(token);
            console.log('Декодированный токен:', decodedToken);

            const role = this.getRoleFromToken(decodedToken);
  
            if (role) {
              this.role = role;
              this.dialogRef.close(this.role);
              this.isAuthError = false;
            } else {
              console.error('Роль не найдена в токене');
              this.isAuthError = true;
            }
          } else {
            console.error('Токен не был передан');
            this.isAuthError = true;
          }
        },
        error: (error) => {
          console.error('Ошибка:', error);
          this.isAuthError = true;
        }
      });
    } else {
      console.log('Форма не валидна!');
    }
  }
  
  decodeToken(token: string): any {
    const payload = token.split('.')[1];
    const decoded = atob(payload);
    return JSON.parse(decoded);
  }
  
  getRoleFromToken(decodedToken: any): string | null {
    const roleKeys = [
      'role',
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role',
    ];
  
    for (let key of roleKeys) {
      if (decodedToken[key]) {
        return decodedToken[key];
      }
    }
    return null;
  }
  
}
