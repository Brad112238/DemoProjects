import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [JsonPipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private apiBase = '/api';

  healthResult = signal<any>(null);
  userCredits = signal<any[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  constructor(private http: HttpClient) {}

  checkHealth() {
    this.loading.set(true);
    this.error.set(null);
    this.http.get(`${this.apiBase}/health`).subscribe({
      next: (res) => {
        this.healthResult.set(res);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Health API 呼叫失敗: ' + err.message);
        this.loading.set(false);
      }
    });
  }

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
