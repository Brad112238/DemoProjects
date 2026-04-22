import { Component, signal, computed, OnInit, AfterViewChecked } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserCreditService } from '../../services/user-credit.service';
import { InvoiceValidator } from '../../utils/invoice-validator';
import { CreateTradeTokenRequest } from '../../models/user-credit.model';

declare const ECPay: any;

type Step = 'fill' | 'confirm' | 'payment';

interface FormData {
  pointAmount: number;
  invoiceType: '個人發票' | '公司發票';
  email: string;
  carruerNum: string;
  companyTitle: string;
  companyTaxId: string;
  companyAddr: string;
}

@Component({
  selector: 'app-top-up',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './top-up.component.html',
  styleUrl: './top-up.component.css'
})
export class TopUpComponent implements OnInit, AfterViewChecked {
  private pendingToken: string | null = null;
  // TODO: 實際應用中從登入狀態取得
  userId = 1;
  hostId = 1;
  userName = signal('使用者');
  userBalance = signal(0);

  step = signal<Step>('fill');
  loading = signal(false);
  errors = signal<string[]>([]);
  amountMsg = signal('');

  amounts = [500, 1000, 2000, 3000, 4000, 5000];
  selectedAmount = signal(500);

  form: FormData = {
    pointAmount: 500,
    invoiceType: '個人發票',
    email: '',
    carruerNum: '',
    companyTitle: '',
    companyTaxId: '',
    companyAddr: ''
  };

  isPersonal = computed(() => this.form.invoiceType === '個人發票');

  // 確認頁面顯示用
  confirmItems = signal<{ name: string; value: string; highlight?: boolean }[]>([]);

  private ecpayInitialized = false;
  private invoiceValidator = new InvoiceValidator();

  constructor(private userCreditService: UserCreditService) {}

  ngAfterViewChecked() {
    if (this.pendingToken && this.step() === 'payment') {
      const el = document.getElementById('ECPayPayment');
      if (el) {
        const token = this.pendingToken;
        this.pendingToken = null;
        this.initEcpayPayment(token);
      }
    }
  }

  ngOnInit() {
    this.userCreditService.getUserCredit(this.userId).subscribe({
      next: (credit) => {
        this.userName.set(credit.smsName || '使用者');
        this.userBalance.set(credit.amount);
      },
      error: () => {}
    });
  }

  selectAmount(amount: number) {
    this.selectedAmount.set(amount);
    this.form.pointAmount = amount;
    this.amountMsg.set('');
  }

  onAmountBlur() {
    const value = this.form.pointAmount;
    // 檢查是否為預設金額
    if (!this.amounts.includes(value)) {
      this.selectedAmount.set(0);
    } else {
      this.selectedAmount.set(value);
    }

    if (value < 500) {
      this.amountMsg.set('儲值金額不能低於 500 元');
    } else if (value >= 200000) {
      this.amountMsg.set('儲值金額不能高於 20 萬元');
    } else {
      this.amountMsg.set('');
    }
  }

  switchInvoice(type: '個人發票' | '公司發票') {
    this.form.invoiceType = type;
  }

  onCarruerBlur() {
    this.form.carruerNum = this.form.carruerNum.toUpperCase();
  }

  // 驗證 → 進入確認頁
  goToConfirm() {
    const errorList: string[] = [];
    const amount = this.form.pointAmount;

    if (isNaN(amount) || amount < 500 || amount >= 200000 || !Number.isInteger(amount)) {
      errorList.push('儲值金額：請輸入 500 元以上且少於 20 萬的整數金額');
    }

    const emailRegex = /^\w+((-\w+)|(\.\w+))*@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/;
    if (!this.form.email.trim()) {
      errorList.push('請填寫發票信箱');
    } else if (!emailRegex.test(this.form.email.trim())) {
      errorList.push('發票信箱格式錯誤');
    }

    if (this.form.invoiceType === '個人發票') {
      if (this.form.carruerNum.length > 0) {
        const carruerRegex = /^\/[0-9A-Z.+-]{7}$/;
        if (!carruerRegex.test(this.form.carruerNum)) {
          errorList.push('載具編號格式錯誤');
        }
      }
    } else {
      if (!this.form.companyTitle.trim()) {
        errorList.push('請填寫公司名稱');
      }
      if (!this.form.companyTaxId.trim()) {
        errorList.push('請填寫統一編號');
      } else if (!this.invoiceValidator.chkInvoiceId(this.form.companyTaxId.trim())) {
        errorList.push('統一編號格式錯誤');
      }
      if (!this.form.companyAddr.trim()) {
        errorList.push('請填寫公司地址');
      }
    }

    if (errorList.length > 0) {
      this.errors.set(errorList);
      return;
    }

    this.errors.set([]);
    this.buildConfirmItems();
    this.step.set('confirm');
  }

