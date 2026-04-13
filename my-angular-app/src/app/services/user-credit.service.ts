import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  AppResult,
  CreateTradeTokenRequest,
  CreateTradeTokenResponse,
  CreatePaymentResponse,
  UserCredit
} from '../models/user-credit.model';

@Injectable({ providedIn: 'root' })
export class UserCreditService {
  private apiBase = '/api/usercredit';

  constructor(private http: HttpClient) {}

  getUserCredit(userId: number): Observable<UserCredit> {
    return this.http.get<UserCredit>(`${this.apiBase}/${userId}`);
  }

  createTradeToken(request: CreateTradeTokenRequest): Observable<AppResult<CreateTradeTokenResponse>> {
    return this.http.post<AppResult<CreateTradeTokenResponse>>(`${this.apiBase}/trade-token`, request);
  }

  createPayment(payToken: string, merchantTradeNo: string): Observable<AppResult<CreatePaymentResponse>> {
    return this.http.post<AppResult<CreatePaymentResponse>>(`${this.apiBase}/create-payment`, {
      payToken,
      merchantTradeNo
    });
  }
}
