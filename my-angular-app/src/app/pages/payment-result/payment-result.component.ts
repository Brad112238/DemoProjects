import { Component, signal, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-payment-result',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './payment-result.component.html',
  styleUrl: './payment-result.component.css'
})
export class PaymentResultComponent implements OnInit {
  status = signal<'success' | 'fail' | 'unknown'>('unknown');
  message = signal('交易已完成，請稍候確認結果。');
  tradeNo = signal('');

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const rtnCode = params['RtnCode'];
      const rtnMsg = params['RtnMsg'];
      const merchantTradeNo = params['MerchantTradeNo'];

      if (merchantTradeNo) {
        this.tradeNo.set(merchantTradeNo);
      }

      if (rtnCode === '1') {
        this.status.set('success');
        this.message.set('付款成功！您的餘額將於稍後更新。');
      } else if (rtnCode) {
        this.status.set('fail');
        this.message.set(rtnMsg || '付款失敗，請稍後再試。');
      }
    });
  }
}
