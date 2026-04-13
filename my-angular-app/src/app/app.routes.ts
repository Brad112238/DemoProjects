import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'top-up',
    loadComponent: () => import('./pages/top-up/top-up.component').then(m => m.TopUpComponent)
  },
  {
    path: 'payment/result',
    loadComponent: () => import('./pages/payment-result/payment-result.component').then(m => m.PaymentResultComponent)
  }
];