  private buildConfirmItems() {
    const items: { name: string; value: string; highlight?: boolean }[] = [
      { name: '儲值金額', value: String(this.form.pointAmount), highlight: true },
      { name: '發票類型', value: this.form.invoiceType },
      { name: '發票信箱', value: this.form.email }
    ];

    if (this.form.invoiceType === '個人發票') {
      items.push({ name: '載具編號', value: this.form.carruerNum || '綠界載具' });
    } else {
      items.push({ name: '公司名稱', value: this.form.companyTitle });
      items.push({ name: '統一編號', value: this.form.companyTaxId });
    }

    this.confirmItems.set(items);
  }

  goBack() {
    this.step.set('fill');
  }

  // 送出表單 → 取得 Token → 顯示 ECPay 付款頁
  submitOrder() {
    const request: CreateTradeTokenRequest = {
      userId: this.userId,
      hostId: this.hostId,
      pointAmount: String(this.form.pointAmount),
      email: this.form.email,
      invoiceType: this.form.invoiceType,
      carruerNum: this.form.carruerNum || undefined,
      companyTitle: this.form.companyTitle || undefined,
      companyTaxId: this.form.companyTaxId || undefined,
      companyAddr: this.form.companyAddr || undefined
    };

    this.loading.set(true);
    this.userCreditService.createTradeToken(request).subscribe({
      next: (res) => {
        if (res.success) {
          this.pendingToken = res.data.token;
          this.step.set('payment');
        } else {
          this.errors.set([res.message || '建立交易失敗']);
          this.step.set('fill');
        }
      },
      error: (err) => {
        this.loading.set(false);
        this.errors.set(['發生錯誤：' + (err.error?.message || err.message)]);
      }
    });
  }

  private initEcpayPayment(token: string) {
    const environment = 'Stage'; // 'Prod' or 'Stage'

    const doCreatePayment = () => {
      try {
        ECPay.createPayment(token, ECPay.Language.zhTW, (paymentErrMsg: string) => {
          this.loading.set(false);
          if (paymentErrMsg) {
            console.error('建立付款頁失敗:', paymentErrMsg);
          }
        }, 'V2');
      } catch (err) {
        console.error(err);
        this.loading.set(false);
      }
    };

    if (this.ecpayInitialized) {
      doCreatePayment();
    } else {
      ECPay.initialize(environment, 1, (initErrMsg: string) => {
        if (initErrMsg) {
          console.error('初始化失敗:', initErrMsg);
          this.loading.set(false);
          return;
        }
        this.ecpayInitialized = true;
        doCreatePayment();
      });
    }
  }

  // 確認付款
  confirmPayment() {
    this.loading.set(true);
    try {
      ECPay.getPayToken((paymentInfo: any, errMsg: string) => {
        if (errMsg != null) {
          this.loading.set(false);
          return;
        }

        const { PayToken, MerchantTradeNo } = paymentInfo;
        this.userCreditService.createPayment(PayToken, MerchantTradeNo).subscribe({
          next: (res) => {
            if (res.success) {
              window.open(res.data.threeDURL);
              // 清掉 ECPay 付款頁 DOM
              const ecpayEl = document.getElementById('ECPayPayment');
              if (ecpayEl) ecpayEl.innerHTML = '';
              // 重置回填寫表單頁
              this.step.set('fill');
              this.loading.set(false);
              this.form.pointAmount = 500;
              this.selectedAmount.set(500);
              this.errors.set([]);
              this.amountMsg.set('');
              // 重新取得餘額
              this.ngOnInit();
            }
          },
          error: (err) => {
            this.loading.set(false);
            alert('發生錯誤：' + (err.error?.message || err.message));
          }
        });
      });
    } catch (err) {
      console.error(err);
      this.loading.set(false);
    }
  }
}
