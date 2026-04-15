import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JsonPipe } from '@angular/common';
import { RouterOutlet, RouterLink } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [JsonPipe, RouterOutlet, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  private apiBase = '/api';

  userCredits = signal<any[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  constructor(private http: HttpClient) {}

  getUserCredits() {
    this.loading.set(true);
    this.error.set(null);
    this.http.get<any[]>(`${this.apiBase}/usercredit`).subscribe({
      next: (res) => {
        this.userCredits.set(res);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('UserCredit API 呼叫失敗: ' + err.message);
        this.loading.set(false);
      }
    });
  }
}
